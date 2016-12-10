using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 200f;
    public float mouseSensitivity = 2f;
    public float rotationSpeed = 2f;

    public RawImage damageImage;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    private Vector3 directionalForce;
    private Vector3 rotationalForce;

    public GameObject missle;
    public AudioClip shieldSound;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float depth = Input.GetAxis("Depth");

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        float rotation = Input.GetAxis("Rotation") * rotationSpeed;

        directionalForce = new Vector3(horizontal, depth, vertical);
        rotationalForce = new Vector3(mouseY, mouseX, rotation);

        //mute thruster sound if no player input
        if (Mathf.Abs(horizontal) > 0.01f || Mathf.Abs(vertical) > 0.01f || Mathf.Abs(depth) > 0.01f)
            audioSource.mute = false;
        else
            audioSource.mute = true;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(missle, transform.position, transform.rotation);
        }

        // make nearby asteroids fly towards the player
        Collider[] colliders = Physics.OverlapSphere(transform.position, 250);
        foreach (var item in colliders)
        {
            if(item.gameObject.CompareTag("Asteroid"))
            {
                AsteroidController controller = item.GetComponent<AsteroidController>();
                if (!controller.targeting)
                {
                    controller.setTargetLocation(transform.position);
                }
            }
        }
    }

    void FixedUpdate() {
        rigidBody.AddRelativeForce(directionalForce * moveSpeed);
        rigidBody.AddRelativeTorque(rotationalForce);

        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, moveSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            audioSource.PlayOneShot(shieldSound);
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
}
