using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ModelName : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject player;
    public float time = 1.0f;
    private Camera cam;
    private Animator anim;
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(Disappear());
    }

    public void ChangeName(string name)
    {
        text.text = name;
    }
    void Update()
    {
        transform.position = cam.WorldToScreenPoint(player.GetComponent<Transform>().position + new Vector3(0, 1f, 0));
    }
    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("disappearTrigger");
        Destroy(gameObject, 2); // this is for 2 second delay
    }

}
