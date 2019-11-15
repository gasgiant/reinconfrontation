using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyImpulse(Vector3 impulse)
    {
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }
}
