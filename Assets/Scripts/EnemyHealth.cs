using StarterAssets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
   [SerializeField] float health = 100;
    [SerializeField] GameObject winScreen;
    [SerializeField] TextMeshProUGUI killText;
    static int totalEnemies;
    [SerializeField] EnemyCount enemyCount;
    public  GameObject hitEffect;
    private void Awake()
    {
        enemyCount.enemyCount = 0;
        enemyCount.kills = 0;
    }
    private void Start()
    {
        enemyCount.kills = 0;
        enemyCount.enemyCount++;
        totalEnemies = enemyCount.enemyCount;
        killText.text = enemyCount.kills.ToString() + "/" + totalEnemies.ToString();
    }
    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) { 
            Destroy(gameObject);
            enemyCount.kills++;
            killText.text = enemyCount.kills.ToString() + "/" + totalEnemies.ToString();
            if (enemyCount.kills == totalEnemies) {
                Time.timeScale = 0f;
                FindAnyObjectByType<WeaponSwitcher>().enabled = false;
                FindAnyObjectByType<FirstPersonController>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                winScreen.SetActive(true);
                enemyCount.enemyCount = 0;
                enemyCount.kills = 0;
            }
        }
    }
}
