using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActive : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] float detectionRange;
    [SerializeField] float moveSpeed;

    [Header("Reference")]
    [SerializeField] GameManager gameManager;
    [SerializeField] Transform playerTransform;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerTransform = FindFirstObjectByType<PlayerActive>().transform;
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    bool PlayerDetected()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        return distance < detectionRange && distance > 2f;
    }

    void ChasePlayer()
    {
        if (PlayerDetected())
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    void Dead()
    {
        gameManager.enemyKilled++;
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        ChasePlayer();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Dead();
        }
    }
}
