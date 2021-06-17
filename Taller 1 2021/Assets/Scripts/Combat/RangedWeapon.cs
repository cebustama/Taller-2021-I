using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingType
{
    click,
    hold,
    release
}

public class RangedWeapon : PlayerWeapon
{
    public ShootingType shootingType;
    public GameObject projectile;

    public float fireRate = 0.5f;
    float fireTimer = 0;

    public Transform firePoint;

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (equipped)
        {
            // Apuntar hacia el mouse
            Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            float mouseAngle = Vector2.SignedAngle(Vector2.right, dir);

            transform.localEulerAngles = new Vector3(0, 0, mouseAngle);

            fireTimer -= Time.deltaTime;

            bool isShooting = false;
            switch (shootingType)
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
                Shoot(dir);
            }
        }
    }

    public virtual void Shoot(Vector2 direction)
    {
        fireTimer = 1 / fireRate;
        GameObject newProjectile = Instantiate(projectile, firePoint.position, Quaternion.identity);
        newProjectile.GetComponent<Projectile>().Launch(direction, gameObject.tag, gameObject.layer);
    }

}
