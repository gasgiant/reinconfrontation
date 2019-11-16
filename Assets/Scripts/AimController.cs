using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimController : MonoBehaviour
{
    [SerializeField] 
    private LineRenderer lineRenderer;
    [SerializeField]
    private LayerMask wallLayer;
    private Camera cam;
    private PlayerController playerController;
    

    private void Start()
    {
        lineRenderer.positionCount = 2;
        cam = FindObjectOfType<Camera>();
        playerController = GetComponent<PlayerController>();
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

    }

    private void LateUpdate()
    {
        Vector3 targetPos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);

        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPos);
        Vector3 dir = (targetPos - transform.position).normalized;

        /*
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 100, wallLayer);
        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);

            lineRenderer.SetPosition(2, hit.point +
                (-((Vector2)dir - Vector3.Dot(hit.normal, dir) * hit.normal) 
                + Vector3.Dot(hit.normal, dir) * hit.normal).normalized * 1);
        }
        */
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
        if (GameManager.Instance.EnableControl && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot(dir);
        }
    }

    private void Shoot(Vector3 dir)
    {
        AudioManager.Instance.PlaySound("Shoot");
        Vector3 impulse = -dir * 40;
        playerController.ApplyImpulse(impulse);
        CommandManager.Instance.RememberCommand(impulse, transform.position, transform.rotation);
        BulletSpawner.Instance.SpawnBullet(transform.position, dir, 0, false);
        CameraController.RandomShake(0.5f, 0.06f, 2, 0.2f, 1);
    }
}
