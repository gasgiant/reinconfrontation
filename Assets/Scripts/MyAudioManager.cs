using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    public AudioClip[] music_list;
    private int counter = 0;

    public void PlayMusicOnLevel(int level)
    {
        if (level < music_list.Length*2 && counter == 0)
        {
            AudioManager.Instance.PlayMusic(music_list[level/2]);
        }
        if (level < music_list.Length * 2 && counter < 2)
        {
            counter++;
        }
        if (level < music_list.Length * 2 && counter >= 2)
        {
            counter = 0;
        }




    }
}
