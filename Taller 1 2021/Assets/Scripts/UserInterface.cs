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

    [Header("Municiones")]
    public GameObject municionesContainer;
    public Image municionesSprite;
    public TextMeshProUGUI municionesTexto;

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

    public void ShowAmmo(Sprite proj, int amount)
    {
        municionesSprite.sprite = proj;
        municionesTexto.text = (amount >= 0) ? amount.ToString() : "";

        municionesContainer.SetActive(true);
    }

    public void HideAmmo()
    {
        municionesContainer.SetActive(false);
    }

    public void UpdateAmmo(int amount)
    {
        municionesTexto.text = amount.ToString();
    }
}
