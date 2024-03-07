using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terr : MonoBehaviour
{
    private GameObject terrObject;
    private bool firstMove;

    private float moveSpeed;
    public GameObject terrain1;
    public GameObject terrain2;
    public GameObject colum;

    private float minDistance = 3f;
    private float desiredDistance = 2f;
    private float probability = 0.5f;
    private List<Vector3> spawnedPositions = new List<Vector3>();
    private float gapBetweenPlatforms = 2f;

    void Start()
    {
        terrObject = this.gameObject;
        terrain2.SetActive(false);
        colum.SetActive(false);
        moveSpeed = 5f;
        firstMove = true;
        Invoke("ReactiveTerrain", 10f);
    }

    void Update()
    {
        terrObject.transform.Translate(new Vector3(0, -1 * Time.deltaTime * moveSpeed, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AdjustPosition();
        CheckForCollisions();

        if (firstMove)
        {
            Invoke("DeactivateTerrain", 3f);
            firstMove = false;
        }
    }

    private void AdjustPosition()
    {
        float randomPositionX;
        float randomPositionY;

        int maxAttempts = 100;
        int currentAttempt = 0;

        do
        {
            randomPositionX = Random.Range(-6f, 9f) * gapBetweenPlatforms;
            randomPositionY = Random.Range(7f, 8f);

            if (Random.value < probability)
            {
                float sign = (Random.Range(0, 2) == 0) ? -1f : 1f;
                randomPositionX += sign * desiredDistance;
            }

            currentAttempt++;
        }
        while (!IsPositionValid(randomPositionX, 7f) && currentAttempt < maxAttempts);

        terrObject.transform.position = new Vector3(randomPositionX, randomPositionY, 0f);
        spawnedPositions.Add(terrObject.transform.position);
    }

    private bool IsPositionValid(float x, float y)
    {
        foreach (var pos in spawnedPositions)
        {
            float distance = Vector2.Distance(new Vector2(x, y), new Vector2(pos.x, pos.y));

            // Check distance between objects
            if (distance < minDistance)
            {
                return false;
            }
        }

        return true;
    }



    private void CheckForCollisions()
    {
        GameObject[] terrObjects = GameObject.FindGameObjectsWithTag("Ter");

        foreach (var terrObj in terrObjects)
        {
            foreach (var pos in spawnedPositions)
            {
                float distance = Vector3.Distance(pos, terrObj.transform.position);

                // Check distance between objects
                if (distance < minDistance)
                {
                    AdjustPosition(); // Adjust position if too close to another object
                    return; // Exit the method after adjusting the position
                }
            }
        }
    }

    private void DeactivateTerrain()
    {
        terrain1.SetActive(false);
        colum.SetActive(true);
    }

    private void ReactiveTerrain()
    {
        terrain2.SetActive(true);
        Invoke("SpawnRandom", 3f);
    }

    private void SpawnRandom()
    {
        terrain2.transform.position = new Vector3(Random.Range(-10f, 10f), 0f, 0f);
        Invoke("ReactiveTerrain", 10f);
    }
}
