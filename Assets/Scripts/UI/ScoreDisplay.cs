using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour
{
    //Unity �ı��������
    private static Text _scoreText;
    private void Awake()
    {
        //ȡ�÷����ı�������ı����
        _scoreText = GetComponent<Text>();
    }
    private void Start()
    {
        //�������÷�������
        ScoreManager.Instance.ResetScore();
    }
    
    //�����ı���̬���������ڱ����������
    public static void UpdateText(int score)=>_scoreText.text = score.ToString();
    //�ı����ž�̬�������޸ķ����ı����α任���������ֵ
    public static void ScaleText(Vector3 targetScale) => _scoreText.rectTransform.localScale = targetScale;
}
