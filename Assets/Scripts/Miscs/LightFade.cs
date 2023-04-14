using System.Collections;
using UnityEngine;

/// <summary>
/// 光线渐暗
/// </summary>
class LightFade : MonoBehaviour
{
    /// <summary>
    /// 渐暗时间
    /// </summary>
    [SerializeField] float fadeDuration = 1f;

    /// <summary>
    /// 是否延迟
    /// </summary>
    [SerializeField] bool delay = false;

    /// <summary>
    /// 延迟时间
    /// </summary>
    [SerializeField] float delayTime = 0f;

    /// <summary>
    /// 光线初始亮度
    /// </summary>
    [SerializeField] float startIntensity = 30f;

    /// <summary>
    /// 光线最终亮度
    /// </summary>
    [SerializeField] float finalIntensity = 0f;

    new Light light;
    
    /// <summary>
    /// 等待延迟时间
    /// </summary>
    WaitForSeconds waitDelayTime;

    void Awake()
    {
        light = GetComponent<Light>();

        waitDelayTime = new WaitForSeconds(delayTime);
    }

    void OnEnable()
    {
        StartCoroutine(nameof(LightCoroutine));
    }
    
    /// <summary>
    /// 光线渐暗协程
    /// </summary>
    /// <returns></returns>
    IEnumerator LightCoroutine()
    {
        if (delay)
            yield return waitDelayTime;

        light.intensity = startIntensity;
        light.enabled = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime / fadeDuration;
            light.intensity = Mathf.Lerp(startIntensity, finalIntensity, t / fadeDuration);

            yield return null;
        }

        light.enabled = false;
    }
}