﻿using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace BaseFramework
{


    /// <summary>
    /// Json数据管理类 主要用于进行 Json的序列化存储到硬盘 和 反序列化从硬盘中读取到内存中
    /// </summary>
    public class BaseJsonDataManager : BaseSingletonInCSharp<BaseJsonDataManager>
    {
        /// <summary>
        /// 数据存储路径
        /// </summary>
        private static string DATA_SAVE_PATH = Application.persistentDataPath + "/Data/Json/";

        /// <summary>
        /// 配置存储路径
        /// </summary>
        public static string CONFIG_SAVE_PATH = Application.streamingAssetsPath + "/Data/Json/";


        //存储Json数据 序列化
        public void SaveData(object data, string fileName, EBaseJsonType type = EBaseJsonType.LitJson)
        {
            //确定存储路径
            string path = DATA_SAVE_PATH + fileName + ".json";
            //序列化 得到Json字符串
            string jsonStr = "";
            switch (type)
            {
                case EBaseJsonType.JsonUtlity:
                    jsonStr = JsonUtility.ToJson(data);
                    break;
                case EBaseJsonType.LitJson:
                    jsonStr = JsonMapper.ToJson(data);
                    break;
            }

            //把序列化的Json字符串 存储到指定路径的文件中
            File.WriteAllText(path, jsonStr);
        }

        //读取指定文件中的 Json数据 反序列化
        public T LoadData<T>(string fileName, EBaseJsonType type = EBaseJsonType.LitJson) where T : new()
        {
            //确定从哪个路径读取
            //首先先判断 默认数据文件夹中是否有我们想要的数据 如果有 就从中获取
            string path = CONFIG_SAVE_PATH + fileName + ".json";
            //先判断 是否存在这个文件
            //如果不存在默认文件 就从 读写文件夹中去寻找
            if (!File.Exists(path))
                path = DATA_SAVE_PATH + fileName + ".json";
            //如果读写文件夹中都还没有 那就返回一个默认对象
            if (!File.Exists(path))
                return new T();

            //进行反序列化
            string jsonStr = File.ReadAllText(path);
            //数据对象
            T data = default(T);
            switch (type)
            {
                case EBaseJsonType.JsonUtlity:
                    data = JsonUtility.FromJson<T>(jsonStr);
                    break;
                case EBaseJsonType.LitJson:
                    data = JsonMapper.ToObject<T>(jsonStr);
                    break;
            }

            //把对象返回出去
            return data;
        }
    }
}