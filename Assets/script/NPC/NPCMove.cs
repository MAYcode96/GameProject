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

        while (Vector2.Distance(
            transform.position,
            targetPoint.position) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPoint.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        if (destroyWhenArrived)
        {
            Destroy(gameObject);
        }

        isMoving = false;
    }
}