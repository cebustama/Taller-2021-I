using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public bool autoRestart = false;

    GameObject playerObject;

    // TODO: Resetear al iniciar la aplicaci�n por primera vez
    // TODO: Agregar sistema de vidas

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        // TODO: No cambiar posici�n original del personaje
        CargarCheckpoint();
    }

    private void Update()
    {
        // Jugador muri�
        if (playerObject == null && autoRestart)
        {
            ReloadLevel();
        }
    }

    public void CargarCheckpoint()
    {
        Transform currentCheckPoint = transform.GetChild(PlayerPrefs.GetInt("checkpoint")).transform;

        // Setear la posici�n del player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = currentCheckPoint.position;

        // Setear la posici�n de la c�mara
        Camera.main.transform.position = new Vector3(currentCheckPoint.position.x, currentCheckPoint.position.y, Camera.main.transform.position.z);
    }

    public void ElegirCheckpoint(int id)
    {
        PlayerPrefs.SetInt("checkpoint", id);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
