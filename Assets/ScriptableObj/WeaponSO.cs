using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public AudioClip firingSFX,reloadSFX;
    public float range = 10;
    public float damage = 1;
    public float firingInterval = 0.5f;
    public int bulletPerRound;
}
