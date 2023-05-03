using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 动态波数UI
/// </summary>
public class DynamicWaveUI : MonoBehaviour
{
    //横线移动和文本缩放的持续时间(动画持续时间)
    [SerializeField] private float animationTime = 1f;
    [Header("---- LINE MOVE ----")]
    //上横线的默认位置
    [SerializeField] private Vector2 lineTopStartPosition = new Vector2(-1250f, 140f);
    //上横线移动的目标位置
    [SerializeField] private Vector2 lineTopTargetPosition = new Vector2(0f, 140f);
    //下横线的默认位置
    [SerializeField] private Vector2 lineBottomStartPosition = new Vector2(1250f, 0f);
    //下横线移动的目标位置
    [SerializeField] private Vector2 lineBottomTargetPosition = Vector2.zero;

    [Header("---- TEXT SCALE ----")]
    //文本起始缩放值
    [SerializeField] private Vector2 waveTextStartScale = new Vector2(1f, 0f);
    //文本目标缩放值
    [SerializeField] private Vector2 waveTextTargetScale = Vector2.one;
    
    //波数UI上横线矩形变换组件
    private RectTransform _lineTop;
    //波数UI下横线矩形变换组件
    private RectTransform _lineBottom;
    //波数UI文本的矩形变换组件
    private RectTransform _waveText;
    //UI在画面中停留的等待时间变量
    private WaitForSeconds waitStayTime;
    void Awake()
    {
        //如果脚本挂载对象上有Animator组件
        if (TryGetComponent(out Animator animator))
            //如果用Animator实现动态UI，则在游戏开始时删除动态UI脚本
            if(animator.isActiveAndEnabled) Destroy((this));
        //敌人管理器每波间隔时间 减 两次横线、文本动画时间值 得到
        waitStayTime = new WaitForSeconds(EnemyManager.Instance.TimeBetweenWaves - animationTime * 2f);
        
        //获取上下横线及文本的矩形变换组件
        _lineTop = transform.Find("Line Top").GetComponent<RectTransform>();
        _lineBottom = transform.Find("Line Bottom").GetComponent<RectTransform>();
        _waveText = transform.Find("Wave Text").GetComponent<RectTransform>();
        //设置上下横线的初始位置和文本的初始缩放值
        _lineTop.localPosition = lineTopStartPosition;
        _lineBottom.localPosition = lineBottomStartPosition;
        _waveText.localScale = waveTextStartScale;
    }
    private void OnEnable()
    {
        //启用横线移动协程，移动上横线
        StartCoroutine(LineMoveCoroutine(_lineTop, lineTopTargetPosition, lineTopStartPosition));
        //启用横线移动协程，移动下横线
        StartCoroutine(LineMoveCoroutine(_lineBottom, lineBottomTargetPosition, lineBottomStartPosition));
        //启用文本缩放协程，缩放波数文本
        StartCoroutine(TextScaleCoroutine(_waveText, waveTextTargetScale, waveTextStartScale));
    }

    #region LINE MOVE
    //横线移动协程
    IEnumerator LineMoveCoroutine(RectTransform rect, Vector2 targetPosition, Vector2 startPosition)
    {
        //先将UI对象移动到目标位置
        yield return StartCoroutine(UIMoveCoroutine(rect, targetPosition));
        //UI对象停留一段时间
        yield return waitStayTime;
        //UI对象移动到初始位置
        yield return StartCoroutine(UIMoveCoroutine(rect, startPosition));
    }
    //UI移动协程
    IEnumerator UIMoveCoroutine(RectTransform rect, Vector2 position)
    {
        float t = 0f;
        //记录矩形初始位置
        Vector2 localPosition = rect.localPosition;
        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            //二维向量线性插值，将UI从起始位置移动到目标位置
            //改变UI的矩形变换组件位置，使用localPosition才会起作用
            rect.localPosition = Vector2.Lerp(localPosition, position, t);
            yield return null;
        }
    }
    #endregion

    #region TEXT SCALE
    //文本缩放协程
    IEnumerator TextScaleCoroutine(RectTransform rect, Vector2 targetScale, Vector2 startScale)
    {
        //先将文本矩形缩放到目标值
        yield return StartCoroutine(UIScaleCoroutine(rect, targetScale));
        //文本停留一段时间
        yield return waitStayTime;
        //文本矩形缩放到起始值
        yield return StartCoroutine(UIScaleCoroutine(rect, startScale));
    }
    //UI缩放协程
    IEnumerator UIScaleCoroutine(RectTransform rect, Vector2 scale)
    {
        float t = 0f;
        //记录矩形初始绽放值
        Vector2 localScale = rect.localScale;
        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            //改换矩形变换组件的缩放值
            rect.localScale = Vector2.Lerp(localScale, scale, t);
            yield return null;
        }
    }
    #endregion
}
