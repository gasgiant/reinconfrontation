using System.Collections;
using UnityEngine;
using TMPro;


public class ModelName : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject player;
    public float time = 1.0f;
    public float waitTime = 0.5f;
    private Camera cam;
    private Animator anim;
    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(Disappear());
    }

    public void SetName(string name)
    {
        text.text = name;
    }
    void Update()
    {
        transform.position = cam.WorldToScreenPoint(player.GetComponent<Transform>().position + new Vector3(0, 1f, 0));
    }
    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(waitTime);
        anim.SetTrigger("appearTrigger");
        yield return new WaitForSeconds(time);
        anim.SetTrigger("disappearTrigger");
    }

}
