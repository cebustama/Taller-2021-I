using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public int mouseButton = 0;
    public FireType currentFireType;

    FireType defaultFireType;

    float fireTimer = 0;

    float fireTypeTimer = 0;

    public string ammoType;
    public int currentTypeAmmo;

    Dictionary<string, int> currentAmmo = new Dictionary<string, int>();

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
        currentAmmo = new Dictionary<string, int>();
        defaultFireType = currentFireType;
        AssignFireType(defaultFireType);
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
                AssignFireType(defaultFireType);
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
                isShooting = Input.GetMouseButtonDown(mouseButton);
                break;
            case ShootingType.hold:
                isShooting = Input.GetMouseButton(mouseButton);
                break;
            case ShootingType.release:
                isShooting = Input.GetMouseButtonUp(mouseButton);
                break;
        }

        if (isShooting && fireTimer <= 0 && currentTypeAmmo >= currentFireType.ammoCost)
        {
            fireTimer =  1 / currentFireType.rateOfFire;
            //GameObject newProjectile = Instantiate(currentFireType.projectilePrefab, transform.position, Quaternion.identity);
            //newProjectile.GetComponent<Projectile>().Launch(direction, gameObject.tag, gameObject.layer);

            float facingRotation = Mathf.Atan2(direction.y, direction.x);
            float startRotation = facingRotation + currentFireType.projectileSpreadAngle / 2f;
            float angleIncrease = currentFireType.projectileSpreadAngle / (currentFireType.projectileAmount - 1f);

            // Mas de uno
            if (currentFireType.projectileAmount > 1)
            {
                for (int i = 0; i < currentFireType.projectileAmount; i++)
                {
                    float currentRotation = startRotation - angleIncrease * i;
                    Vector2 currentDirection = Quaternion.AngleAxis(currentRotation, Vector3.back) * direction;
                    GameObject newProjectile = Instantiate(currentFireType.projectilePrefab, transform.position,
                        Quaternion.Euler(0f, 0f, facingRotation * Mathf.Rad2Deg));
                    newProjectile.GetComponent<Projectile>().Launch(currentDirection.normalized, gameObject.tag, gameObject.layer);
                }
            }
            // Solo uno
            else
            {
                GameObject newProjectile = Instantiate(currentFireType.projectilePrefab, transform.position,
                        Quaternion.Euler(0f, 0f, facingRotation * Mathf.Rad2Deg));
                newProjectile.GetComponent<Projectile>().Launch(direction, gameObject.tag, gameObject.layer);
            }

            currentAmmo[currentFireType.name] -= currentFireType.ammoCost;
            UpdateAmmo();
        }
    }

    public void AssignFireType(FireType newFireType, float time = float.MaxValue)
    {
        currentFireType = newFireType;

        // Municion
        ammoType = currentFireType.name;
        if (!currentAmmo.ContainsKey(ammoType)) currentAmmo.Add(ammoType, 0);

        UserInterface.instance.ShowAmmo(currentFireType.projectileSprite, 
            currentFireType.ammoCost > 0 ? currentAmmo[ammoType] : -1);
        UpdateAmmo();

        if (time == float.MaxValue)
        {
            defaultFireType = currentFireType;
        }
        else
        {
            fireTypeTimer = time;
        }
    }

    public void AddAmmo(string ammoType, int amount)
    {
        if (!currentAmmo.ContainsKey(ammoType)) currentAmmo.Add(this.ammoType, 0);
        currentAmmo[ammoType] += amount;
        UpdateAmmo();
    }

    private void UpdateAmmo()
    {
        currentTypeAmmo = currentAmmo[ammoType];
        UserInterface.instance.UpdateAmmo(currentTypeAmmo);
    }
}
