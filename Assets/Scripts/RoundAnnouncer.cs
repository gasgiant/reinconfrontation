using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RoundAnnouncer : MonoBehaviour
{
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
        big_text.text = "generation_" + String.Format("{0:00}", level);
        StartCoroutine(AnnounceLevel());
    }

    private IEnumerator AnnounceLevel()
    {
        big_anim.SetTrigger("levelNameAppear");
        yield return new WaitForSeconds(timeToAppear);
        big_anim.SetTrigger("levelNameDisappear");
    }
}
