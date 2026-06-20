using UnityEngine;
using System.Collections;

public class NPCMover : MonoBehaviour
{
    public Transform targetPoint;

    public float moveSpeed = 2f;

    public bool destroyWhenArrived;

    public Animator animator;

    public string walkParameter = "isWalking";

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

        // WALK
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

            // MOVE
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPoint.position,
                moveSpeed * Time.deltaTime
            );

            // FLIP
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

        // IDLE
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