using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] int damage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) PlayerHealth.Instance.getDamage(damage);
    }
}
