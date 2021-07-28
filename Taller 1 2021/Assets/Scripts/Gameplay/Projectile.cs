using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float moveSpeed;
    public float rotateSpeed = 0f;
    private Vector2 directionToMove;
    public AudioClip sound;

    [Header("Lifetime")]
    public float lifetime;
    private float lifeTimeSeconds;
    public GameObject destroyEffect;

    private Rigidbody2D rb;

    private Hit hit;

    List<string> tagExceptions = new List<string>();

    private void Start()
    {
        lifeTimeSeconds = lifetime;
        rb = GetComponent<Rigidbody2D>();
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
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        rb.velocity = initialVel * moveSpeed;
        rb.angularVelocity = rotateSpeed;

        // Detecci�n de colisiones
        if (tagException != "")
        {
            hit = GetComponent<Hit>();
            if (hit != null) hit.tagHitFilterList.Add(tagException);

            tagExceptions.Add(tagException);
        }

        if (layerException != int.MinValue)
        {
            gameObject.layer = layerException;
            hit = GetComponent<Hit>();
            if (hit != null)
            {
                hit.layerHitFilterList.Add(layerException);
            }
        }

        // Si tiene asignado un sonido, lo reproduce
        if (sound != null)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.pitch = Random.Range(0.9f, 1.1f);
            source.PlayOneShot(sound);
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

        //Debug.Log(other.name);

        // Solo destruir con el player
        /*PlayerController player = other.GetComponent<PlayerController>();
        Enemy enemy = other.GetComponent<Enemy>();
        if (player != null || enemy != null || other.gameObject.layer == LayerMask.NameToLayer("Collisions"))
        {
            if (destroyEffect != null) Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }*/
    }
}
