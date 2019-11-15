using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] 
    private LineRenderer lineRenderer;
    private Camera cam;
    private PlayerController playerController;

    private void Start()
    {
        lineRenderer.positionCount = 2;
        cam = FindObjectOfType<Camera>();
        playerController = GetComponent<PlayerController>();
    }

    private void LateUpdate()
    {
        Vector3 targetPos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPos);
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 impulse = -dir * 40;
            playerController.ApplyImpulse(impulse);
            CommandManager.Instance.RememberCommand(impulse, transform.position, transform.rotation);
            BulletSpawner.Instance.SpawnBullet(transform.position, dir, 0);
        }
    }
}
