using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    private Vector3 oldPosition;
    private GameObject obj;

    public float moveSpeed;
    public float moveRange;

    // Start is called before the first frame update
    void Start()
    {
        obj = gameObject;
        oldPosition = obj.transform.position;

        moveSpeed = 2f;
        moveRange = 7f;
        
    }

    // Update is called once per frame
    void Update()
    {
        obj.transform.Translate(new Vector3(0, 1 * Time.deltaTime * moveSpeed, 0));

        if (Vector3.Distance(oldPosition, obj.transform.position) > moveRange)
        {
            obj.transform.position = oldPosition;
        }
    }
}
