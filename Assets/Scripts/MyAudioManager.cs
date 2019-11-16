using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    public AudioClip[] music_list;

    public void PlayMusicOnLevel(int level)
    {
        if (level < music_list.Length*2)
             AudioManager.Instance.PlayMusic(music_list[level/2]);
            
    }
}
