using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScenes : MonoBehaviour
{
    public void restart()
    {
        PlayerPrefs.SetInt("round", 1);
        PlayerPrefs.SetInt("cardDrawed", 0);
        SceneManager.LoadScene("game");
    }

    public void run()
    {
        PlayerPrefs.SetInt("round", 1);
        PlayerPrefs.SetInt("cardDrawed", 0);
        SceneManager.LoadScene("game");
    }
}
