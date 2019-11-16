using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject damageTrigger;

    public static float Speed = 20;
    private Rigidbody2D rb;
    private int hits = 0;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction)
    {
        rb.velocity = direction * Speed;
        Invoke("EnableDamageTrigger", 0.1f);
    }

    private void EnableDamageTrigger()
    {
        damageTrigger.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            /*
            hits++;
            if (hits > 1)
            {
                BulletSpawner.Instance.RemoveBullet(gameObject);
                Destroy(gameObject);
            }
            */
        }
    }
}
