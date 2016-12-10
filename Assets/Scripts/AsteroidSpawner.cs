using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour {

    public GameObject[] asteroids;
    public int maxAstroids;
    public int maxRotationSpeed;
    public int maxScale;

    public static int minDistance = 750;
    public static int maxDistance = 1000;

    private Transform player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnAsteroid(maxAstroids, 20, maxDistance - 50);
    }
    
    // Update is called once per frame
    void Update () {
        int numAsteroids = GameObject.FindGameObjectsWithTag("Asteroid").Length;
        if (numAsteroids < maxAstroids)
        {
            spawnAsteroid(maxAstroids - numAsteroids, minDistance, maxDistance - 50);
        }
    }

    private void spawnAsteroid(int number, int minRange, int maxRange)
    {
        for (int i = 0; i < number; i++)
        {
            int index = Random.Range(0, asteroids.Length - 1);

            // calculate a random position around the player
            float angle = Random.Range(0.0f, Mathf.PI * 2);
            float height = Random.Range(-maxRange, maxRange);
            Vector3 circle = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            circle *= Random.Range(minRange, maxRange);
            circle += player.position;
            circle.y += height;

            GameObject asteroid = (GameObject)Instantiate(asteroids[index], circle, Random.rotation);

            float xScale = Random.Range(1, maxScale);
            float yScale = Random.Range(1, maxScale);
            float zScale = Random.Range(1, maxScale);
            asteroid.transform.localScale = new Vector3(xScale, yScale, zScale);

            Vector3 spin = new Vector3(Random.Range(0, maxRotationSpeed), Random.Range(0, maxRotationSpeed), Random.Range(0, maxRotationSpeed));
            asteroid.GetComponent<Rigidbody>().AddRelativeTorque(spin);
        }
    }
}
