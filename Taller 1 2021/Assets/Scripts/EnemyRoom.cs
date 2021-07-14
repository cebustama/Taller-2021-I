using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    Collider2D roomArea;
    public List<GameObject> enemies = new List<GameObject>();
    public int enemiesAlive;

    [Header("Door Objects")]
    public List<GameObject> doors;

    private bool doorsOpen = true;

    private void Start()
    {
        roomArea = GetComponent<Collider2D>();
        FindAllEnemiesInside();
    }

    private void Update()
    {
        CountEnemiesAlive();

        // Ya no quedan enemigos, abrir las puertas
        if (enemiesAlive <= 0 && !doorsOpen)
        {
            OpenDoors();
        }
    }

    // Encuentra todos los enemigos dentro del área mientras que estos tengan la etiqueta "Enemy"
    // y tengan un Collider2D que NO sea trigger
    private void FindAllEnemiesInside()
    {
        enemies = new List<GameObject>();
        Collider2D[] enemyColliders = Physics2D.OverlapAreaAll(roomArea.bounds.min, roomArea.bounds.max);
        foreach (Collider2D enemy in enemyColliders)
        {
            if (enemy.CompareTag("Enemy") && !enemy.isTrigger)
            {
                enemies.Add(enemy.gameObject);
            }
        }
    }

    private void CloseDoors()
    {
        // Recorrer todos los objetos en la lista y activarlos
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }

        doorsOpen = false;
    }

    private void OpenDoors()
    {
        // Recorrer todos los objetos en la lista y activarlos
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }

        doorsOpen = true;
    }

    private void CountEnemiesAlive()
    {
        enemiesAlive = 0;
        foreach (GameObject enemy in enemies)
        {
            // Si enemigo aun existe y también está activado
            if (enemy != null && enemy.activeSelf)
            {
                enemiesAlive++;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            CloseDoors();
        }
    }
}
