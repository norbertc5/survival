using norbertcUtilities.ActionOnTime;
using UnityEngine;
using System;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] Material damageMaterial;

    int health;
    public int Health { get { return health; } set { health = value; } }
    ItemsHealthBar healthBar;
    Hand handScrpit;

    void Start()
    {
        healthBar = FindObjectOfType<ItemsHealthBar>();
        handScrpit = FindObjectOfType<Hand>();
        Health = maxHealth;
    }

    public void GetDamage(int amount)
    {
        Health -= amount;
        healthBar.SetHealthBar(transform.position, (float)Health/100);

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material defaultMaterial = meshRenderer.material;
        meshRenderer.material = damageMaterial;
        ActionOnTime.Create(() => { meshRenderer.material = defaultMaterial; }, .1f, "ReturnDefaultMaterial");

        if(Health <= 0)
        {
            ActionOnTime.Stop("ReturnDefaultMaterial");
            healthBar.Hide();
            handScrpit.attack -= GetDamage;
            Destroy(gameObject);
        }
    }
}
