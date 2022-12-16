using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour
{
    void Start()
    {
        while (true)
        {
            transform.position = new Vector3(Random.Range(-7f, 7f), 0.875f, Random.Range(-7f, 7f));
            if (transform.position.z * transform.position.z + transform.position.x * transform.position.x > 25)
                break;
        }
    }
}
