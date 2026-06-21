using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float footstepInterval = 0.35f; // Sisakan intervalnya saja

    private Rigidbody2D rb;
    private float moveInput;
    private Animator anim;
    private bool canMove = true;
    private float footstepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove)
        {
            moveInput = 0;
            anim.SetFloat("speed", 0);
            StopFootstep();
            return;
        }

        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.isDialogueOpen)
        {
            moveInput = 0;
            anim.SetFloat("speed", 0);
            StopFootstep();
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(moveInput));

        if (moveInput != 0)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootstep();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        if (moveInput > 0)
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
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

    void PlayFootstep()
    {
        // Memanggil sound effect manager kamu
        SoundEffectManager.Play("footstep");
    }

    void StopFootstep()
    {
        footstepTimer = 0f;
    }

    public void SetMovement(bool value)
    {
        canMove = value;
    }
}