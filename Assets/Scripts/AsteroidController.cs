using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {

    private Transform player;
    private Rigidbody rigidBody;
    private int speed;
    public float distanceFromPlayer;
    public float distanceFromTarget;
    private bool reachedTarget = false;
    public int trackingAccuracy;
    public int maxChildren;
    public bool isChild;

    public float movementSpeed;
    
    [HideInInspector]
    private Vector3 targetedPosition;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();

        ConfigManager configManager = ConfigManager.getInstance();
        trackingAccuracy = configManager.trackingAccuracy;
        speed = configManager.asteroidMaxSpeed;

        CalculateTargetLocation();
    }
    
    // Update is called once per frame
    void Update ()
    {
        distanceFromPlayer = Mathf.Abs(Vector3.Distance(player.position, transform.position));
        if (distanceFromPlayer > AsteroidSpawner.maxDistance)
        {
            if (isChild)
            {
                Destroy(transform.gameObject);
                return;
            }
            CalculateTargetLocation();
            reachedTarget = false;
        }

        distanceFromTarget = Mathf.Abs(Vector3.Distance(targetedPosition, transform.position));
        if (distanceFromTarget < 100f)
        {
            reachedTarget = true;
        }
    }

    void FixedUpdate () {
        if (!reachedTarget)
        {
            Vector3 directionalForce = (targetedPosition - transform.position) * speed;
            rigidBody.AddForce(directionalForce);
            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, speed);
        }
    }

    public void addExplosionForce(int explosionPower, Vector3 explosionPos, int explosionRadius)
    {
        // prevent culling on destroyed asteroids
        if (rigidBody != null)
        {
            rigidBody.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);
            reachedTarget = true;
        }
    }

    void CalculateTargetLocation()
    {
        // calculate a random position around the player
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        float height = Random.Range(-trackingAccuracy, trackingAccuracy);
        Vector3 circle = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        circle *= trackingAccuracy;
        circle += player.position;
        circle.y += height;

        targetedPosition = circle;
    }
}
