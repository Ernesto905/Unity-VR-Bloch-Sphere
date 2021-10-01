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
     
    //public BSphereStartRot : Quaternion;

    private Complex alpha = Complex.Zero;
    private Complex beta = Complex.Zero;
    private Complex theta = Complex.Zero;   
    private Complex phi = Complex.Zero;

    

    
    public GameObject myBloch;  
    // Start is called before the first frame update
    void Start()        
    {   
        //used to determine starting pos of sphere
        BSphereStartPos =  transform.localPosition;
        Debug.Log( "Inside BS: current bloch location is" + BSphereStartPos);
        
    }

    // Update is called once per frame
    void Update()
    {
        DateTime currentTime = DateTime.Now;

        float secondsDegree = -(currentTime.Second / 60f) * 360f;
        //Debug.Log(secondsDegree);
        myBloch.transform.localRotation = UnityEngine.Quaternion.Euler(new Vector3(0,0,secondsDegree));
    
        
    }
}
