using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]PlayerSO PlayerSO;
    [SerializeField] EnemyCount enemyCount;
    Animator animator;
    [SerializeField] AnimationClip dieAnim;
    GameObject weapon;
    [SerializeField] Image healthBar;
    [SerializeField] GameObject gameOverScreen;
    private AudioSource audioSource;
    [SerializeField] List<AudioClip> hurtSFX = new List<AudioClip>();
    int listCount;
    public static PlayerHealth Instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists and destroy this one if it does
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign this instance as the singleton instance
        Instance = this;

    }
    void Start()
    {
        Time.timeScale = 1;
        PlayerSO.health = 100;
        weapon =  GameObject.Find("Weapons");
        animator = GetComponent<Animator>();
        healthBar.fillAmount = PlayerSO.health/100f;
        animator.enabled = !enabled;
        audioSource = GetComponent<AudioSource>();
        listCount = hurtSFX.Count;
    }


    public void getDamage(int damage)
    {
        audioSource.PlayOneShot(hurtSFX[Random.Range(0,listCount)]);
        PlayerSO.health-=damage;
        healthBar.fillAmount -= damage/100f;
        if (PlayerSO.health <= 0)
        {
            HandleDeath();
        }
    }
    public void heal(int amount)
    {
        if (PlayerSO.health < 100)
        {
            PlayerSO.health += amount;
            healthBar.fillAmount += amount / 100f;
        }
    }
    public void HandleDeath()
    {
        enemyCount.enemyCount = 0;
        enemyCount.kills = 0;
        FindFirstObjectByType<WeaponSwitcher>().enabled = false;
        FindFirstObjectByType<FirstPersonController>().enabled = false;
        animator.enabled = true;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        weapon.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Arrow")) getDamage(10);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")HandleDeath();
    }

}
