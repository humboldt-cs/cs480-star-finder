using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(new Vector3(0, 0, 0), Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
