using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEngine.SceneManagement;

public class HoleTile : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float fallTime = .5f;

    [SerializeField] private bool changeScene;
    [SerializeField] private int nextSceneIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && other.isTrigger)
        {
            StartCoroutine(FallCo(player));
        }
    }

    private IEnumerator FallCo(PlayerController player)
    {
        // Position del player en la grilla
        Vector3Int playerGridPos = new Vector3Int(
            Mathf.FloorToInt(player.transform.position.x),
            Mathf.FloorToInt(player.transform.position.y),
            0);

        Vector3 playerWorldPos = tilemap.GetCellCenterLocal(playerGridPos);
        Vector2Int changeInt = new Vector2Int(Mathf.CeilToInt(player.change.x), Mathf.CeilToInt(player.change.y));

        // Dirección contraria a la entrada para volver
        Vector2Int returnDir = changeInt * -1;

        player.Fall();

        Vector2 startingPos = player.rb.position;
        Vector2 finalPos = startingPos + changeInt;

        Vector3 startingScale = player.transform.localScale;

        float fallTimer = 0;

        while (fallTimer < fallTime)
        {
            fallTimer += Time.deltaTime;
            // Mover al player hacia la dirección que está cayendo
            player.rb.position = Vector2.Lerp(startingPos, finalPos, (fallTimer / fallTime));
            player.transform.localScale = Vector3.Lerp(startingScale, Vector3.zero, (fallTimer / fallTime));
            yield return null;
        }
        
        // Cambiar a otra escena
        if (changeScene)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        // Volver a lugar desde el que cayó
        else
        {
            player.transform.position = playerWorldPos;
            player.transform.localScale = startingScale;

            player.FinishFall();
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && other.isTrigger)
        {
            Debug.Log("EXIT " + other.name);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null && other.isTrigger)
        {
            //Debug.Log("DENTRO " + other.name);
        }
    }
}
