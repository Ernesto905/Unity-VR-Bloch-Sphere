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

    [SerializeField] private Complex waveFunction = Complex.Zero;
    [SerializeField] private Complex euler = Complex.Zero;
    [SerializeField] private float theta = 0;
    [SerializeField] private float phi = 0;

    public static float y=0f;
    public static float x=0f;
    public static float z=0f;



    
    public GameObject myBloch;  
    // Start is called before the first frame update
    void Start()        
    {   
        //used to determine starting pos of sphere
        BSphereStartPos =  transform.localPosition;
        Debug.Log(theta);
       
    }

    // Update is called once per frame
    void Update()
    {
        DateTime currentTime = DateTime.Now;
        float secondsDegree = -(currentTime.Second / 60f) * 360f;
        
        //test quaternion 90 degrees Y axis
        //UnityEngine.Quaternion rotation = UnityEngine.Quaternion.Euler();         not currently being used
    
        //Perform roation on sphere (Time Dependent)
        rotator = GameObject.Find("Rotator");
        //rotator.transform.localRotation = UnityEngine.Quaternion.Euler(new Vector3(0,0,secondsDegree));


        //rotator.transform.localRotation = UnityEngine.Quaternion.Euler(rotationVector);
    
        //Debug.Log("current orientation is: " + rotationVector);
        

    }

    void calcWF()
    {

        waveFunction = new Complex(Mathf.Cos(theta / 2), (Mathf.Sin(theta / 2)));

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

}
