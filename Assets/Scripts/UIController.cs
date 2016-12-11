using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text pitch;
    public Text yaw;
    public Text roll;
    public Text position;

    private Text score;
    private ScoreController scoreController;
  
    private Transform player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        score = transform.FindChild("Score").GetComponent<Text>();
        scoreController = ScoreController.getInstance();
    }
	
	// Update is called once per frame
	void Update () {

        roll.text = Mathf.Atan2(2 * player.rotation.y * player.rotation.w + 2 * player.rotation.x * player.rotation.z, 1 - 2 * player.rotation.y * player.rotation.y - 2 * player.rotation.z * player.rotation.z).ToString();
        pitch.text = Mathf.Atan2(2 * player.rotation.x * player.rotation.w + 2 * player.rotation.y * player.rotation.z, 1 - 2 * player.rotation.x * player.rotation.x - 2 * player.rotation.z * player.rotation.z).ToString();
        yaw.text = Mathf.Asin(2 * player.rotation.x * player.rotation.y + 2 * player.rotation.z * player.rotation.w).ToString();
        position.text = player.position.ToString();
        score.text = scoreController.getScore().ToString();
    }
}
