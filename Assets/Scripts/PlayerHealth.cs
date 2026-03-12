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
        if (hurtSFX != null && hurtSFX.Count > 0 && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Count)]);
        }

        if (PlayerSO != null)
        {
            PlayerSO.health -= damage;
            if (healthBar != null) healthBar.fillAmount = PlayerSO.health / 100f;

            if (PlayerSO.health <= 0)
            {
                HandleDeath();
            }
        }
    }
    public void heal(int amount)
    {
        if (PlayerSO == null) return;

        if (PlayerSO.health < 100)
        {
            PlayerSO.health += amount;
            if (healthBar != null) healthBar.fillAmount = PlayerSO.health / 100f;
        }
    }
    public void HandleDeath()
    {
        if (enemyCount != null)
        {
            enemyCount.enemyCount = 0;
            enemyCount.kills = 0;
        }

        WeaponSwitcher ws = FindFirstObjectByType<WeaponSwitcher>();
        if (ws != null) ws.enabled = false;

        FirstPersonController fpc = FindFirstObjectByType<FirstPersonController>();
        if (fpc != null) fpc.enabled = false;

        if (animator != null) animator.enabled = true;
        if (gameOverScreen != null) gameOverScreen.SetActive(true);

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (weapon != null) weapon.SetActive(false);
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
