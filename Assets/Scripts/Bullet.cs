using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject triggerObject;
    [SerializeField]
    private Color playerColor;
    [SerializeField]
    private Color enemyColor;
    [SerializeField]
    private TrailRenderer trail;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private GameObject damageTrigger;

    public static float Speed = 10;
    private Rigidbody2D rb;
    private int hits = 0;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, bool isEnemy)
    {
        rb.velocity = direction * Speed;
        Invoke("EnableDamageTrigger", 0.1f);
        if (isEnemy)
        {
            SetColor(enemyColor);
            triggerObject.tag = "EnemyBullet";
        }
        else
        {
            SetColor(playerColor);
            triggerObject.tag = "Bullet";
        }
    }

    private void EnableDamageTrigger()
    {
        damageTrigger.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            
            hits++;
            if (hits > 1)
            {
                BulletSpawner.Instance.RemoveBullet(gameObject);
                Destroy(gameObject);
            }
            
        }
    }

    private void SetColor(Color color)
    {
        trail.startColor = color;
        trail.endColor = new Color(color.r, color.g, color.b, 0.5f);
        sprite.color = color;
    }
}
