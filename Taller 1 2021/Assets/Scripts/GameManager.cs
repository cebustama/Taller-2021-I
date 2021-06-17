using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool esconderMouse = false;
    public GameObject menuDePausa;

    private void Awake()
    {
        //Cursor.visible = esconderMouse;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.Quit();
            TogglePausa();
        }

        // Cámara lenta
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            Time.timeScale = 1;
        }
        
    }

    public void TogglePausa()
    {
        // Activar la pausa
        if (!menuDePausa.activeSelf)
        {
            menuDePausa.SetActive(true);
            Time.timeScale = 0;
        }
        // Desactivar la pause
        else
        {
            menuDePausa.SetActive(false);
            Time.timeScale = 1;
        }
    }
}