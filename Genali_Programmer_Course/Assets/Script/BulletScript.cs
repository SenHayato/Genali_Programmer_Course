using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;

    void BulletMove()
    {
        transform.localPosition += bulletSpeed * Time.deltaTime * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<EnemyActive>(out var enemyActive))
            {
                enemyActive.TakeDamage(bulletDamage);
            }
            Destroy(gameObject);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
        if (this != null)
        {
            Invoke(nameof(DestroyBullet), 5f);
        }
    }
}
