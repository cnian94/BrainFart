using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioScript : MonoBehaviour
{
    private float pixelsToUnits;


    void Awake()
    {
        pixelsToUnits = 1;
        gameObject.GetComponent<Camera>().orthographicSize = Screen.height / pixelsToUnits / 2;
        Vector3 pos = gameObject.GetComponent<Camera>().transform.position;
        pos.x = Screen.width / 2;
        pos.y = Screen.height / 2;
        gameObject.GetComponent<Camera>().transform.position = pos;
    }

}
