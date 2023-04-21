using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

/// <summary>
/// 存储系统
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// 通过Json存储数据
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <param name="data"></param>
    public static void SaveByJson(string saveFileName, object data)
    {
        //将数据类转为Josn
        var json = JsonUtility.ToJson(data);
        //获取存储文件在持久数据目录中的路径
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        //将Json数据写入到存储路径中
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
    /// <summary>
    /// 读取Json数据
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LoadFromJson<T>(string saveFileName)
    {
        //获取存储文件的路径
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            //读取Json数据
            var json = File.ReadAllText(path);
            //将Json数据转为对应的类
            var data = JsonUtility.FromJson<T>(json);
            //返回读取的类数据
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
    
    /// <summary>
    /// 删除存储的数据
    /// </summary>
    /// <param name="saveFileName"></param>
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

    /// <summary>
    /// 是否存在存储文件
    /// </summary>
    /// <param name="saveFileName"></param>
    /// <returns></returns>
    public static bool SaveFileExists(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        return File.Exists(path);
    }
}