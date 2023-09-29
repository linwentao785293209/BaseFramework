// ����һ�����ͻ��� BaseSingletonInCSharp��Ҫ�������� T �������������ͣ�class�����Ҿ����޲������캯����new()��
public class BaseSingletonInCSharp<T> where T : class, new()
{
    // ���ڴ洢����ʵ����˽�о�̬�ֶ�
    private static T instance;

    // �����߳�ͬ����������
    private static readonly object LockObject = new object();

    // ������̬���ԣ����ڻ�ȡ����ʵ��
    public static T Instance
    {
        get
        {
            // ���ʵ����δ����
            if (instance == null)
            {
                // ʹ����ȷ���̰߳�ȫ
                lock (LockObject)
                {
                    // �ٴμ��ʵ���Ƿ�Ϊ�գ���Ϊ����߳̿���ͬʱͨ����һ�����
                    if (instance == null)
                    {
                        // ���Ϊ�գ�����һ���µ�ʵ������ֵ�� instance
                        instance = new T();
                    }
                }
            }
            // ���ص���ʵ��
            return instance;
        }
    }

    // �ܱ����Ĺ��캯����ֻ�������ڲ����������з���
    protected BaseSingletonInCSharp()
    {
        // ���������ӳ�ʼ�����룬��ͨ������Ҫ����Ϊʵ�������ӳٵ�
    }
}
