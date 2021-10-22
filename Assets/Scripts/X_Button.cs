using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class X_Button : MonoBehaviour
{
    public GameObject rotator;

    [SerializeField] private float threshold= 0.1f;
    [SerializeField] private float deadZone = 0.025f;
 
    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint; 




    public UnityEvent onPressed, onReleased;
   
    void Start()
    {
        //basic button functionality
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();


    }

    // Update is called once per frame
    void Update()
    {
        if ( !_isPressed && GetValue() + threshold >= 1)
            Pressed();
        if (_isPressed && GetValue() - threshold <= 0)
            Released();
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Math.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);  
    }

    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        xGate();
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
    }

    private void xGate()
    { 
        
        //configures rotator game object
        rotator = GameObject.Find("Rotator");
        Transform rotate = rotator.transform;

        
        MyBloch.z-=180f;
        UnityEngine.Vector3 rotation = new Vector3(MyBloch.x, MyBloch.y, MyBloch.z); //vector to rotate
        

        rotator.transform.eulerAngles = (rotation);
        
    }
}
