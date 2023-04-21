using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 动态波数UI
/// </summary>
public class DynamicWaveUI : MonoBehaviour
{
    /// <summary>
    /// 动画播放的时长
    /// </summary>
    [SerializeField] private float animationTime = 1f;

    [Header("---- LINE MOVE ----")]
    /// <summary>
    /// 顶部线条开始的位置
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector2 lineTopStartPosition = new Vector2(-1250f, 140f);
    /// <summary>
    /// 顶部线条的目标位置
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector2 lineTopTargetPosition = new Vector2(0f, 140f);
    /// <summary>
    /// 底部线条开始的位置
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector2 lineBottomStartPosition = new Vector2(1250f, 0f);
    /// <summary>
    /// 底部线条的目标位置
    /// </summary>
    [SerializeField] private Vector2 lineBottomTargetPosition = Vector2.zero;

    [Header("---- TEXT SCALE ----")]
    /// <summary>
    /// 波数文本的初始缩放值
    /// </summary>
    /// <returns></returns>
    [SerializeField] private Vector2 waveTextStartScale = new Vector2(1f, 0f);
    /// <summary>
    /// 波数文本的目标缩放值
    /// </summary>
    [SerializeField] private Vector2 waveTextTargetScale = Vector2.one;
    
    /// <summary>
    /// 顶部线条的Rect
    /// </summary>
    private RectTransform lineTop;
    /// <summary>
    /// 底部线条的Rect
    /// </summary>
    private RectTransform lineBottom;
    /// <summary>
    /// 波数文本的Rect
    /// </summary>
    private RectTransform waveText;
    /// <summary>
    /// UI停留在画面中间的时长
    /// </summary>
    private WaitForSeconds waitStayTime;
    void Awake()
    {
        if (TryGetComponent(out Animator animator))
            if(animator.isActiveAndEnabled) Destroy((this));
        
        waitStayTime = new WaitForSeconds(EnemyManager.Instance.TimeBetweenWaves - animationTime * 2f);
        
        lineTop = transform.Find("Line Top").GetComponent<RectTransform>();
        lineBottom = transform.Find("Line Bottom").GetComponent<RectTransform>();
        waveText = transform.Find("Wave Text").GetComponent<RectTransform>();
        
        lineTop.localPosition = lineTopStartPosition;
        lineBottom.localPosition = lineBottomStartPosition;
        waveText.localScale = waveTextStartScale;
    }
    private void OnEnable()
    {
        StartCoroutine(LineMoveCoroutine(lineTop, lineTopTargetPosition, lineTopStartPosition));
        
        StartCoroutine(LineMoveCoroutine(lineBottom, lineBottomTargetPosition, lineBottomStartPosition));
        
        StartCoroutine(TextScaleCoroutine(waveText, waveTextTargetScale, waveTextStartScale));
    }

    #region LINE MOVE
    /// <summary>
    /// 线的移动协程
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="targetPosition"></param>
    /// <param name="startPosition"></param>
    /// <returns></returns>
    IEnumerator LineMoveCoroutine(RectTransform rect, Vector2 targetPosition, Vector2 startPosition)
    {
        yield return StartCoroutine(UIMoveCoroutine(rect, targetPosition));
        yield return waitStayTime;
        yield return StartCoroutine(UIMoveCoroutine(rect, startPosition));
    }
    /// <summary>
    /// UI移动协程
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    IEnumerator UIMoveCoroutine(RectTransform rect, Vector2 position)
    {
        float t = 0f;
        Vector2 localPosition = rect.localPosition;
        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localPosition = Vector2.Lerp(localPosition, position, t);
            yield return null;
        }
    }
    #endregion

    #region TEXT SCALE
    /// <summary>
    /// 文本缩放协程
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="targetScale"></param>
    /// <param name="startScale"></param>
    /// <returns></returns>
    IEnumerator TextScaleCoroutine(RectTransform rect, Vector2 targetScale, Vector2 startScale)
    {
        
        yield return StartCoroutine(UIScaleCoroutine(rect, targetScale));
        yield return waitStayTime;
        yield return StartCoroutine(UIScaleCoroutine(rect, startScale));
    }
    /// <summary>
    /// UI缩放协程
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    IEnumerator UIScaleCoroutine(RectTransform rect, Vector2 scale)
    {
        float t = 0f;
        Vector2 localScale = rect.localScale;
        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            rect.localScale = Vector2.Lerp(localScale, scale, t);
            yield return null;
        }
    }
    #endregion
}
