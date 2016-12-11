using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShieldController : MonoBehaviour {

    private RectTransform shieldImage;
    private GameObject[] barriers = new GameObject[3];
    private float shields = 1f;
    private float shieldRegenSpeed;
    private float shieldHeight;
    private int lives = 3;

    public Canvas highScoreCanvas;

	// Use this for initialization
	void Start () {
        shieldImage = getUIElement("Shield").GetComponent<RectTransform>();
        barriers[0] = getUIElement("Barriers").FindChild("Barrier1").gameObject;
        barriers[1] = getUIElement("Barriers").FindChild("Barrier2").gameObject;
        barriers[2] = getUIElement("Barriers").FindChild("Barrier3").gameObject;
        shieldHeight = shieldImage.sizeDelta.y;
        
        shieldRegenSpeed = ConfigManager.getInstance().shieldRechargeSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        if (shields < 1f)
        {
            shields += shieldRegenSpeed * Time.deltaTime;
        }
        shields = shields > 1f ? 1f : shields;
        shieldImage.sizeDelta = new Vector2(shieldImage.sizeDelta.x, shieldHeight * shields);

    }

    public void damageShields(float amount)
    {
        shields -= amount;
        if (shields < 0f)
        {
            shields = 1f;
            lives--;
            if (lives < 0)
            {
                transform.GetComponent<CapsuleCollider>().enabled = false;
                transform.GetComponent<PlayerController>().dead = true;
                Cursor.lockState = CursorLockMode.None;

                ScoreController scoreController = ScoreController.getInstance();
                GameObject.FindGameObjectWithTag("UI").SetActive(false);
                if (scoreController.isHighScore())
                {
                    highScoreCanvas.gameObject.SetActive(true);
                }
                else
                {
                    SceneManager.LoadScene("Score");
                }
            }
            else
            {
                barriers[lives].SetActive(false);
            }
        }
    }

    private Transform getUIElement(string elementName)
    {
        return GameObject.FindGameObjectWithTag("UI").GetComponent<RectTransform>().FindChild(elementName);
    }

    public void regenerateBarrier()
    {
        if (lives < 3)
        {
            barriers[lives].SetActive(true);
            lives++;
        }
    }
}
