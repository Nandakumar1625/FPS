using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCount", menuName = "Scriptable Objects/EnemyCount")]
public class EnemyCount : ScriptableObject
{
    public int enemyCount = 0;
    public int kills = 0;
}
