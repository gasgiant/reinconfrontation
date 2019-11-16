using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI shotsCountText;
    [SerializeField] 
    private LineRenderer lineRenderer;
    private Camera cam;
    private PlayerController playerController;
    private int shotsCount;
    private bool loseInvoked;

    private void Start()
    {
        lineRenderer.positionCount = 2;
        cam = FindObjectOfType<Camera>();
        playerController = GetComponent<PlayerController>();
        shotsCountText.text = "" + (GameManager.Instance.RoundNumber + 1 - shotsCount);
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
        shotsCount = 0;
        loseInvoked = false;
        shotsCountText.text = "" + (GameManager.Instance.RoundNumber + 1 - shotsCount);
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
            shotsCount++;
            if (shotsCount >= GameManager.Instance.RoundNumber + 1)
            {
                if (!loseInvoked)
                    Invoke("Lose", 2);
                loseInvoked = true;
                shotsCountText.text = "0";
            }
            else
            {
                shotsCountText.text = "" + (GameManager.Instance.RoundNumber + 1 - shotsCount);
                Vector3 impulse = -dir * 40;
                playerController.ApplyImpulse(impulse);
                CommandManager.Instance.RememberCommand(impulse, transform.position, transform.rotation);
                BulletSpawner.Instance.SpawnBullet(transform.position, dir, 0);
            }
        }
    }

    private void Lose()
    {
        GameManager.Instance.FinishRound(false);

    }
}
