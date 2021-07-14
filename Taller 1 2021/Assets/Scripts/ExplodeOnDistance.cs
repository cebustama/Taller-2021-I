using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDistance : MonoBehaviour
{
    public Transform target;
    public float explodeDistance = 3f;
    public GameObject explosionPrefab;
    public float explosionRadius = 3f;
    public float explosionDamage = 10f;

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector2.Distance(target.position, transform.position);
        if (distanceToTarget < explodeDistance)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D c in colliders)
            {
                if (c.CompareTag("Player"))
                {
                    PlayerController player = c.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.Hit(.5f, explosionDamage);
                    }
                }
            }

            gameObject.SetActive(false);
        }
    }
}
