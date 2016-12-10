using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

    public int maxSpeed;
    private Transform player;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private AudioSource audioSource;

    private bool flying = true;

    ParticleSystem fire;
    ParticleSystem smoke;

    GameObject missle;

    public int explosionRadius;
    public int explosionPower;
    public AudioClip explosionSound;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        fire = transform.FindChild("fire").GetComponent<ParticleSystem>();
        smoke = transform.FindChild("smoke").GetComponent<ParticleSystem>();
        missle = transform.FindChild("missile").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Vector3.Distance(player.position, transform.position)) > AsteroidSpawner.maxDistance)
        {
            StartCoroutine("StopMissle");
        }
    }

    void FixedUpdate()
    {
        if (!flying)
            return;

        rigidBody.AddRelativeForce(Vector3.forward * maxSpeed);
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeed);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            flying = false;
            capsuleCollider.enabled = false;
            audioSource.clip = explosionSound;
            audioSource.Play();
            Destroy(other.gameObject);
            Explode();
            StartCoroutine("StopMissle");
        }
    }

    private IEnumerator StopMissle()
    {
        Destroy(missle);
        audioSource.loop = false;
        fire.Stop();
        smoke.Stop();
        yield return new WaitWhile(fire.IsAlive);
        yield return new WaitWhile(smoke.IsAlive);
        Destroy(gameObject);
    }

    private void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Player"))
                continue;
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0F);

        }
    }
}
