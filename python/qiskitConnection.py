from qiskit import QuantumCircuit, execute, Aer
from math import sqrt, pi
import numpy as np
import socket

#initialize socket
host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host,port))

#backend simulator setup
backend = Aer.get_backend('statevector_simulator') 

#initialize qbit
currentState = [0,0,0] 
qc = QuantumCircuit(1)

#takes in the alpha phase state, converts to radian, and returns it as degrees
def toTheta(alpha):
    radians = 2 * np.arccos(alpha)
    degrees = radians * 180 / pi
    return degrees

#return phase states in numpy array 
def retState(qCirc):
    result = execute(qCirc,backend).result().get_statevector()
    return result
    

inp = ''
while True:
    
    recievedData = sock.recv(1024).decode("UTF-8") 
    
    if recievedData == "XGate":
        print('XGATE has been pressed')
        
        qc.x(0)
        qstate = retState(qc)
        theta = toTheta(qstate[0])
        
        #alter the Vector according to the new value of theta
        currentState[0] = int(theta.real)
        
        posString = ','.join(map(str, currentState))
        print(f"the sent string is {posString}") #used for testing
        
        
        sock.sendall(posString.encode("UTF-8"))
        
    elif recievedData == "YGate":
        print('XGATE has been pressed')
        
        qc.h(0)
        qstate = retState(qc)
        theta = toTheta(qstate[0])
        
        #alter the Vector according to the new value of theta
        currentState[0] = int(theta.real)
        
        posString = ','.join(map(str, currentState))
        print(f"the sent string is {posString}") #used for testing
        
        
        sock.sendall(posString.encode("UTF-8"))
        
        
        
    else: print(recievedData)
    
    
    
    # #gate decision
    # inp = input('Enter q input: ')
    # if inp == 'x': qc.x(0)
    # if inp == 'h': qc.h(0)
    # if inp == 'm': pass

    # #get current state of theta
    # qstate = retState(qc)
    # theta = toTheta(qstate[0])
    
    # #alter the Vector according to the new value of theta
    # currentState[0] = int(theta.real)
    
    # #string manipulation (For socket communication)
    # posString = ','.join(map(str, currentState))
    # print(f"the sent string is {posString}") #used for testing
    
    # #send socket 
    # sock.sendall(posString.encode("UTF-8"))  
    


