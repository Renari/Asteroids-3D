using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

    public GameObject[] asteroids;
    public int maxAstroids;
    public int maxRotationSpeed;
    public int minScale;
    public int maxScale;

    public static int minDistance = 250;
    public static int maxDistance = 1000;

    private Transform player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnAsteroids(maxAstroids, maxDistance, maxDistance - 50);
    }
    
    // Update is called once per frame
    void Update () {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        int numAsteroids = 0;
        foreach (var asteroid in asteroids)
        {
            if(!asteroid.GetComponent<AsteroidController>().isChild)
            {
                numAsteroids++;
            }
        }
        if (numAsteroids < maxAstroids)
        {
            spawnAsteroids(maxAstroids - numAsteroids, minDistance, maxDistance - 50);
        }
    }

    public void spawnAsteroids(int number, int minRange, int maxRange)
    {
        for (int i = 0; i < number; i++)
        {
            // calculate a random position around the player
            float angle = Random.Range(0.0f, Mathf.PI * 2);
            float height = Random.Range(-maxRange, maxRange);
            Vector3 circle = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            circle *= Random.Range(minRange, maxRange);
            circle += player.position;
            circle.y += height;

            spawnAsteroidAtLocation(circle);
        }
    }

    public GameObject spawnAsteroidAtLocation(Vector3 location)
    {
        int index = Random.Range(0, asteroids.Length - 1);
        GameObject asteroid = (GameObject)Instantiate(asteroids[index], location, Random.rotation);

        float xScale = Random.Range(minScale, maxScale);
        float yScale = Random.Range(minScale, maxScale);
        float zScale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = new Vector3(xScale, yScale, zScale);

        Vector3 spin = new Vector3(Random.Range(0, maxRotationSpeed), Random.Range(0, maxRotationSpeed), Random.Range(0, maxRotationSpeed));
        asteroid.GetComponent<Rigidbody>().AddRelativeTorque(spin);
        return asteroid;
    }

    public void spawnChildAsteroid(Vector3 location)
    {
        GameObject asteroid = spawnAsteroidAtLocation(location);
        AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();
        asteroidController.maxChildren = 0;
        asteroidController.isChild = true;
        asteroid.transform.localScale = new Vector3(
            asteroid.transform.localScale.x * .25f, 
            asteroid.transform.localScale.y * .25f, 
            asteroid.transform.localScale.z * .25f);
    }
}
