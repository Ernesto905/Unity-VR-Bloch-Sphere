using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.UI;

using System;

public class MyBloch : MonoBehaviour
{
    public Vector3 BSphereStartPos;
    UnityEngine.Quaternion defaultRotation;
    public GameObject rotator;



    

    public static float y=0f;
    public static float x=0f;
    public static float z=0f;

   




    
    public GameObject myBloch;  
    // Start is called before the first frame update
    void Start()        
    {   
        //used to determine starting pos of sphere
        BSphereStartPos =  transform.localPosition;
    }

    void Update()
    {

        //rotateRot(thetaInDeg);                            THIS WILL BE PLACEMENT OF SOCKKET VARIABLE   

    }

    
    //used for maintaining current orientation on entire Bsphere
    void Awake()
    {
        defaultRotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = defaultRotation;
    }

    
    void rotateRot(int t)
    {
        rotator.transform.eulerAngles = new Vector3(0,0,t);
    }

    
}
