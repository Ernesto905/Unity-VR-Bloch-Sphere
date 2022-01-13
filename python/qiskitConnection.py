from qiskit import QuantumCircuit    
from qiskit.quantum_info import Statevector
import numpy as np
import socket

    

def initializeSocket():
    host, port = "127.0.0.1", 25001
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host,port))
    return sock

def initializeCircuit():
    currentState = [0,0,0] 
    qc = QuantumCircuit(1)
    return qc, currentState


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


def connectToUnity(sock, qc, currentState):
    
    while True:

        collapsed = False
        recievedData = sock.recv(1024).decode("UTF-8") 

        if recievedData == "XGate":
            qc.x(0)
            
        elif recievedData == "hGate":
            qc.h(0)
        
        elif recievedData == "yGate":
            qc.y(0)
            
        elif recievedData == "zGate":
            qc.z(0)
        elif recievedData == "Measurement":
            collapsed = True
            
        else: 
            raise Exception(f"Error: Recieved data unrecognized: {recievedData}")
        
        
        #get circuit as a readable statevector
        sv = Statevector(qc)

        #Measure in 0/1 basis 
        if collapsed:
            sv = sv.measure()[1]
            if sv.data[0] == (0. + 0.j) and sv.data[1] == (1.+0.j):
                print("Changed to ket 1 state")
                qc.initialize('1')
            elif sv.data[1] == (0. + 0.j) and sv.data[0] == (1.+0.j):
                print("Changed to ket 0 state")
                qc.initialize('0')
        
        #Establish qubit properties 
        qstate = sv.data
        alpha = qstate[0]
        beta = qstate[1]
        theta = int(toTheta(alpha))
        phi = int(toPhi(theta, beta))
        
        
        #alter the vector according to the new value of theta and phi
        currentState[0] = theta
        currentState[1] = phi
        
        #reset qubit's phase if in |0> state
        if sv[0] != (1 + 0j) and theta == 0:
            print('State reset')
            qc = QuantumCircuit(1)
            sv = Statevector(qc)
            currentState = [0,0,0]

             
        print(f"Sent theta is {theta} and sent phi is {phi} and current statevector is {sv.data}")
        posString = ','.join(map(str, currentState))
        sock.sendall(posString.encode("UTF-8"))



def main():
    sock = initializeSocket()

    circuit, currentState = initializeCircuit()

    connectToUnity(sock, circuit, currentState)

    
if __name__ == '__main__':
    main()

