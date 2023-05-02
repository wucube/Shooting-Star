using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

public static class SaveSystem
{
    public static void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(path,json);

        try
        {
#if UNITY_EDITOR
            Debug.Log($"Susscessfully saved data to {path}.");
#endif
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to save data to {path}. \n{exception}");
#endif
        }
        
    }
    
    public static T LoadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to load data from {path}. \n{exception}");
#endif
            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to delete {path}. \n{exception}");
#endif
        }
    }

    //检测存档文件是否存在
    public static bool SaveFileExists(string saveFileName)
    {
        //获取存档文件路径
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        //检测文件是否存在
        return File.Exists(path);
    }
}