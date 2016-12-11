
using UnityEngine;

public class ScoreController  {

    private static ScoreController instance;

    private int score = 0;
    public int[] scores{ get; private set; }
    public string[] names{ get; private set; }

    private ScoreController()
    {

        names = PlayerPrefs.HasKey("ScoreNames") ? PlayerPrefsX.GetStringArray("ScoreNames") : new string[10];
        scores = PlayerPrefs.HasKey("Scores") ? PlayerPrefsX.GetIntArray("Scores") : new int[10];
    }

	public static ScoreController getInstance() {
        if (instance == null) {
            instance = new ScoreController();
        }
        return instance;
    }

    public void addScore(int value) {
        score += value;
    }

    public int getScore() {
        return score;
    }

    public void resetScore() {
        score = 0;
    }

    public void submitScore(string name) {
        int i = 9;
        while (score > scores[i] && i > 0)
        {
            i--;
            scores[i + 1] = scores[i];
            names[i + 1] = names[i];
        }
        names[i] = name;
        scores[i] = score;
        resetScore();
        PlayerPrefsX.SetIntArray("Scores", scores);
        PlayerPrefsX.SetStringArray("Names", names);
        PlayerPrefs.Save();

    }

    public bool isHighScore()
    {
        for (int i = 9; i >= 0; i--)
        {
            if (scores[i] < score)
            {
                return true;
            }
        }
        return false;
    }
}
