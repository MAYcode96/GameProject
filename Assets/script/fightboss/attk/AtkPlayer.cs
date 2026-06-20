using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum AttackType
    {
        FlyToTarget,
        SpawnOnTarget
    }

    public AttackType attackType = AttackType.FlyToTarget;
    public int damage = 20;
    public float speed = 5f;
    private Transform target;
    private bool isGone = false;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        if (attackType == AttackType.SpawnOnTarget)
        {
            if (target != null)
            {
                transform.position = target.position;
                HitTarget();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (isGone)
            return;

        if (attackType != AttackType.FlyToTarget)
            return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (isGone)
            return;

        isGone = true;

        if (target != null)
        {
            MonsterHealth monster = target.GetComponent<MonsterHealth>();

            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}