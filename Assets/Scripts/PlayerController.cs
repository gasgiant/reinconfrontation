using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.velocity = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameManager.Instance.FinishRound(isEnemy);
        }
    }
}
