using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float moveSpeed;
    public Vector2 directionToMove;

    [Header("Lifetime")]
    public float lifetime;
    private float lifeTimeSeconds;

    public Rigidbody2D rb;

    private Hit hit;

    List<string> tagExceptions = new List<string>();

    private void Start()
    {
        lifeTimeSeconds = lifetime;
    }

    private void Update()
    {
        lifeTimeSeconds -= Time.deltaTime;
        if (lifeTimeSeconds <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 initialVel, string tagException = "", int layerException = int.MinValue)
    {
        rb.velocity = initialVel * moveSpeed;

        if (tagException != "")
        {
            hit = GetComponent<Hit>();
            if (hit != null) hit.tagHitFilterList.Add(tagException);

            tagExceptions.Add(tagException);
        }

        if (layerException != int.MinValue)
        {
            gameObject.layer = layerException;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check tag exceptions
        foreach (string s in tagExceptions)
        {
            if (other.CompareTag(s)) return;
        }

        // Check layer
        if (other.gameObject.layer == gameObject.layer) return;

        Debug.Log(other.name);

        Destroy(gameObject);
    }
}
