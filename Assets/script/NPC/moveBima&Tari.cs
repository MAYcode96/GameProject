using UnityEngine;

public class NPCFollower : MonoBehaviour
{
    [Header("Follow")]
    public float speed = 3f;
    public float stopDistance = 1.5f;

    [Header("Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Transform player;

    void Start()
    {
        FindPlayer();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        FollowPlayer();
    }

    void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        bool isMoving = distance > stopDistance;

        animator.SetBool("isWalking", isMoving);

        if (isMoving)
        {
            // Gerak hanya sumbu X
            Vector3 targetPosition = new Vector3(
                player.position.x,
                transform.position.y,
                transform.position.z
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            // Tentukan arah hadap
            if (player.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false; // kanan
            }
            else
            {
                spriteRenderer.flipX = true; // kiri
            }
        }
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
}