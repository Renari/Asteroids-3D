using UnityEngine;
using System.Collections;

public class AsteroidController : MonoBehaviour {

    private Transform player;
    private Rigidbody rigidBody;
    private const int speed = 250;

    public float movementSpeed;

    [HideInInspector]
    public bool targeting = false;
    [HideInInspector]
    private Vector3 targetedPosition;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update () {
        float distanceFromPlayer = Mathf.Abs(Vector3.Distance(player.position, transform.position));
        if (distanceFromPlayer > AsteroidSpawner.maxDistance)
        {
            Destroy(this.gameObject);
        }

        if (distanceFromPlayer > 250)
        {
            targeting = false;
        }
    }

    void FixedUpdate () {
        if (targeting)
        {
            Vector3 directionalForce = (targetedPosition - transform.position).normalized * speed;
            rigidBody.AddRelativeForce(-directionalForce);
            
        }

    }

    public void setTargetLocation(Vector3 location) {
        targetedPosition = location;
        targeting = true;
    }
}
