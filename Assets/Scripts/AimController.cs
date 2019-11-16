using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimController : MonoBehaviour
{
    //[SerializeField]
    //private TextMeshProUGUI shotsCountText;
    [SerializeField] 
    private LineRenderer lineRenderer;
    private Camera cam;
    private PlayerController playerController;
    //private int shotsCount;
    //private bool loseInvoked;

    private void Start()
    {
        lineRenderer.positionCount = 2;
        cam = FindObjectOfType<Camera>();
        playerController = GetComponent<PlayerController>();
        //shotsCountText.text = "" + (GameManager.Instance.RoundNumber - shotsCount + 1);
    }

    private void OnEnable()
    {
        GameManager.ResetEvent.AddListener(Reset);
    }

    private void OnDisable()
    {
        GameManager.ResetEvent.RemoveListener(Reset);
    }

    private void Reset()
    {
        //shotsCount = 0;
        //shotsCountText.text = "" + (GameManager.Instance.RoundNumber - shotsCount + 1);
        //loseInvoked = false;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPos);
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        if (GameManager.Instance.EnableControl && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot(dir);
        }
        /*
        if (GameManager.Instance.EnableControl && Input.GetKeyDown(KeyCode.Mouse0))
        {
            shotsCount++;
            if (shotsCount >= GameManager.Instance.RoundNumber + 1)
            {
                if (!loseInvoked)
                {
                    Invoke("Lose", 2);
                    Shoot(dir);
                }
                loseInvoked = true;
                shotsCountText.text = "0";
            }
            else
            {
                Shoot(dir);
            }
        }
        */
    }

    private void Shoot(Vector3 dir)
    {
        //shotsCountText.text = "" + (GameManager.Instance.RoundNumber - shotsCount + 1);
        Vector3 impulse = -dir * 40;
        playerController.ApplyImpulse(impulse);
        CommandManager.Instance.RememberCommand(impulse, transform.position, transform.rotation);
        BulletSpawner.Instance.SpawnBullet(transform.position, dir, 0, false);
    }

    private void Lose()
    {
        //if (loseInvoked)
        //    GameManager.Instance.FinishRound(false);

    }
}
