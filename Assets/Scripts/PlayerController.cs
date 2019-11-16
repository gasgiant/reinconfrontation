using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainVisuals;
    [SerializeField]
    private GameObject deathParticles;
    [SerializeField]
    private bool isEnemy;

    private Rigidbody2D rb;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameManager.Instance.FinishRound(isEnemy);
        }
    }

    private void Die(bool win)
    {
        if (win == isEnemy)
        {
            mainVisuals.SetActive(false);
            deathParticles.SetActive(true);
        }
    }
}
