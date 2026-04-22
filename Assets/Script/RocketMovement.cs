using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D หรือ ← →
        float moveY = Input.GetAxis("Vertical");   // W/S หรือ ↑ ↓

        Vector2 movement = new Vector2(moveX, moveY);
        rb.linearVelocity = movement * speed;
    }
}