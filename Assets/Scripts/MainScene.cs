using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene : MonoBehaviour{

    public void ScoreScene()
    {
        SceneManager.LoadScene("Score");
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void OptionsScene()
    {
        SceneManager.LoadScene("Options");
    }
}
