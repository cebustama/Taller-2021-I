using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private FireType fireType;
    [SerializeField] private bool oneTimeUse = true;

    [SerializeField] private float powerupTime = float.MaxValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooter shooter = other.GetComponent<PlayerShooter>();
            if (shooter != null)
            {
                shooter.AssignFireType(fireType, powerupTime);
                if (oneTimeUse) gameObject.SetActive(false);
            }
        }
    }
}
