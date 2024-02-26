using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


//最终
/// <summary>
/// PlayerPrefs数据管理类 统一管理数据的存储和读取
/// </summary>
public class PlayerPrefsDataManager
{
    //管理类用单例模式
    private static PlayerPrefsDataManager instance = new PlayerPrefsDataManager();

    public static PlayerPrefsDataManager Instance
    {
        get
        {
            return instance;
        }
    }

    private PlayerPrefsDataManager()
    {

    }

    /// <summary>
    /// 存储数据
    /// </summary>
    /// <param name="data">数据对象</param>
    /// <param name="keyName">数据对象的唯一key 自己控制</param>
    public void SaveData( object data, string keyName )
    {
        Log("存储自定义类型" + keyName);
        //就是要通过 Type 得到传入数据对象的所有的 字段
        //然后结合 PlayerPrefs来进行存储

        #region 第一步 获取传入数据对象的所有字段
        //获得传进来的data的Type
        Type dataType = data.GetType();
        //得到dataType的所有字段的数组
        FieldInfo[] infos = dataType.GetFields();
        #endregion

        #region 第二步 自己定义一个key的规则 进行数据存储
        //我们存储都是通过PlayerPrefs来进行存储的
        //保证key的唯一性 我们就需要自己定一个key的规则

        //我们自己定一个规则
        // keyName_数据类型_字段类型_字段名
        #endregion

        #region 第三步 遍历所有字段 进行数据存储

        //设置临时变量 方便遍历字段数组使用使用
        string saveKeyName = "";
        FieldInfo info;

        //遍历字段数组
        for (int i = 0; i < infos.Length; i++)
        {
            //得到字段数组中每一个具体字段的信息
            //准备对每一个字段 进行数据存储
            info = infos[i];

            //通过FieldInfo可以直接获取到 字段的类型 和字段的名字
            //字段的类型 info.FieldType.Name
            //字段的名字 info.Name;

            //要根据我们定的key的拼接规则 来进行唯一key的生成
            //Player1_PlayerInfo_Int32_age
            saveKeyName = keyName + "_" + dataType.Name +
                "_" + info.FieldType.Name + "_" + info.Name;

            //现在得到了唯一Key 按照我们的规则
            //接下来就要来通过PlayerPrefs来进行存储
            //如何获取值 info类的GetValue方法传入具体对象data
            //info.GetValue(data)

            //封装一个专门来存储值的方法 
            SaveValue(info.GetValue(data), saveKeyName);
        }

        //立即存储
        PlayerPrefs.Save();
        #endregion
    }

   /// <summary>
   /// 存储具体值的方法
   /// </summary>
   /// <param name="value">值对象</param>
   /// <param name="keyName">值对象唯一的key 按自定义规则生成的</param>
    private void SaveValue(object value, string keyName)
    {
        //直接通过PlayerPrefs来进行存储了
        //就是根据数据类型的不同 来决定使用哪一个API来进行存储
        //PlayerPrefs只支持3种类型存储 
        //判断 数据类型 是什么类型 然后调用具体的方法来存储

        //得到要存储的值对象的类型
        Type fieldType = value.GetType();

        //用多个if堆值对象的类型进行判断
        //是不是int
        if (fieldType == typeof(int))
        {
            Log("存储int:" + keyName + "值 = " + (int)value);
            //为int数据加密
            int rValue = (int)value;
            rValue += 10;
            PlayerPrefs.SetInt(keyName, rValue);
        }
        //是不是float
        else if (fieldType == typeof(float))
        {
            Log("存储float:" + keyName + "值 = " + (float)value);
            PlayerPrefs.SetFloat(keyName, (float)value);
        }
        //是不是string
        else if (fieldType == typeof(string))
        {
            Log("存储string:" + keyName + "值 = " + value.ToString());
            PlayerPrefs.SetString(keyName, value.ToString());
        }
        //是不是bool
        else if (fieldType == typeof(bool))
        {
            Log("存储bool:" + keyName + "值 = " + ((bool)value));
            //自己定义一个存储bool的规则
            PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
        }
        //如何判断 泛型类的类型呢
        //通过反射 IsAssignableFrom方法判断 父子关系
        //这相当于是判断 这个字段是不是IList的子类 是的话基本上就是List类了
        else if ( typeof(IList).IsAssignableFrom(fieldType) )
        {
            //父类装子类 把要存储的值对象的类型转成IList在遍历
            IList list = value as IList;

            //先存储List中元素的数量 
            PlayerPrefs.SetInt(keyName, list.Count);
            Log("存储ListCount" + keyName + "值 = " + (list.Count));

            //声明一个index用做遍历List拼接key用
            int index = 0;
            foreach (object obj in list)
            {
                //遍历List递归存储具体的值
                //这里面存的已经是泛型的变量类型了 比如List<int> 这个obj就是int类型的了
                //假如泛型的变量类型不是常规变量 会继续往下递归
                SaveValue(obj, keyName + index);
                ++index;
            }

        }
        //判断是不是Dictionary类型 通过Dictionary的父类IDictionary来判断 和List的判断类似
        else if ( typeof(IDictionary).IsAssignableFrom(fieldType) )
        {
            //父类装子类 把要存储的值对象的类型转成IDictionary在遍历
            IDictionary dic = value as IDictionary;

            //先存储Dictionary中元素的数量 
            PlayerPrefs.SetInt(keyName, dic.Count);
            Log("存储DictionaryCount" + keyName + "值 = " + (dic.Count));

            //遍历存储Dictionary里面的具体值
            //声明一个index用做遍历Dictionary拼接key用
            int index = 0;
            foreach (object key in dic.Keys)
            {
                //遍历Dictionary递归存储具体的值
                //这里面存的已经是泛型的变量类型了 比如Dictionary<int,string> 就分别存int类型和string类型的变量
                //假如泛型的变量类型不是常规变量 会继续往下递归
                SaveValue(key, keyName + "_key_" + index);
                SaveValue(dic[key], keyName + "_value_" + index);
                ++index;
            }
        }
        //基础数据类型都不是 那么可能就是自定义类型
        else
        {
            //在走一遍存储自定义内的流程
            SaveData(value, keyName);
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="type">想要读取数据的 数据类型Type</param>
    /// <param name="keyName">数据对象的唯一key 自己控制</param>
    /// <returns></returns>
    public object LoadData( Type type, string keyName )
    {
        Log("读取自定义类型" + keyName);
        //不用object对象传入 而使用 Type传入
        //主要目的是节约一行代码（在外部）
        //假设现在你要 读取一个Player类型的数据 如果是传入object 你就必须在外部new一个对象传入
        //现在有Type的 你只用传入 一个Type typeof(Player) 然后我在内部动态创建一个对象给你返回出来
        //达到了 让你在外部 少写一行代码的作用

        //根据你传入的类型 和 keyName
        //依据你存储数据时  key的拼接规则 来进行数据的获取赋值 返回出去

        //根据传入的Type 创建一个对象 用于存储数据 要确保传进来的类型又无参构造
        object data = Activator.CreateInstance(type);

        //接下来的目的就是要往 这个new出来的对象中存储数据 填充数据

        //得到这个类所有字段的字段数组
        FieldInfo[] infos = type.GetFields();

        //用于拼接key的字符串
        string loadKeyName = "";

        //用于存储 单个字段信息的 对象 方便等一下遍历使用
        FieldInfo info;

        //遍历这个类的字段数组
        for (int i = 0; i < infos.Length; i++)
        {
            //获得单条字段
            info = infos[i];

            //key的拼接规则 一定是和存储时一模一样 这样才能找到对应数据
            loadKeyName = keyName + "_" + type.Name +
                "_" + info.FieldType.Name + "_" + info.Name;

            //有key 就可以结合 PlayerPrefs来读取数据
            //SetValue方法 赋值数据给对象 填充数据到data对象中 
            info.SetValue(data, LoadValue(info.FieldType, loadKeyName));
        }

        //返回填充完的data对象
        return data;
    }

    /// <summary>
    /// 读取单个数据的方法
    /// </summary>
    /// <param name="fieldType">字段类型 用于判断 用哪个api来读取</param>
    /// <param name="keyName">用于获取具体数据</param>
    /// <returns></returns>
    private object LoadValue(Type fieldType, string keyName)
    {
        //根据 字段类型 来判断 用哪个API来读取
        if( fieldType == typeof(int) )
        {
            Log("读取int:" + keyName + "值 = " + (PlayerPrefs.GetInt(keyName, 0) - 10));
            //解密 减10
            return PlayerPrefs.GetInt(keyName, 0) - 10;
        }
        else if (fieldType == typeof(float))
        {
            Log("读取float:" + keyName + "值 = " + (PlayerPrefs.GetFloat(keyName, 0)));
            return PlayerPrefs.GetFloat(keyName, 0);
        }
        else if (fieldType == typeof(string))
        {
            Log("读取string:" + keyName + "值 = " + (PlayerPrefs.GetString(keyName, "")));
            return PlayerPrefs.GetString(keyName, "");
        }
        else if (fieldType == typeof(bool))
        {
            Log("读取bool:" + keyName + "值 = " + (PlayerPrefs.GetInt(keyName, 0) == 1));
            //根据自定义存储bool的规则 来进行值的获取
            return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;
        }
        //通过反射 IsAssignableFrom方法判断 父子关系
        //这相当于是判断 这个字段是不是IList的子类 是的话基本上就是List类了
        else if ( typeof(IList).IsAssignableFrom(fieldType) )
        {
            //得到List长度
            int listCount = PlayerPrefs.GetInt(keyName, 0);
            Log("读取ListCount" + keyName + "值 = " + (listCount));

            //实例化一个List对象 来进行赋值
            //用了反射中双A中 Activator进行快速实例化List对象
            IList list = Activator.CreateInstance(fieldType) as IList;

            //根据List长度 根据长度逐个添加元素
            for (int i = 0; i < listCount; i++)
            {
                //GetGenericArguments得到List中泛型的类型
                //比如List<int> 这个fieldType.GetGenericArguments()[0]就是int类型了
                //得到了这个类型后 在递归调用读取List里面具体的值
                list.Add(LoadValue(fieldType.GetGenericArguments()[0], keyName + i));
            }
            return list;
        }
        //通过反射 IsAssignableFrom方法判断 父子关系
        //这相当于是判断 这个字段是不是IDictionary的子类 是的话基本上就是Dictionary类了
        else if ( typeof(IDictionary).IsAssignableFrom(fieldType) )
        {
            //得到字典的长度
            int dictionaryCount = PlayerPrefs.GetInt(keyName, 0);
            Log("读取DictionaryCount" + keyName + "值 = " + (dictionaryCount));

            //实例化一个字典对象 用父类装子类
            //用了反射中双A中 Activator进行快速实例化Dictionary对象
            IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;

            //GetGenericArguments得到Dictionary中泛型的类型数组 0是key 1是value
            Type[] kvType = fieldType.GetGenericArguments();

            //根据Dictionary长度 根据长度逐个添加键值对
            for (int i = 0; i < dictionaryCount; i++)
            {
                dic.Add(LoadValue(kvType[0], keyName + "_key_" + i),
                         LoadValue(kvType[1], keyName + "_value_" + i));
            }
            return dic;
        }
        //基础数据类型都不是 那么可能就是自定义类型
        else
        {
            //在走一遍读取自定义内的流程
            return LoadData(fieldType, keyName);
        }

    }

    /// <summary>
    /// PlayerPrefsDataMgr管理类的Log
    /// </summary>
    /// <param name="message"></param>
    private void Log(object message)
    {
        Debug.Log("PlayerPrefsDataMgr:"+ message);
    }
}
