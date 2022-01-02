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


#takes in the alpha phase state. Normalize it, and returns it as degrees
def toTheta(a):
    normalized_alpha = np.sqrt((a.real **2) + (a.imag ** 2))

    angle = 2 * np.arccos(normalized_alpha)
    
    return np.degrees(angle)

#takes in complex beta and angle theta in degrees. Derives normalized phi, then returns it in degrees
def toPhi(t, b):
    t = np.radians(t)

    angle = (np.log(b / np.sin(t/2))) / 1j if b != 0 else 0
    normalized_angle = np.sqrt((angle.real ** 2) + (angle.imag ** 2))
    
    return np.degrees(normalized_angle)


while True:
    recievedData = sock.recv(1024).decode("UTF-8") 
    

    if recievedData == "XGate":
        lastGate = "X Gate"
        qc.x(0)
        
    elif recievedData == "hGate":
        lastGate = "H Gate"
        qc.h(0)
    
    elif recievedData == "yGate":
        lastGate = "Y Gate"
        qc.y(0)
        
    elif recievedData == "zGate":
        lastGate = "Z Gate"
        qc.z(0)
        
    else: 
        raise Exception(f"Error: Recieved data unrecognized: {recievedData}")
    
    #qubit information to readable statevector
    sv = Statevector(qc)
    qstate = sv.data
    
    #Qubit properties 
    alpha = qstate[0]
    beta = qstate[1]
    theta = toTheta(alpha)
    phi = toPhi(theta, beta)
    sv = Statevector(qc)

    
    
    #alter the Vector according to the new value of theta
    currentState[0] = int(theta)
    currentState[1] = int(phi)
    
    posString = ','.join(map(str, currentState))
    
    print(f"Sent state is: {posString} and the sent gate is {lastGate}")
    sock.sendall(posString.encode("UTF-8"))

    


