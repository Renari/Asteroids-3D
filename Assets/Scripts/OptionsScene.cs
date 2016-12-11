using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScene : MonoBehaviour {

    Image Easy;
    Image Medium;
    Image Hard;

    private void Start()
    {
        Easy = GameObject.FindGameObjectWithTag("UI").transform.Find("Easy").GetComponent<Image>();
        Medium = GameObject.FindGameObjectWithTag("UI").transform.Find("Medium").GetComponent<Image>();
        Hard = GameObject.FindGameObjectWithTag("UI").transform.Find("Hard").GetComponent<Image>();
        switch (ConfigManager.getInstance().difficulty)
        {
            case ConfigManager.Difficulty.Easy:
                Easy.color = new Color(120f / 255f, 145f / 255f, 1);
                break;
            case ConfigManager.Difficulty.Medium:
                Medium.color = new Color(120f / 255f, 145f / 255f, 1);
                break;
            case ConfigManager.Difficulty.Hard:
                Hard.color = new Color(120f / 255f, 145f / 255f, 1);
                break;
            default:
                break;
        }
    }

    public void HardDifficulty()
    {
        Easy.color = Color.white;
        Medium.color = Color.white;
        Hard.color = new Color(120f / 255f, 145f / 255f, 1);
        ConfigManager.getInstance().SetDifficulty(ConfigManager.Difficulty.Hard);
    }

    public void MediumDifficulty()
    {
        Easy.color = Color.white;
        Medium.color = new Color(120f / 255f, 145f / 255f, 1);
        Hard.color = Color.white;
        ConfigManager.getInstance().SetDifficulty(ConfigManager.Difficulty.Medium);
    }

    public void EasyDifficulty()
    {
        Easy.color = new Color(120f / 255f, 145f / 255f, 1);
        Medium.color = Color.white;
        Hard.color = Color.white;
        ConfigManager.getInstance().SetDifficulty(ConfigManager.Difficulty.Easy);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetRoll(float value)
    {
        ConfigManager.getInstance().rollSpeed = value;
    }

    public void SetMouseSensitivty(float value)
    {
        ConfigManager.getInstance().mouseSensitivity = value;
    }

    public void MainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
