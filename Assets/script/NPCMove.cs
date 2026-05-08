using UnityEngine;

public class NPCMover : MonoBehaviour
{
    public Transform targetPoint;
    public float speed = 2f;
    private bool hasMoved = false;

    public DialogueSequence nextDialogue; // 🔥 dialog kedua

    private bool isMoving = false;

    public void MoveToTarget()
    {
        if (hasMoved) return;

        hasMoved = true;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving || targetPoint == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            isMoving = false;
            OnArrive();
        }
    }

    void OnArrive()
    {
        Debug.Log("NPC sampai tujuan");

        if (nextDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(nextDialogue); // 🔥 dialog kedua muncul
        }
    }
}