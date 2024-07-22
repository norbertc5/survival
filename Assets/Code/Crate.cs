using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 100;

    int health;
    public int Health { get { return health; } set { health = value; } }
    ItemsHealthBar healthBar;

    void Start()
    {
        healthBar = FindObjectOfType<ItemsHealthBar>();
        Health = maxHealth;
    }

    public void GetDamage(int amount)
    {
        Health -= amount;
        healthBar.SetHealthBar(transform.position, (float)Health/100);
        print($"got {amount} damage. Remain {Health} health.");
    }
}
