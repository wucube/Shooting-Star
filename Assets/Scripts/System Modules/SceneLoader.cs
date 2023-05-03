using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private Image transitionImage;
    [SerializeField] private float fadeTime = 2f;
    //
    private Color _color;
    //场景名字符串常量
    private const string Gameplay = "GamePlay";
    private const string MainMenu = "MainMenu";
    private const string Scoring = "Scoring";
    
    //场景加载函数
    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //加载GamePlay场景
    public void LoadGamePlayScene()
    {
        StartCoroutine(LoadingCoroutine(Gameplay));;
    }
    //场景加载协程
    IEnumerator LoadingCoroutine(string sceneName)
    {

        var loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;
        
        transitionImage.gameObject.SetActive(true);
       
        while (_color.a<1)
        {
            
            _color.a = Mathf.Clamp01(_color.a += Time.unscaledDeltaTime / fadeTime);
            
            transitionImage.color = _color;
            
            yield return null;
        }
        
        yield return new WaitUntil(() => loadingOperation.progress >= 0.9f);
        
        loadingOperation.allowSceneActivation = true;
        
       
        while (_color.a>0)
        {
            _color.a = Mathf.Clamp01(_color.a -= Time.unscaledDeltaTime / fadeTime);
            transitionImage.color = _color;
            yield return null;
        }
        
        transitionImage.gameObject.SetActive(false);
    }

    //加载主菜单场景
    public void LoadMainMenuScene()
    {
        //先停用所有加载协程
        StopAllCoroutines();
        //启用加载协程，加载主菜单场景
        StartCoroutine(LoadingCoroutine(MainMenu));
    }

    //加载计分场景
    public void LoadScoringScene()
    {
        //先停用场景加载器所有协程
        StopAllCoroutines();
        //再启用加载场景协程，加载计分场景
        StartCoroutine(LoadingCoroutine(Scoring));
    }
}
