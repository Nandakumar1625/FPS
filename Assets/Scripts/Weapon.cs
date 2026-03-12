using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeReference] Camera FPCamera;
    AudioSource audioSource;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] WeaponSO weapon;
    public ammoSO ammo;
    UIManager uiManager;
    [SerializeField] GameObject ui;
    bool isFiring = false;
    float firingTimer;
    public int loadedBullets;
  
    private void Awake()
    {
        if (ui != null) uiManager = ui.GetComponent<UIManager>();
        firingTimer = weapon != null ? weapon.firingInterval : 0;
        loadedBullets = weapon != null ? weapon.bulletPerRound : 0;
    }

    private void Start()
    {
        audioSource = GameObject.Find("Weapons")?.GetComponent<AudioSource>();
        if (ammo != null && uiManager != null) checkAmmo(ammo.ammo);
    }
    void Update()
    {
        if (firingTimer < weapon.firingInterval) firingTimer += Time.deltaTime;
        if (Input.GetButtonDown("Fire1")&&loadedBullets>0) isFiring = true;
        if (isFiring&&(Input.GetButtonUp("Fire1") || loadedBullets <= 0)) { isFiring = false;}
        if (isFiring && firingTimer >= weapon.firingInterval) { 
            shoot();
            firingTimer = 0;
        }
    }
    private void shoot()
    {
        RaycastHit hit;
        PlaymuzzleFlash();
        audioSource.PlayOneShot(weapon.firingSFX);
        if (loadedBullets > 0) { loadedBullets--; checkAmmo(ammo.ammo); }
        else checkAmmo(ammo.ammo);
           
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, weapon.range))
        {
            CreateHitImpact(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target != null) target.takeDamage(weapon.damage);
            else return;
        }

    }
    private void PlaymuzzleFlash()
    {
        muzzleFlash.Play();
    }
    private void CreateHitImpact(RaycastHit hit)
    {
        GameObject currentHitEffect = hitEffect;
        if (hit.transform.CompareTag("Enemy")) { 
            currentHitEffect = hit.transform.GetComponent<EnemyHealth>().hitEffect; 
        }
        GameObject Impact = Instantiate(currentHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(Impact, 2f);
    }
    public void checkAmmo(ammoSO.ammoType type)
    {
        if (uiManager == null || ammo == null) return;
        
        if (type == ammoSO.ammoType.Single) { uiManager.updateBulletText(loadedBullets, ammo.singleAmmo); }
        else if (type == ammoSO.ammoType.Auto) { uiManager.updateBulletText(loadedBullets, ammo.autoAmmo); }
        if (loadedBullets == 0)StartCoroutine(reload(type));
    }
    public IEnumerator reload(ammoSO.ammoType type) {
        audioSource.PlayOneShot(weapon.reloadSFX);
        yield return new WaitUntil(()=>!audioSource.isPlaying);
        if (type == ammoSO.ammoType.Single && ammo.singleAmmo >= weapon.bulletPerRound && loadedBullets == 0)
        {
                loadedBullets = weapon.bulletPerRound;
          
                ammo.singleAmmo -= weapon.bulletPerRound;
                uiManager.updateBulletText(loadedBullets, ammo.singleAmmo);
        }
        if (type == ammoSO.ammoType.Auto && ammo.autoAmmo >= weapon.bulletPerRound && loadedBullets == 0)
        {
                loadedBullets = weapon.bulletPerRound;
                ammo.autoAmmo -= weapon.bulletPerRound;
                uiManager.updateBulletText(loadedBullets, ammo.autoAmmo);
        }
        else if(ammo.singleAmmo > 0 && loadedBullets == 0)
        {
            loadedBullets = ammo.singleAmmo;
            ammo.singleAmmo = 0 ;
            uiManager.updateBulletText(loadedBullets, ammo.singleAmmo);
        }
        else if (ammo.autoAmmo > 0 && loadedBullets == 0)
        {
            loadedBullets = ammo.autoAmmo;
            ammo.autoAmmo = 0;
            uiManager.updateBulletText(loadedBullets, ammo.autoAmmo);
        }
        

    }
   
   
}
