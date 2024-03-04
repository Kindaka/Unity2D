using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terr : MonoBehaviour
{
    private GameObject terrainObject;

    private float moveSpeed;
    public GameObject terrain;
    public GameObject column;
    public int oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        terrainObject = gameObject;
        column.SetActive(false);
        moveSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        terrainObject.transform.Translate(new Vector3(0, -1 * Time.deltaTime * moveSpeed, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetRandomPosition();
        column.SetActive(true);
        Invoke("DeactivateTerrain", 3f);
    }

    private void DeactivateTerrain()
    {
        terrain.SetActive(false);
    }

    private void SetRandomPosition()
    {
        float newX = Random.Range(-12f, 17f);
        float newY = Random.Range(5f, 7f);

        // Ensure the new position is not too close to the previous one
        while (Vector2.Distance(new Vector2(newX, newY), new Vector2(oldPosition, terrainObject.transform.position.y)) < 3f)
        {
            newX = Random.Range(-12f, 17f);
            newY = Random.Range(5f, 7f);
        }

        terrainObject.transform.position = new Vector3(newX, newY, 0);
        oldPosition = Mathf.RoundToInt(newX);
    }
}
