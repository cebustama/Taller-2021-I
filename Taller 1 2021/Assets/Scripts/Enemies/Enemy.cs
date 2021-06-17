using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Enemy Settings")]
    public int maxHealth;
    float health;

    public string enemyName;

    [Header("Effects")]
    public GameObject deathEffect;
    private float deathEffectDeathTime = 1f;

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Animator animator;

    public const float arrivedDistance = 0.5f;

    protected Vector3 homePosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        homePosition = transform.position;
        health = maxHealth;
    }

    private void OnEnable()
    {
        transform.position = homePosition;
        health = maxHealth;
        currentState = EnemyState.idle;
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DeathEffect();
            this.gameObject.SetActive(false);
        }
    }

    private void DeathEffect()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDeathTime);
        }
    }

    public void Hit(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        //Debug.Log("Soy " + enemyName + " y me están atacando");
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null && currentState != EnemyState.stagger)
        {
            currentState = EnemyState.stagger;
            yield return new WaitForSeconds(knockTime);

            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
}
