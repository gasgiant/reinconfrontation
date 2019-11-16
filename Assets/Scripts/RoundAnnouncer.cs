using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RoundAnnouncer : MonoBehaviour
{
    public TextMeshProUGUI bigText;
    private string myText;
    public GameObject level_obj;
    public float timeToAppear = 2f;
    private Animator big_anim;
    public float typeSpeed = 0.01f;
    private string currentText = "";
    void Start()
    {
        big_anim = level_obj.GetComponent<Animator>();
    }
    public void NewLevel(int level)
    {
        myText = "generation_" + String.Format("{0:00}", level);
        bigText.text = "";
        StartCoroutine(AnnounceLevel());
    }

    private IEnumerator AnnounceLevel()
    {
        big_anim.SetTrigger("levelNameAppear");
        for (int i = 0; i <= myText.Length; i++)
        {
            bigText.text = myText.Substring(0, i);
            yield return new WaitForSeconds(typeSpeed);
        }
        yield return new WaitForSeconds(timeToAppear);
        big_anim.SetTrigger("levelNameDisappear");
    }
}
