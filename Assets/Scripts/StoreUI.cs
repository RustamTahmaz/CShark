using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreUI : MonoBehaviour
{
    /// <summary>
    /// Call this from your “Back” button to return to the Main Menu scene.
    /// </summary>
    public void OnBackPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Call this from your “Play” (or “Start Game”) button to go into the Game scene.
    /// </summary>
    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Game");  // or whatever your gameplay scene is called
    }
}
