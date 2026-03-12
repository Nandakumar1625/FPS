using UnityEngine;

public class WeaponManager : MonoBehaviour

{
    [SerializeField] ammoSO ammoS;
    [SerializeField] ammoSO ammoA;
    private void Awake()
    {
        ammoS.singleAmmo = 10;
        ammoA.autoAmmo = 60;
    }

}
