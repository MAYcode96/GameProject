using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;
    private float moveInput;
    private Animator anim;

    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // ❌ kalau lagi dialog, stop input
        if (!canMove)
        {
            moveInput = 0;
            anim.SetFloat("speed", 0);
            return;
        }

        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.isDialogueOpen)
        {
            moveInput = 0;
            anim.SetFloat("speed", 0);
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        anim.SetFloat("speed", Mathf.Abs(moveInput));

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
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // 🔥 dipanggil dari NPC / Dialogue system
    public void SetMovement(bool value)
    {
        canMove = value;
    }
}