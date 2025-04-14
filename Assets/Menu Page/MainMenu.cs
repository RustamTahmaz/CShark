using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnStoreButtonPressed()
    {
        SceneManager.LoadScene("Store");
    }
}