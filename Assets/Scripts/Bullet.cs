using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private GameObject triggerObject;
    [SerializeField]
    private Color playerColor;
    [SerializeField]
    private Gradient playerTailColor;
    [SerializeField]
    private Color enemyColor;
    [SerializeField]
    private Gradient enemyTailColor;
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

    private void Update()
    {
        Vector3 dir = rb.velocity.normalized;
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    public void Initialize(Vector2 direction, bool isEnemy)
    {
        rb.velocity = direction * Speed;
        Invoke("EnableDamageTrigger", 0.1f);
        if (isEnemy)
        {
            SetColor(enemyColor, enemyTailColor);
            triggerObject.tag = "EnemyBullet";
        }
        else
        {
            SetColor(playerColor, playerTailColor);
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
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void SetColor(Color color, Gradient tailColor)
    {
        sprite.color = color;
        trail.colorGradient = tailColor;
    }
}
