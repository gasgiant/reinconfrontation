using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainVisuals;
    [SerializeField]
    private GameObject deathParticlesPrefab;
    [SerializeField]
    private GameObject onHitParticlesPrefab;
    [SerializeField]
    private TextMeshPro livesText;
    [SerializeField]
    private bool isEnemy;

    private Rigidbody2D rb;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private int hits;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        GameManager.ResetEvent.AddListener(Reset);
        GameManager.DieEvent.AddListener(Die);
    }

    private void OnDisable()
    {
        GameManager.ResetEvent.RemoveListener(Reset);
        GameManager.DieEvent.RemoveListener(Die);
    }

    public void ApplyImpulse(Vector3 impulse)
    {
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    private void Reset()
    {
        transform.position = GameManager.Invert * initialPosition;
        transform.rotation = initialRotation;
        rb.velocity = Vector2.zero;
        mainVisuals.SetActive(true);
        hits = 0;
        if (isEnemy)
            livesText.text = (GameManager.Instance.RoundNumber + 1 - hits).ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemy && collision.CompareTag("Bullet"))
        {
            hits++;
            if (hits > GameManager.Instance.RoundNumber)
            {
                GameManager.Instance.FinishRound(isEnemy);
                livesText.text = "0";
            }
            else
            {
                Instantiate(onHitParticlesPrefab, transform.position, Quaternion.identity);
                livesText.text = (GameManager.Instance.RoundNumber + 1 - hits).ToString();
            }
        }
        if (!isEnemy && collision.CompareTag("EnemyBullet"))
        {

            GameManager.Instance.FinishRound(isEnemy);
        }
    }

    private void Die(bool win)
    {
        if (win == isEnemy)
        {
            mainVisuals.SetActive(false);
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }
    }
}
