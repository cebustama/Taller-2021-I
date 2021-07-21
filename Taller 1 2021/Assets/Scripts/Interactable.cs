using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string[] message;
    private int index = 0;

    public bool playerInRange = false;

    private void Update()
    {
        // Si el jugador está en el rango y apreta el botón de interactuar
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Mostrar texto
            if (message.Length > 0 && index < message.Length) UserInterface.instance.ShowText(message[index++]);
            else ResetText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ResetText();
        }
    }

    private void ResetText()
    {
        index = 0;
        UserInterface.instance.HideText();
    }
}
