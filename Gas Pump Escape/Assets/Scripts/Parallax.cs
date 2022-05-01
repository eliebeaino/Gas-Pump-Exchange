using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float lenght;
    float startPos;

    public GameObject camera;
    [SerializeField] float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startPos = camera.transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (camera.transform.position.x * (1 - scrollSpeed));
        float distance = (camera.transform.position.x * scrollSpeed);

        transform.position = new Vector3( startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + lenght) startPos += lenght;
        else if (temp < startPos - lenght) startPos -= lenght;
    }
}
