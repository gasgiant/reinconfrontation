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
    //[SerializeField]
    //private TextMeshPro livesText;
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
    }

    private void OnDisable()
    {
        GameManager.ResetEvent.RemoveListener(Reset);
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
            Destroy(gameObject);
        //if (isEnemy)
        //    livesText.text = (GameManager.Instance.RoundNumber + 1 - hits).ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemy && collision.CompareTag("Bullet"))
        {
            Die();
            CameraController.RandomShake(0.5f, 0.06f, 2, 0.2f, 1);
        }
        if (!isEnemy && collision.CompareTag("EnemyBullet"))
        {
            CameraController.RandomShake(0.8f, 0.05f, 3, 5f, 1);
            GameManager.Instance.FinishRound(false);
            mainVisuals.SetActive(false);
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }
    }

    private void Die()
    {
        mainVisuals.SetActive(false);
        Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        CommandManager.Instance.RemoveEnemy(this);



        
    }
}
