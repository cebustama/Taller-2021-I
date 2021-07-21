using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UserInterface : MonoBehaviour
{
    public static UserInterface instance;

    [Header("Player Stats")]
    public Image playerHealthBar;
    public Image playerStaminaWheel;

    [Header("Textos")]
    public GameObject textWindow;
    public TextMeshProUGUI textText;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ShowText(string textToShow)
    {
        textText.text = textToShow;
        textWindow.SetActive(true);
    }

    public void HideText()
    {
        textWindow.SetActive(false);
    }
}
