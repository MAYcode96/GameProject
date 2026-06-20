using UnityEngine;

public class PlayerAttackEvent : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public void SpawnBullet()
    {
        GameObject monster =
            GameObject.FindGameObjectWithTag("monster");

        if (monster == null)
        {
            Debug.Log("Monster tidak ditemukan!");
            return;
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectile =
            bullet.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetTarget(monster.transform);
        }
    }
}