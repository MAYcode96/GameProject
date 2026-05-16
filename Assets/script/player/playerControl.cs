using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (DialogueManager.Instance != null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}