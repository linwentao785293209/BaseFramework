using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace BaseFramework
{
    //XML数据管理类
    public class BaseXmlDataManager : BaseSingletonInCSharp<BaseXmlDataManager>
    {
        /// <summary>
        /// 数据存储路径
        /// </summary>
        private static string DATA_SAVE_PATH = Application.persistentDataPath + "/Data/Xml/";

        /// <summary>
        /// 配置存储路径
        /// </summary>
        public static string CONFIG_SAVE_PATH = Application.streamingAssetsPath + "/Data/Xml/";


        /// <summary>
        /// 将数据对象序列化之后保存到xml格式文件中。
        /// </summary>
        /// <param name="data">要存储的数据对象</param>
        /// <param name="fileName">指定存储的文件名。注意，不需要包括后缀".xml"。</param>
        public void SaveData(object data, string fileName)
        {
            // 获取该数据类型存储XML文件的路径
            string path = DATA_SAVE_PATH + fileName + ".xml";
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                // 对数据进行序列化处理（使用 XML 序列化方式）
                XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
                xmlSerializer.Serialize(streamWriter, data);
            }
        }

        /// <summary>
        /// 从指定的xml文件反序列化出对象实例并返回。
        /// </summary>
        /// <param name="type">指定反序列化得到的对象类型</param>
        /// <param name="fileName">指定读取的文件名。注意，请不要包含后缀".xml"。</param>
        /// <returns>返回反序列化得到的对象实例。</returns>
        public object LoadData(Type type, string fileName)
        {
            // 获取该数据类型存储XML文件的路径
            string path = DATA_SAVE_PATH + fileName + ".xml";
            // 如果数据文件不存在，则不做任何处理，直接返回一个实例对象
            if (!File.Exists(path))
            {
                // 尝试从默认读取路径中尝试读取文件。如果包含该文件，则尝试获取其信息并进行反序列化。
                path = CONFIG_SAVE_PATH + fileName + ".xml";
                if (!File.Exists(path))
                {
                    // 不幸的是，即使在默认路径中也找不到该文件，因此我们将无法从XML文件中反序列化任何内容，所以要新建一个对象来返回。
                    return Activator.CreateInstance(type);
                }
            }

            // 存在XML文件，于是我们创建文本读写器，将XML文件转换为对象并返回之。
            using (StreamReader streamReader = new StreamReader(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                return xmlSerializer.Deserialize(streamReader);
            }
        }
    }
}