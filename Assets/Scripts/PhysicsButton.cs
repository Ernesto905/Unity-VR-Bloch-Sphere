using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PhysicsButton : MonoBehaviour
{
    public GameObject bSphere;

    [SerializeField] private float threshold= .1f;
    [SerializeField] private float deadzone = 0.025f;
 
    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint; 



    public UnityEvent onPressed, onReleased;
    // Start is called before the first frame update
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

        if (Math.Abs(value) < deadzone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);  
    }

    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        Debug.Log("pressed!");
        resetBloch();
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        Debug.Log("Released!");
    }

    private void resetBloch()
    { 
        //configures blochsphere game object's initial position && current position
        bSphere = GameObject.Find("BlochSphere");
        Transform blochTransform = bSphere.transform; 
        Vector3 curBlochPos = blochTransform.position;
        Vector3 intiialPos = new Vector3(1.0615f, 1.224f, -0.0152725f); //predefined initial pos for Bloch

        if (curBlochPos != intiialPos) //reset position if necessary
        {
            bSphere.transform.position = intiialPos;
        }
            

        Debug.Log("This is inside phys button: " + curBlochPos);
    }
}
