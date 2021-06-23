using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public int newGameSceneIndex = 1;

    public Image fadeIn;
    public float fadeTime = 2f;

    public GameObject menuSeleccion;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("firstTimePlaying") == 0)
        {
            PlayerPrefs.SetInt("level1locked", 0);
            PlayerPrefs.SetInt("level2locked", 1);
            PlayerPrefs.SetInt("firstTimePlaying", 1);
        }
    }

    public void StartNewGame()
    {
        //SceneManager.LoadScene(newGameSceneIndex);
        //StartCoroutine(FadeInCo());

        menuSeleccion.SetActive(true);
    }

    private IEnumerator FadeInCo()
    {
        float time = 0;
        while (fadeIn.color.a < 1)
        {
            time += Time.deltaTime;
            float newAlpha = (time / fadeTime);
            fadeIn.color = new Color(fadeIn.color.r, fadeIn.color.g, fadeIn.color.b, newAlpha);
            yield return null;
        }

        SceneManager.LoadScene(newGameSceneIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadNewScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("level1locked", 0);
        PlayerPrefs.SetInt("level2locked", 1);

        PlayerPrefs.SetInt("firstTimePlaying", 0);
    }
}
