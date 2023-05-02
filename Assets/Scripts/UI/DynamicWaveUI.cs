using System;
using System.Collections;
using UnityEngine;

public class DynamicWaveUI : MonoBehaviour
{
    //�����ƶ����ı����ŵĳ���ʱ��(��������ʱ��)
    [SerializeField] private float animationTime = 1f;
    [Header("---- LINE MOVE ----")]
    //�Ϻ��ߵ�Ĭ��λ��
    [SerializeField] private Vector2 lineTopStartPosition = new Vector2(-1250f, 140f);
    //�Ϻ����ƶ���Ŀ��λ��
    [SerializeField] private Vector2 lineTopTargetPosition = new Vector2(0f, 140f);
    //�º��ߵ�Ĭ��λ��
    [SerializeField] private Vector2 lineBottomStartPosition = new Vector2(1250f, 0f);
    //�º����ƶ���Ŀ��λ��
    [SerializeField] private Vector2 lineBottomTargetPosition = Vector2.zero;

    [Header("---- TEXT SCALE ----")]
    //�ı���ʼ����ֵ
    [SerializeField] private Vector2 waveTextStartScale = new Vector2(1f, 0f);
    //�ı�Ŀ������ֵ
    [SerializeField] private Vector2 waveTextTargetScale = Vector2.one;
    
    //����UI�Ϻ��߾��α任���
    private RectTransform _lineTop;
    //����UI�º��߾��α任���
    private RectTransform _lineBottom;
    //����UI�ı��ľ��α任���
    private RectTransform _waveText;
    //UI�ڻ�����ͣ���ĵȴ�ʱ�����
    private WaitForSeconds waitStayTime;
    void Awake()
    {
        //����ű����ض�������Animator���
        if (TryGetComponent(out Animator animator))
            //�����Animatorʵ�ֶ�̬UI��������Ϸ��ʼʱɾ����̬UI�ű�
            if(animator.isActiveAndEnabled) Destroy((this));
        //���˹�����ÿ�����ʱ�� �� ���κ��ߡ��ı�����ʱ��ֵ �õ�
        waitStayTime = new WaitForSeconds(EnemyManager.Instance.TimeBetweenWaves - animationTime * 2f);
        
        //��ȡ���º��߼��ı��ľ��α任���
        _lineTop = transform.Find("Line Top").GetComponent<RectTransform>();
        _lineBottom = transform.Find("Line Bottom").GetComponent<RectTransform>();
        _waveText = transform.Find("Wave Text").GetComponent<RectTransform>();
        //�������º��ߵĳ�ʼλ�ú��ı��ĳ�ʼ����ֵ
        _lineTop.localPosition = lineTopStartPosition;
        _lineBottom.localPosition = lineBottomStartPosition;
        _waveText.localScale = waveTextStartScale;
    }
    private void OnEnable()
    {
        //���ú����ƶ�Э�̣��ƶ��Ϻ���
        StartCoroutine(LineMoveCoroutine(_lineTop, lineTopTargetPosition, lineTopStartPosition));
        //���ú����ƶ�Э�̣��ƶ��º���
        StartCoroutine(LineMoveCoroutine(_lineBottom, lineBottomTargetPosition, lineBottomStartPosition));
        //�����ı�����Э�̣����Ų����ı�
        StartCoroutine(TextScaleCoroutine(_waveText, waveTextTargetScale, waveTextStartScale));
    }

    #region LINE MOVE
    //�����ƶ�Э��
    IEnumerator LineMoveCoroutine(RectTransform rect, Vector2 targetPosition, Vector2 startPosition)
    {
        //�Ƚ�UI�����ƶ���Ŀ��λ��
        yield return StartCoroutine(UIMoveCoroutine(rect, targetPosition));
        //UI����ͣ��һ��ʱ��
        yield return waitStayTime;
        //UI�����ƶ�����ʼλ��
        yield return StartCoroutine(UIMoveCoroutine(rect, startPosition));
    }
    //UI�ƶ�Э��
    IEnumerator UIMoveCoroutine(RectTransform rect, Vector2 position)
    {
        float t = 0f;
        //��¼���γ�ʼλ��
        Vector2 localPosition = rect.localPosition;
        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            //��ά�������Բ�ֵ����UI����ʼλ���ƶ���Ŀ��λ��
            //�ı�UI�ľ��α任���λ�ã�ʹ��localPosition�Ż�������
            rect.localPosition = Vector2.Lerp(localPosition, position, t);
            yield return null;
        }
    }
    #endregion

    #region TEXT SCALE
    //�ı�����Э��
    IEnumerator TextScaleCoroutine(RectTransform rect, Vector2 targetScale, Vector2 startScale)
    {
        //�Ƚ��ı��������ŵ�Ŀ��ֵ
        yield return StartCoroutine(UIScaleCoroutine(rect, targetScale));
        //�ı�ͣ��һ��ʱ��
        yield return waitStayTime;
        //�ı��������ŵ���ʼֵ
        yield return StartCoroutine(UIScaleCoroutine(rect, startScale));
    }
    //UI����Э��
    IEnumerator UIScaleCoroutine(RectTransform rect, Vector2 scale)
    {
        float t = 0f;
        //��¼���γ�ʼ����ֵ
        Vector2 localScale = rect.localScale;
        while (t < 1f)
        {
            t += Time.deltaTime / animationTime;
            //�Ļ����α任���������ֵ
            rect.localScale = Vector2.Lerp(localScale, scale, t);
            yield return null;
        }
    }
    #endregion
}
