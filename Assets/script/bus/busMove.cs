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
        if (target == null || hasTriggered) return;

        float distance = Vector2.Distance(transform.position, target.position);

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (distance <= stopDistance)
        {
            hasTriggered = true; 
            StartCoroutine(HandleEvent());
        }
    }

    IEnumerator HandleEvent()
    {
       
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }
}