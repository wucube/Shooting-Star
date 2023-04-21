using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 分数显示
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    /// <summary>
    /// 显示分数的文本组件
    /// </summary>
    private static Text scoreText;
    private void Awake()
    {
        scoreText = GetComponent<Text>();
    }
    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }
    
    /// <summary>
    /// 更新分数
    /// </summary>
    /// <param name="score"></param>
    public static void UpdateText(int score)=>scoreText.text = score.ToString();
    /// <summary>
    /// 缩放分数文本组件
    /// </summary>
    /// <param name="targetScale"></param>
    public static void ScaleText(Vector3 targetScale) => scoreText.rectTransform.localScale = targetScale;
}
