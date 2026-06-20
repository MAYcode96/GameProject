using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + " terkena " + damage + " damage");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}