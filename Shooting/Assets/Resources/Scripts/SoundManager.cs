using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum SoundType
    {
        None = 0,
        Jump,
        Land,
        Step,
        BounceBasket,
        BounceFloor,
        BounceWire,
        BounceBackboard,

        //BGM
        BGM,
    }

    private static GameObject oneShotSoundGameObject;
    private static AudioSource oneShotSoundSource;
    private static Dictionary<SoundType, float> soundTimerDictionary;

    public static void Init()
    {
        soundTimerDictionary= new Dictionary<SoundType, float>();
        soundTimerDictionary[SoundType.Step] = 0f;
        soundTimerDictionary[SoundType.Jump] = 0f;
    }

    private static bool CanPlaySound(SoundType type, float playerMoveTimerMax)
    {
        if (soundTimerDictionary.ContainsKey(type))
        {
            float lastTimePlayed = soundTimerDictionary[type];
            if (lastTimePlayed + playerMoveTimerMax < Time.time)
            {
                soundTimerDictionary[type] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public static bool isNeedLoop(SoundType type)
    {
        switch (type)
        {
            case SoundType.BGM:
                return true;
        }
        return false;
    }

    //播放需要间隔播放的音频
    public static void PlaySoundWithTimer(SoundType type, float time)
    {
        if (CanPlaySound(type, time))
        {
            if (oneShotSoundGameObject == null)
            {
                oneShotSoundGameObject = new GameObject("Sound");
                oneShotSoundSource = oneShotSoundGameObject.AddComponent<AudioSource>();
            }
            if (isNeedLoop(type))
            {
                oneShotSoundSource.clip = GetAudioClip(type);
                oneShotSoundSource.loop = true;
                oneShotSoundSource.Play();
            }
            else
            {
                oneShotSoundSource.PlayOneShot(GetAudioClip(type));
            }
        }
    }

    //专门播放BGM,obj不销毁
    public static void PlayBGMSound(SoundType type, GameObject obj)
    {
        obj = new GameObject("BGMSound");
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        if (isNeedLoop(type))
        {
            audioSource.clip = GetAudioClip(type);
            audioSource.loop = true;
            audioSource.volume = 0.2f;
            audioSource.Play();
        }
    }

    public static void PlayBGMSound(SoundType type)
    {
        if (GameObject.Find("BGMSound") != null)
        {
            GameObject.Destroy(GameObject.Find("BGMSound").gameObject);
        }
        GameObject obj = new GameObject("BGMSound");
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        if (isNeedLoop(type))
        {
            audioSource.clip = GetAudioClip(type);
            audioSource.loop = true;
            audioSource.volume = 0.2f;
            audioSource.Play();
        }
    }

    //播放需要循环播放的音频，播放完成后跟随obj的生命周期
    public static void PlayLoopSound(SoundType type, GameObject obj)
    {
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        if (isNeedLoop(type))
        {
            audioSource.clip = GetAudioClip(type);
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = GetAudioClip(type);
            audioSource.loop = true;
            audioSource.volume = 0.4f;
            audioSource.Play();
        }
    }

    //播放完成后销毁
    public static void PlaySound(SoundType sound, Vector3 position)
    {

        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        if (isNeedLoop(sound))
        {
            audioSource.clip = GetAudioClip(sound);
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
        }
        Object.Destroy(soundGameObject, audioSource.clip.length);
    }

    public static void PlaySoundWithTimer(SoundType sound, Vector3 position, float time)
    {
        if (CanPlaySound(sound,time))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            if (isNeedLoop(sound))
            {
                audioSource.clip = GetAudioClip(sound);
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = GetAudioClip(sound);
                audioSource.Play();
            }
            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(SoundType sound)
    {
        if (oneShotSoundGameObject == null)
        {
            oneShotSoundGameObject = new GameObject("Sound");
            oneShotSoundSource = oneShotSoundGameObject.AddComponent<AudioSource>();
        }
        if (isNeedLoop(sound))
        {
            oneShotSoundSource.clip = GetAudioClip(sound);
            oneShotSoundSource.loop = true;
            oneShotSoundSource.Play();
        }
        else
        {
            oneShotSoundSource.PlayOneShot(GetAudioClip(sound));
        }      
    }

    //public static void PlaySound(SoundType sound, Vector3 position, bool isOnShot = true)
    //{

    //    GameObject soundGameObject = new GameObject("Sound");
    //    soundGameObject.transform.position = position;
    //    AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
    //    if (isNeedLoop(sound))
    //    {
    //        audioSource.clip = GetAudioClip(sound);
    //        audioSource.loop = true;
    //        audioSource.Play();
    //    }
    //    else
    //    {
    //        audioSource.PlayOneShot(GetAudioClip(sound));
    //    }
    //    Object.Destroy(soundGameObject, audioSource.clip.length);
    //}

    private static AudioClip GetAudioClip(SoundType soundType)
    {
        for (int i = 0; i < GameAsstes.Instance.soundAudioClips.Length; i++)
        {
            if (GameAsstes.Instance.soundAudioClips[i].type == soundType)
            {
                return GameAsstes.Instance.soundAudioClips[i].audioClip;
            }
        }
        Debug.LogError("Can't find audio clip: " + soundType);
        return null;
    }
}
