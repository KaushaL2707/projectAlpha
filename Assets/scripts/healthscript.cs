using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currnetHealth;

    void Start()
    {
        currnetHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currnetHealth -= damage;

        if (currnetHealth <= 0) Die();
    }

    void Die()
    {
        Destroy(gameobject);
    }
} 