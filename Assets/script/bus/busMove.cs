using UnityEngine;
using System.Collections;

public class MoveToTarget : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float stopDistance = 0.05f;

    public CameraController cameraController;

    private bool hasTriggered = false;

    void Update()
    {
        // 1. Exit if no target or if we've already started the ending sequence
        if (target == null || hasTriggered) return;

        // 2. Calculate distance to target
        float distance = Vector2.Distance(transform.position, target.position);

        // 3. Move toward the target
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // 4. Check if we are close enough to "arrive"
        if (distance <= stopDistance)
        {
            hasTriggered = true; // Prevents Update from running this again
            StartCoroutine(HandleEvent());
        }
    }

    IEnumerator HandleEvent()
    {
        // Optional: Add logic here to tell the CameraController to do something
        // if (cameraController != null) cameraController.DoSomething();

        // Wait for 1.5 seconds to let the player process the arrival
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}