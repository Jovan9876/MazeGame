using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager {

    public enum Sound {
        PlayerMove,
        PlayerWallCollide,
        EnemyDie,
        EnemyRespawn,
        BallHit,
        DayMusic,
        NightMusic,
    }

    private static Dictionary<Sound, float> soundTimerDict;


    public static void Init() {
        soundTimerDict = new Dictionary<Sound, float>();
        soundTimerDict[Sound.PlayerMove] = 0f;
    }

    public static void PlaySound(Sound sound) {
        if (CanPlaySound(sound)) { 
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound) { 
    
        switch(sound) {
            default: 
                return true;
            case Sound.PlayerMove:
                if (soundTimerDict.ContainsKey(sound)) { 
                    float lastTimePlayed = soundTimerDict[sound];
                    float playerMoveTimer = .5f;
                    if (lastTimePlayed + playerMoveTimer < Time.time) {
                        soundTimerDict[sound] = Time.time;
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return true;
                }
        }

    }

    private static AudioClip GetAudioClip(Sound sound) {

        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.Instance.soundAudioClipArray) { 
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError($"Sound: {sound} not found!");
        return null;
    }

}
