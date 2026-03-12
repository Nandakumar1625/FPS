using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] GameObject reticle;

    private void Start()
    {
        gameOverCanvas.enabled = false;
    }

    public void HandleDeath()
    {
        
        reticle.SetActive(false);
        Time.timeScale = 0f;
        FindAnyObjectByType<WeaponSwitcher>().enabled = false;
        FindAnyObjectByType<FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}