using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ��Ƶ�������Ļ��࣬������������ֺ���Ч
public class BaseAudioManager : BaseSingletonInCSharp<BaseAudioManager>
{
    // ���ڲ��ű������ֵ���Ϸ�������ƵԴ
    private GameObject backgroundMusicNode; // �������ֵ���Ϸ����
    private AudioSource backgroundMusicAudioSource = null; // �������ֵ���ƵԴ
    private float backgroundMusicVolume = 1; // �������ֵ�����

    // ���ڲ�����Ч����Ϸ�������ƵԴ�б�
    private GameObject soundEffectNode = null; // ��Ч����Ϸ����
    private List<AudioSource> soundEffectAudioSourceList = new List<AudioSource>(); // ��Ч����ƵԴ�б�
    private float soundEffectVolume = 1; // ��Ч������

    // ���캯������ʼ���������������ڸ����¼�
    public BaseAudioManager()
    {
        // ����������ڸ����¼�����
        BaseMonoBehaviourManager.Instance.AddBaseLifeCycleManagerListener<BaseLifeCycleUpdateManager>(OnUpdate);
    }

    // ÿ֡���·���
    private void OnUpdate()
    {
        // ������Ч��ƵԴ�б��Ƴ��Ѿ�ֹͣ���ŵ���ƵԴ
        for (int i = soundEffectAudioSourceList.Count - 1; i >= 0; --i)
        {
            if (!soundEffectAudioSourceList[i].isPlaying)
            {
                // ֹͣ�����ٲ���ʹ�õ���ƵԴ
                GameObject.Destroy(soundEffectAudioSourceList[i]);
                soundEffectAudioSourceList.RemoveAt(i);
            }
        }
    }

    // ���ű�������
    public void PlayBackgroundMusic(string BackgroundMusicName)
    {
        if (backgroundMusicAudioSource == null)
        {
            // �����������ֵ���Ϸ�������ƵԴ
            backgroundMusicNode = new GameObject();
            backgroundMusicNode.name = "BackgroundMusicNode";
            backgroundMusicAudioSource = backgroundMusicNode.AddComponent<AudioSource>();
        }

        // �첽���ر���������Դ����������ƵԴ���ԣ�Ȼ�󲥷�
        BaseResourceManager.Instance.LoadAsync<AudioClip>("Music/BackgroundMusic/" + BackgroundMusicName, (audioClip) =>
        {
            backgroundMusicAudioSource.clip = audioClip;
            backgroundMusicAudioSource.loop = true; // ��������ѭ������
            backgroundMusicAudioSource.volume = backgroundMusicVolume; // ���ñ�����������
            backgroundMusicAudioSource.Play(); // ���ű�������
        });
    }

    // ��ͣ��������
    public void PauseBackgroundMusic()
    {
        if (backgroundMusicAudioSource == null) return;

        // ��ͣ�������ֵĲ���
        backgroundMusicAudioSource.Pause();
    }

    // ֹͣ��������
    public void StopBackgroundMusic()
    {
        if (backgroundMusicAudioSource == null) return;

        // ֹͣ�������ֵĲ���
        backgroundMusicAudioSource.Stop();
    }

    // �ı䱳�����ֵ�����
    public void ChangeBackgroundMusicVolume(float backgroundMusicVolume)
    {
        this.backgroundMusicVolume = backgroundMusicVolume;
        if (backgroundMusicAudioSource == null)
            return;

        // ���±������ֵ�����
        backgroundMusicAudioSource.volume = this.backgroundMusicVolume;
    }

    // ������Ч
    public void PlaySoundEffect(string name, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if (soundEffectNode == null)
        {
            // ������Ч����Ϸ����
            soundEffectNode = new GameObject();
            soundEffectNode.name = "SoundEffectNode";
        }

        // �첽������Ч��Դ����������ƵԴ��Ȼ�󲥷���Ч
        BaseResourceManager.Instance.LoadAsync<AudioClip>("Music/SoundEffect/" + name, (audioClip) =>
        {
            AudioSource audioSource = soundEffectNode.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = isLoop; // ������Ч�Ƿ�ѭ������
            audioSource.volume = soundEffectVolume; // ������Ч����
            audioSource.Play(); // ������Ч
            soundEffectAudioSourceList.Add(audioSource); // ����ƵԴ��ӵ��б���

            // ִ�лص�����������У�
            if (callBack != null)
                callBack(audioSource);
        });
    }

    // �ı���Ч������
    public void ChangeSoundEffectVolume(float soundEffectVolume)
    {
        this.soundEffectVolume = soundEffectVolume;
        for (int i = 0; i < soundEffectAudioSourceList.Count; ++i)
            soundEffectAudioSourceList[i].volume = soundEffectVolume;
    }

    // ָֹͣ������Ч
    public void StopSoundEffect(AudioSource audioSource)
    {
        if (soundEffectAudioSourceList.Contains(audioSource))
        {
            // ����Ч�б����Ƴ���ƵԴ��ֹͣ���ţ�������
            soundEffectAudioSourceList.Remove(audioSource);
            audioSource.Stop();
            GameObject.Destroy(audioSource);
        }
    }
}
