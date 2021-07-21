using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public FireType currentFireType;

    FireType defaultFireType;

    float fireTimer = 0;

    float fireTypeTimer = 0;

    public enum AimType
    {
        top,
        right,
        mouse
    }

    public AimType aimType;

    private Vector2 direction;

    private void Start()
    {
        defaultFireType = currentFireType;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFireType == null)
        {
            return;
        }

        // Si le asigné un timer al tipo de disparo, descontar tiempo y luego volver al default
        if (fireTypeTimer > 0)
        {
            fireTypeTimer -= Time.deltaTime;
            if (fireTypeTimer <= 0)
            {
                currentFireType = defaultFireType;
            }
        }

        if (aimType == AimType.mouse)
        {
            direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }
        else if (aimType == AimType.top)
        {
            direction = Vector2.up;
        }
        else if (aimType == AimType.right)
        {
            direction = Vector2.right;
        }

        fireTimer -= Time.deltaTime;

        bool isShooting = false;
        switch (currentFireType.shootingType)
        {
            case ShootingType.click:
                isShooting = Input.GetMouseButtonDown(0);
                break;
            case ShootingType.hold:
                isShooting = Input.GetMouseButton(0);
                break;
            case ShootingType.release:
                isShooting = Input.GetMouseButtonUp(0);
                break;
        }

        if (isShooting && fireTimer <= 0)
        {
            fireTimer =  1 / currentFireType.rateOfFire;
            //GameObject newProjectile = Instantiate(currentFireType.projectilePrefab, transform.position, Quaternion.identity);
            //newProjectile.GetComponent<Projectile>().Launch(direction, gameObject.tag, gameObject.layer);

            float facingRotation = Mathf.Atan2(direction.y, direction.x);
            float startRotation = facingRotation + currentFireType.projectileSpreadAngle / 2f;
            float angleIncrease = currentFireType.projectileSpreadAngle / (currentFireType.projectileAmount - 1f);

            for (int i = 0; i < currentFireType.projectileAmount; i++)
            {
                float currentRotation = startRotation - angleIncrease * i;
                Vector2 currentDirection = Quaternion.AngleAxis(currentRotation, Vector3.back) * direction;
                GameObject newProjectile = Instantiate(currentFireType.projectilePrefab, transform.position, 
                    Quaternion.Euler(0f, 0f, facingRotation * Mathf.Rad2Deg));
                newProjectile.GetComponent<Projectile>().Launch(currentDirection.normalized, gameObject.tag, gameObject.layer);
            }

        }
    }

    public void AssignFireType(FireType newFireType, float time = float.MaxValue)
    {
        currentFireType = newFireType;

        if (time == float.MaxValue)
        {
            defaultFireType = currentFireType;
        }
        else
        {
            fireTypeTimer = time;
        }
    }
}
