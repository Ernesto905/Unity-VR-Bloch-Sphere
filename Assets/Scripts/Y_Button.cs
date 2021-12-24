using UnityEngine;
using UnityEngine.Events;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
public class Y_Button : MonoBehaviour
{
    public GameObject rotator;
    [SerializeField] private float threshold= 0.1f;
    [SerializeField] private float deadZone = 0.025f;
 
    private bool _isPressed;
    private UnityEngine.Vector3 _startPos;
    private ConfigurableJoint _joint; 
    public UnityEvent onPressed, onReleased;
    private string gate; 

     
    

    Vector3 receivedPos = Vector3.zero;

    void Start()
    {
        //basic button functionality
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();

    }
    

   

    void sendConfirmation()
    {

        //---Establish network details----
        NetworkStream nwStream = X_Button.client.GetStream();
        byte[] buffer = new byte[X_Button.client.ReceiveBufferSize];

        //---Send Confirmation---
        byte[] myWriteBuffer = Encoding.ASCII.GetBytes(gate); //Converting string to byte data
        nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);

        recieveConfirmation();        

    }

    void recieveConfirmation()
    {
        Debug.Log("Made it here");
        //---Establish network details----
        NetworkStream nwStream = X_Button.client.GetStream();
        byte[] buffer = new byte[X_Button.client.ReceiveBufferSize];

        Debug.Log("Made it here2");
        // //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, X_Button.client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

        Debug.Log(dataReceived);
    }
    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
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
        var value = UnityEngine.Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Math.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);  
    }

    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        yGate();
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
    }

    private void yGate()
    { 
        gate = "YGate";
        sendConfirmation();
        
       

        //Rotates Bloch Arrow 
        Quaternion rotationAmt = Quaternion.Euler(receivedPos);
        rotator = GameObject.Find("Rotator");
        rotator.transform.localRotation = rotationAmt;
              
        
    }

    

    
}
