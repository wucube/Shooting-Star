using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 场景加载器
/// </summary>
public class SceneLoader : PersistentSingleton<SceneLoader>
{
    /// <summary>
    /// 用于场景加载的过渡图片
    /// </summary>
    [SerializeField] private Image transitionImage;

    /// <summary>
    /// 图片淡出时间
    /// </summary>
    [SerializeField] private float fadeTime = 2f;

    private Color color;
    
    //场景名称字符串场景
    private const string Gameplay = "GamePlay";
    private const string MainMenu = "MainMenu";
    private const string Scoring = "Scoring";
    
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    /// <summary>
    /// 加载GamePlay场景
    /// </summary>
    public void LoadGamePlayScene()
    {
        StartCoroutine(LoadingCoroutine(Gameplay));;
    }
    /// <summary>
    /// 异步加载场景的协程
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator LoadingCoroutine(string sceneName)
    {
        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        //不激活加载的场景
        loadingOperation.allowSceneActivation = false;
        //激活过渡的图片对象
        transitionImage.gameObject.SetActive(true);
        
        while (color.a < 1)
        {
            //逐渐增加图片的不透明度
            color.a = Mathf.Clamp01(color.a += Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;

            yield return null;
        }
        //若 加载的场景进度超过0.9f 就执行下一步操作
        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);
        //激活加载的场景
        loadingOperation.allowSceneActivation = true;

        while (color.a>0)
        {   
            //逐渐降低图片的不透明度
            color.a = Mathf.Clamp01(color.a -= Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = color;
            yield return null;
        }
        //隐藏场景加载的过渡图片
        transitionImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// 加载主菜单场景
    /// </summary>
    public void LoadMainMenuScene()
    {
        //先停止所有协程
        StopAllCoroutines();

        StartCoroutine(LoadingCoroutine(MainMenu));
    }

    /// <summary>
    /// 加载排行榜场景
    /// </summary>
    public void LoadScoringScene()
    {
        //先停止所有协程
        StopAllCoroutines();
        
        StartCoroutine(LoadingCoroutine(Scoring));
    }
}
