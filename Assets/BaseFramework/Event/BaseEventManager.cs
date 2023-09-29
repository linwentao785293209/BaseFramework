using System.Collections.Generic;
using UnityEngine.Events;

// �¼�������
public class BaseEventManager : BaseSingletonInCSharp<BaseEventManager>
{
    // key ���� �¼������֣����磺�������������������ͨ�صȵȣ�
    // value ���� ��Ӧ���Ǽ�������¼���Ӧ��ί�к�����
    private Dictionary<string, IBaseEventListener> baseEventListenerDictionary = new Dictionary<string, IBaseEventListener>();

    /// <summary>
    /// ����¼�����
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action">׼�����������¼���ί�к���</param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        // ��û�ж�Ӧ���¼�����
        // �е����
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            (baseEventListenerDictionary[name] as BaseEventListener<T>).actions += action;
        }
        // û�е����
        else
        {
            baseEventListenerDictionary.Add(name, new BaseEventListener<T>(action));
        }
    }

    /// <summary>
    /// ��������Ҫ�������ݵ��¼�
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action">���������¼���ί�к���</param>
    public void AddEventListener(string name, UnityAction action)
    {
        // ��û�ж�Ӧ���¼�����
        // �е����
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            (baseEventListenerDictionary[name] as BaseEventListener).actions += action;
        }
        // û�е����
        else
        {
            baseEventListenerDictionary.Add(name, new BaseEventListener(action));
        }
    }

    /// <summary>
    /// �Ƴ���Ӧ���¼�����
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action">��Ӧ֮ǰ��ӵ�ί�к���</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (baseEventListenerDictionary.ContainsKey(name))
            (baseEventListenerDictionary[name] as BaseEventListener<T>).actions -= action;
    }

    /// <summary>
    /// �Ƴ�����Ҫ�������¼�
    /// </summary>
    /// <param name="name">�¼�������</param>
    /// <param name="action">��Ӧ֮ǰ��ӵ�ί�к���</param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (baseEventListenerDictionary.ContainsKey(name))
            (baseEventListenerDictionary[name] as BaseEventListener).actions -= action;
    }

    /// <summary>
    /// �¼�����
    /// </summary>
    /// <param name="name">��һ�����ֵ��¼�������</param>
    public void EventTrigger<T>(string name, T info)
    {
        // ��û�ж�Ӧ���¼�����
        // �е����
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            // ʹ�� null �����������ȫ�ش����¼�
            (baseEventListenerDictionary[name] as BaseEventListener<T>)?.actions?.Invoke(info);
        }
    }

    /// <summary>
    /// �¼�����������Ҫ�����ģ�
    /// </summary>
    /// <param name="name">��һ�����ֵ��¼�������</param>
    public void EventTrigger(string name)
    {
        // ��û�ж�Ӧ���¼�����
        // �е����
        if (baseEventListenerDictionary.ContainsKey(name))
        {
            // ʹ�� null �����������ȫ�ش����¼�
            (baseEventListenerDictionary[name] as BaseEventListener)?.actions?.Invoke();
        }
    }

    /// <summary>
    /// ����¼�����
    /// ��Ҫ���ڳ����л�ʱ
    /// </summary>
    public void Clear()
    {
        baseEventListenerDictionary.Clear();
    }
}