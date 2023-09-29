using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 音频管理器的基类，负责管理背景音乐和音效
public class BaseAudioManager : BaseSingletonInCSharp<BaseAudioManager>
{
    // 用于播放背景音乐的游戏对象和音频源
    private GameObject backgroundMusicNode; // 背景音乐的游戏对象
    private AudioSource backgroundMusicAudioSource = null; // 背景音乐的音频源
    private float backgroundMusicVolume = 1; // 背景音乐的音量

    // 用于播放音效的游戏对象和音频源列表
    private GameObject soundEffectNode = null; // 音效的游戏对象
    private List<AudioSource> soundEffectAudioSourceList = new List<AudioSource>(); // 音效的音频源列表
    private float soundEffectVolume = 1; // 音效的音量

    // 构造函数，初始化并监听生命周期更新事件
    public BaseAudioManager()
    {
        // 添加生命周期更新事件监听
        BaseMonoBehaviourManager.Instance.AddBaseLifeCycleManagerListener<BaseLifeCycleUpdateManager>(OnUpdate);
    }

    // 每帧更新方法
    private void OnUpdate()
    {
        // 遍历音效音频源列表，移除已经停止播放的音频源
        for (int i = soundEffectAudioSourceList.Count - 1; i >= 0; --i)
        {
            if (!soundEffectAudioSourceList[i].isPlaying)
            {
                // 停止并销毁不再使用的音频源
                GameObject.Destroy(soundEffectAudioSourceList[i]);
                soundEffectAudioSourceList.RemoveAt(i);
            }
        }
    }

    // 播放背景音乐
    public void PlayBackgroundMusic(string BackgroundMusicName)
    {
        if (backgroundMusicAudioSource == null)
        {
            // 创建背景音乐的游戏对象和音频源
            backgroundMusicNode = new GameObject();
            backgroundMusicNode.name = "BackgroundMusicNode";
            backgroundMusicAudioSource = backgroundMusicNode.AddComponent<AudioSource>();
        }

        // 异步加载背景音乐资源，并设置音频源属性，然后播放
        BaseResourceManager.Instance.LoadAsync<AudioClip>("Music/BackgroundMusic/" + BackgroundMusicName, (audioClip) =>
        {
            backgroundMusicAudioSource.clip = audioClip;
            backgroundMusicAudioSource.loop = true; // 背景音乐循环播放
            backgroundMusicAudioSource.volume = backgroundMusicVolume; // 设置背景音乐音量
            backgroundMusicAudioSource.Play(); // 播放背景音乐
        });
    }

    // 暂停背景音乐
    public void PauseBackgroundMusic()
    {
        if (backgroundMusicAudioSource == null) return;

        // 暂停背景音乐的播放
        backgroundMusicAudioSource.Pause();
    }

    // 停止背景音乐
    public void StopBackgroundMusic()
    {
        if (backgroundMusicAudioSource == null) return;

        // 停止背景音乐的播放
        backgroundMusicAudioSource.Stop();
    }

    // 改变背景音乐的音量
    public void ChangeBackgroundMusicVolume(float backgroundMusicVolume)
    {
        this.backgroundMusicVolume = backgroundMusicVolume;
        if (backgroundMusicAudioSource == null)
            return;

        // 更新背景音乐的音量
        backgroundMusicAudioSource.volume = this.backgroundMusicVolume;
    }

    // 播放音效
    public void PlaySoundEffect(string name, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if (soundEffectNode == null)
        {
            // 创建音效的游戏对象
            soundEffectNode = new GameObject();
            soundEffectNode.name = "SoundEffectNode";
        }

        // 异步加载音效资源，并创建音频源，然后播放音效
        BaseResourceManager.Instance.LoadAsync<AudioClip>("Music/SoundEffect/" + name, (audioClip) =>
        {
            AudioSource audioSource = soundEffectNode.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.loop = isLoop; // 设置音效是否循环播放
            audioSource.volume = soundEffectVolume; // 设置音效音量
            audioSource.Play(); // 播放音效
            soundEffectAudioSourceList.Add(audioSource); // 将音频源添加到列表中

            // 执行回调函数（如果有）
            if (callBack != null)
                callBack(audioSource);
        });
    }

    // 改变音效的音量
    public void ChangeSoundEffectVolume(float soundEffectVolume)
    {
        this.soundEffectVolume = soundEffectVolume;
        for (int i = 0; i < soundEffectAudioSourceList.Count; ++i)
            soundEffectAudioSourceList[i].volume = soundEffectVolume;
    }

    // 停止指定的音效
    public void StopSoundEffect(AudioSource audioSource)
    {
        if (soundEffectAudioSourceList.Contains(audioSource))
        {
            // 从音效列表中移除音频源，停止播放，并销毁
            soundEffectAudioSourceList.Remove(audioSource);
            audioSource.Stop();
            GameObject.Destroy(audioSource);
        }
    }
}
