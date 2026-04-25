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
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsPlaying())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 0.4121328f, 0.4121328f);
        }
    }

    void FixedUpdate()
    {
        // Gerak kiri kanan (NO SLIDE)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, 0);
    }
}