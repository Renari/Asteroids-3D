using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 200f;
    public float mouseSensitivity;
    public float rotationSpeed;

    [HideInInspector]
    public bool dead = false;

    public RawImage damageImage;

    private Rigidbody rigidBody;
    private AudioSource[] audioSource;

    private Vector3 directionalForce;
    private Vector3 rotationalForce;

    public GameObject missle;
    public AudioClip shieldSound;

    private ShieldController shieldController;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponents<AudioSource>();
        shieldController = GetComponent<ShieldController>();

        ConfigManager configManager = ConfigManager.getInstance();
        rotationSpeed = configManager.rollSpeed;
        mouseSensitivity = configManager.mouseSensitivity;

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update () {
        if (dead)
            return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float depth = Input.GetAxis("Depth");

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        float rotation = Input.GetAxis("Rotation") * rotationSpeed;

        directionalForce = new Vector3(horizontal, depth, vertical);
        rotationalForce = new Vector3(mouseY, mouseX, rotation);

        Debug.Log(rotationalForce);

        //mute thruster sound if no player input
        if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f || Mathf.Abs(depth) > 0.01f)
            audioSource[0].mute = false;
        else
            audioSource[0].mute = true;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(missle, transform.position, transform.rotation);
        }
    }

    void FixedUpdate() {
        if (dead)
            return;
        rigidBody.AddRelativeForce(directionalForce * moveSpeed);
        rigidBody.AddRelativeTorque(rotationalForce);

        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, moveSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            shieldController.damageShields(0.25f);
            audioSource[1].PlayOneShot(shieldSound);
            damageImage.color = new Color(damageImage.color.r, damageImage.color.g, damageImage.color.b, 1);
            StartCoroutine("FadeDamageImage");
        }
    }

    IEnumerator FadeDamageImage()
    {
        for (float i = 1; i >= 0; i-=0.05f)
        {
            damageImage.color = new Color(damageImage.color.r, damageImage.color.g, damageImage.color.b, i);
            yield return new WaitForSeconds(.1f);
        }
    }

    public void SubmitHighScore(string name)
    {
        ScoreController.getInstance().submitScore(name);
        SceneManager.LoadScene("Score");
    }
}
