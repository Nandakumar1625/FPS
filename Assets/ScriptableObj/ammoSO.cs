using UnityEngine;

[CreateAssetMenu(fileName = "ammoSO", menuName = "Scriptable Objects/ammoSO")]
public class ammoSO : ScriptableObject
{
    public enum ammoType
    {
        Single,
        Burst,
        Auto
    }
    public ammoType ammo;
    public int singleAmmo;
    public int autoAmmo;
}
