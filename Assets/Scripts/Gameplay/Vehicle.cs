using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    void Awake()
    {
     
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
    }

    void Start()
    {
    
        transform.eulerAngles = new Vector3(0f, -90f, 0f);
    }
}