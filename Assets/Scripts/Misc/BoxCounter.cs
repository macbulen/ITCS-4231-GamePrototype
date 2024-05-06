using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCounter : MonoBehaviour
{
    [SerializeField] OnScreenCounter _counter;

    private void Update()
    {
     //isBoxClose();   
    }

    private void isBoxClose()
    {
        // GameObject player = GameObject.Find("Red");
        // Vector3 direction = player.transform.position - transform.position;
        // float distance = direction.magnitude;

        //     if (distance <= 2f) 
        //     { 
        //         _counter.Counter++;
        //         return; 
        //     }
    }
}
