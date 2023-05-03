using UnityEngine;
using UnityEngine.UI;
public class ScoreDisplay : MonoBehaviour
{
    //Unity 文本对象变量
    private static Text _scoreText;
    private void Awake()
    {
        //取得分数文本对象的文本组件
        _scoreText = GetComponent<Text>();
    }
    private void Start()
    {
        //调用重置分数函数
        ScoreManager.Instance.ResetScore();
    }
    
    //更新文本静态函数，便于被其他类调用
    public static void UpdateText(int score)=>_scoreText.text = score.ToString();
    //文本缩放静态函数，修改分数文本矩形变换组件的缩放值
    public static void ScaleText(Vector3 targetScale) => _scoreText.rectTransform.localScale = targetScale;
}
