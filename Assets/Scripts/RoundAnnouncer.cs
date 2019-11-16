using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RoundAnnouncer : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public TextMeshProUGUI big_text;
    public GameObject level_obj;
    public float timeToAppear = 2f;
    private Animator big_anim;
    void Start()
    {
        big_anim = level_obj.GetComponent<Animator>();
        newLevel(1, true);
    }
    public void newLevel(int level, bool toAnnounce)
    {
        if (toAnnounce)
        {
            big_text.text = "generation_" + String.Format("{0:00}", level);
            big_anim.SetTrigger("levelNameAppear");
            StartCoroutine(AnnounceLevel());
        }
        player.GetComponent<ModelName>().SetName("model_" + String.Format("{0:00}", level));
        enemy.GetComponent<ModelName>().SetName("model_" + String.Format("{0:00}", level - 1));
    }

    private IEnumerator AnnounceLevel()
    {
        yield return new WaitForSeconds(timeToAppear);
        big_anim.SetTrigger("levelNameDisappear");
    }
}
