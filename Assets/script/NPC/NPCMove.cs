using UnityEngine;
using System.Collections;

public class NPCMover : MonoBehaviour
{
    [Header("Move Target")]
    public Transform targetPoint;

    [Header("Move Settings")]
    public float moveSpeed = 2f;

    [Header("Destroy After Move")]
    public bool destroyWhenArrived;

    [Header("Animation")]
    public Animator animator;

    [Tooltip("Parameter Animator untuk jalan")]
    public string walkParameter = "isWalking";

    [Tooltip("Sprite Renderer untuk flip arah")]
    public SpriteRenderer spriteRenderer;

    private bool isMoving;

    public void StartMove()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRoutine());
        }
    }

    IEnumerator MoveRoutine()
    {
        isMoving = true;

        // PLAY WALK
        if (animator != null)
        {
            animator.SetBool(walkParameter, true);
        }

        while (Vector2.Distance(
            transform.position,
            targetPoint.position) > 0.05f)
        {
            Vector2 direction =
                (targetPoint.position - transform.position).normalized;

            // GERAK
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPoint.position,
                moveSpeed * Time.deltaTime
            );

            // FLIP KARAKTER
            if (spriteRenderer != null)
            {
                if (direction.x > 0.1f)
                {
                    spriteRenderer.flipX = false;
                }
                else if (direction.x < -0.1f)
                {
                    spriteRenderer.flipX = true;
                }
            }

            yield return null;
        }

        // PLAY IDLE
        if (animator != null)
        {
            animator.SetBool(walkParameter, false);
        }

        if (destroyWhenArrived)
        {
            Destroy(gameObject);
        }

        isMoving = false;
    }
}