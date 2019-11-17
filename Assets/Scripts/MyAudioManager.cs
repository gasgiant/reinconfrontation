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
            int index = Index(level);
            if (index > music_list.Length - 1)
                index = music_list.Length - 1;
            AudioManager.Instance.PlayMusic(music_list[index]);
        }
        /*
        if (level < music_list.Length * 2 && counter < 2)
        {
            counter++;
        }
        if (level < music_list.Length * 2 && counter >= 2)
        {
            counter = 0;
        }
        */
    }

    private int Index(int level)
    {
        if (level < 4)
            return level / 2;
        else
            return level - 2;
    }
}
