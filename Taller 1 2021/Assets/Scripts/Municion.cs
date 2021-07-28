using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Municion : MonoBehaviour
{
    public FireType tipoMunicion;
    public int cantidad = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerShooter shooter = other.GetComponent<PlayerShooter>();
            shooter.AddAmmo(tipoMunicion.name, cantidad);
            Destroy(gameObject);
        }
    }


}
