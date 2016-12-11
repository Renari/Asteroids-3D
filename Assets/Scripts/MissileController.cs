using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

    public int maxSpeed;
    private Transform player;
    private Rigidbody rigidBody;
    private CapsuleCollider capsuleCollider;
    private AudioSource audioSource;
    private ScoreController scoreController;
    private AsteroidSpawner asteroidSpawner;

    private bool flying = true;

    ParticleSystem fire;
    ParticleSystem smoke;
    ParticleSystem explosion;

    GameObject missle;

    public int explosionRadius;
    public int explosionPower;
    public AudioClip explosionSound;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        asteroidSpawner = player.GetComponent<AsteroidSpawner>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
        rigidBody = GetComponent<Rigidbody>();
        fire = transform.FindChild("fire").GetComponent<ParticleSystem>();
        smoke = transform.FindChild("smoke").GetComponent<ParticleSystem>();
        explosion = transform.FindChild("explosion").GetComponent<ParticleSystem>();
        missle = transform.FindChild("missile").gameObject;
        scoreController = ScoreController.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Vector3.Distance(player.position, transform.position)) > 500)
        {
            audioSource.Stop();
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
            AddScore();
            rigidBody.velocity = Vector3.zero;
            flying = false;
            capsuleCollider.enabled = false;
            audioSource.loop = false;
            audioSource.clip = explosionSound;
            audioSource.Play();
            explosion.Play();
            SpawnChildAsteroids(other.gameObject);
            Destroy(other.gameObject);
            Explode();
            StartCoroutine("StopMissle");
        }
    }

    private void SpawnChildAsteroids(GameObject asteroid)
    {
        AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();
        int number = Random.Range(0, asteroidController.maxChildren);
        for (int i = 0; i < number; i++)
        {
            asteroidSpawner.spawnChildAsteroid(asteroid.transform.position);
        }
    }

    private void AddScore()
    {
        switch (ConfigManager.getInstance().difficulty)
        {
            case ConfigManager.Difficulty.Easy:
                scoreController.addScore(1);
                if (scoreController.getScore() % 25 == 0)
                {
                    player.GetComponent<ShieldController>().regenerateBarrier();
                }
                break;
            case ConfigManager.Difficulty.Medium:
                scoreController.addScore(2);
                if (scoreController.getScore() % 100 == 0)
                {
                    player.GetComponent<ShieldController>().regenerateBarrier();
                }
                break;
            case ConfigManager.Difficulty.Hard:
                scoreController.addScore(3);
                break;
            default:
                break;
        }
    }

    private IEnumerator StopMissle()
    {
        Destroy(missle);
        fire.Stop();
        smoke.Stop();
        yield return new WaitForSeconds(3);
        explosion.Stop();
        yield return new WaitWhile(fire.IsAlive);
        yield return new WaitWhile(smoke.IsAlive);
        yield return new WaitWhile(explosion.IsAlive);
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
            else if (hit.CompareTag("Asteroid"))
            {
                AsteroidController asteroidController = hit.GetComponent<AsteroidController>();
                asteroidController.addExplosionForce(explosionPower, explosionPos, explosionRadius);
            }

        }
    }
}
