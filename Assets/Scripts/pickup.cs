using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class pickup : MonoBehaviour
{
    [SerializeField] int healthAmount, ammoAmount;
    PlayerHealth playerHealth;
    [SerializeField] ammoSO ammo;
    public enum Item
    {
        weapon,
        ammo,
        health
    }
    [SerializeField] Item item;


    private void Start()
    {
        playerHealth = GameObject.Find("PlayerCapsule").GetComponent<PlayerHealth>();
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (item) {
                case Item.health:
                    playerHealth.heal(healthAmount);
                    Destroy(gameObject);
                    break;
                case Item.ammo:
                    if (ammo.ammo == ammoSO.ammoType.Single) ammo.singleAmmo += ammoAmount;
                    else if (ammo.ammo == ammoSO.ammoType.Auto) ammo.autoAmmo += ammoAmount;
                    FindAnyObjectByType<Weapon>().checkAmmo(ammo.ammo);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
