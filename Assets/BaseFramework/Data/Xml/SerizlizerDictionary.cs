using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

// 这是一个继承自Dictionary，实现了IXmlSerializable接口的自定义字典类型，用于XML序列化和反序列化键值对数据。
public class SerizlizerDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    // 返回空值。GetSchema方法是必须由接口 IXMlSerializable实现的方法，但在此类中不需要使用它。
    public XmlSchema GetSchema()
    {
        return null;
    }

    // 自定义字典类型（继承自Dictionary）的反序列化规则。接受一个XmlReader阅读器作为反序列化数据源。
    public void ReadXml(XmlReader xmlReader)
    {
        // 创建两个XmlSerializer实例，一个key的类型、一个是value的类型，用于反序列化XML流中的键值对数据。
        XmlSerializer keyXmlSerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueXmlSerializer = new XmlSerializer(typeof(TValue));

        // 要跳过根节点
        xmlReader.Read();
        // 只要当前节点不为EndElement，则意味着还可以读取到新的节点，继续反序列化操作。
        while (xmlReader.NodeType != XmlNodeType.EndElement)
        {
            // 反序列化从XML阅读器读取的键对象，并进行强制类型转换到泛型TKey类别 
            TKey key = (TKey)keyXmlSerializer.Deserialize(xmlReader);
            // 反序列化从XML阅读器读取的值对象，并进行强制类型转换到泛型TValue类别 
            TValue value = (TValue)valueXmlSerializer.Deserialize(xmlReader);
            // 将键和值添加到字典中
            this.Add(key, value);
        }

        // 要跳过尾节点 避免影响之后的数据读取
        xmlReader.Read();
    }

    // 自定义字典类型的序列化规则。使用提供的XmlWriter实例将该字典序列化为XML。
    public void WriteXml(XmlWriter xmlWriter)
    {
        // 创建两个XmlSerializer实例，一个key的类型、一个是value的类型，用于序列化键值对数据。
        XmlSerializer keyXmlSerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueXmlSerializer = new XmlSerializer(typeof(TValue));

        // 使用foreach循环安全遍历字典中所有的键值对象，并依次将其序列化到由writer引用的输出流中。
        foreach (KeyValuePair<TKey, TValue> kv in this)
        {
            // 序列化键和值到writer流。
            keyXmlSerializer.Serialize(xmlWriter, kv.Key);
            valueXmlSerializer.Serialize(xmlWriter, kv.Value);
        }
    }
}