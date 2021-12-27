from qiskit import QuantumCircuit
from qiskit.quantum_info import Statevector
from math import sqrt, pi
import numpy as np
import socket

#initialize socket
host, port = "127.0.0.1", 25001
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((host,port))

#initialize qbit
currentState = [0,0,0] 
qc = QuantumCircuit(1)

#takes in the alpha phase state, converts to radian, and returns it as degrees
def toTheta(a):
    angle = 2 * np.arccos(a)
    return np.degrees(angle.real)

#takes in complex beta and angle theta in degrees and returns phi in degrees
def toPhi(t, b):
    t = np.radians(t)
    angle = (np.log(b / np.sin(t/2))) / 1j
    
    #deal with NaN
    real = 0 if np.isnan(angle.real) else angle.real
    
    return np.degrees(real)

while True:
    
    recievedData = sock.recv(1024).decode("UTF-8") 
    
    #qubit information to readable statevector
    sv = Statevector(qc)
    qstate = sv.data

    if recievedData == "XGate":
        print('XGATE has been pressed')
        qc.x(0)
        
    elif recievedData == "hGate":
        print('HGATE has been pressed')
        qc.h(0)
    
    elif recievedData == "yGate":
        print('YGATE has been pressed')
        
        qc.y(qubit)(0)
        
        
        
    else: 
        raise Exception(f"Error: Recieved data unrecognized: {recievedData}")
    
    #Qubit properties 
    alpha = qstate[0]
    beta = qstate[1]
    theta = toTheta(alpha)
    phi = toPhi(theta, beta)
    
    
    #alter the Vector according to the new value of theta
    currentState[0] = int(theta)
    currentState[1] = int(phi)
    
    posString = ','.join(map(str, currentState))
    print(f"the sent string is {posString}") #used for testing
    
    
    sock.sendall(posString.encode("UTF-8"))
    


