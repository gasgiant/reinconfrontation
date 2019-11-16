using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    public AudioClip[] music_list;

    public void playMusicOnLevel(int level)
    {
        if (level < music_list.Length)
            AudioManager.Instance.PlayMusic(music_list[level]);
            
    }
}
