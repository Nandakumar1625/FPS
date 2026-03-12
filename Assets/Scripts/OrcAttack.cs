using UnityEngine;

public class OrcAttack : MonoBehaviour
{
    [SerializeField] LayerMask attackLayer;
    [SerializeField] float attackRadius;
    [SerializeField] Transform attackPoint;
    private EnemyHealth enemyHealth;
    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }
    private void attack()
    {
        bool collInfo = Physics.CheckSphere(attackPoint.position, attackRadius, attackLayer);
        if (collInfo) { PlayerHealth.Instance.getDamage(10); }
    }
}
