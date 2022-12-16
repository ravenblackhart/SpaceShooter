using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{
    public GameObject[] maskObj; 
    void Start()
    {
        for (int i = 0; i < maskObj.Length; i++)
        {
            maskObj[i].GetComponent<SpriteRenderer>().material.renderQueue = 3002; 
        }
    }

}
