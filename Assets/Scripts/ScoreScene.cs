using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScene : MonoBehaviour {
    ScoreController scoreController;

    // Use this for initialization
    void Start()
    {
        scoreController = ScoreController.getInstance();
        Transform ui = GameObject.FindGameObjectWithTag("UI").GetComponent<Transform>();
        for (int i = 0; i < 10; i++)
        {
            Text scoreName = ui.FindChild("ScoreName" + i).GetComponent<Text>();
            scoreName.text = scoreController.names[i];

            Text score = ui.FindChild("Score" + i).GetComponent<Text>();
            score.text = scoreController.scores[i] > 0 ? scoreController.scores[i].ToString() : "";
        }
    }
    
    public void MainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
