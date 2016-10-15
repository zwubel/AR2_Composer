using UnityEngine;
using System;
using System.Net.Sockets;

public class Marker{
    private int ID;
    private float posX;
    private float posY;
    private float angle;

    public Marker(char ID, float posX, float posY, float angle){
        this.ID = ID;
        this.posX = posX;
        this.posY = posY;
        this.angle = angle;
    }

    public string toStr(int markerNumber){
        return "Marker " + markerNumber + "data:\n" +
            "\tID: " + this.ID + "\n" +
            "\tPosition: (" + this.posX + "/" + this.posY + ")\n" +
            "\tAngle: " + this.angle + "\n-----------------------";
    }
}

public class s_TCP : MonoBehaviour{
    internal Boolean socketReady = false;
    TcpClient mySocket;
    NetworkStream theStream;
    String Host = "192.168.0.5";
    Int32 Port = 10000;
    byte[] buffer;
    int bufferLength = 26;//3328; // 13 bytes * 256 markers (maximum)
    Marker[] markers;
    int maxMarkerCount = 2;//256;

    // Initialization
    void Start(){
        markers = new Marker[maxMarkerCount];
        setupSocket();
    }

    private void setupSocket(){
        try{
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream();
            socketReady = true;
            Debug.Log("Socket set up successfully.");
        }catch (Exception e){
            Debug.LogError("Socket setup failed. Error: " + e);
        }
    }

    // Is called once every frame
    void Update(){
        // Is the socket ready and does it have data waiting?
        if (socketReady && theStream.DataAvailable){
            Debug.Log("Socket is ready and stream data is available.");
            buffer = new byte[bufferLength];
            int bytesRead = theStream.Read(buffer, 0, bufferLength); // Read socket
            if (bytesRead == bufferLength) { // Number of bytes read equal to expected number?
                Debug.Log("bytesRead is equal to bufferLength.");
                for (int i=0; i<bufferLength; i += 13){ // 13 bytes per marker needed                    
                    int curID = System.BitConverter.ToInt32(buffer, i); // ID
                    if (curID.Equals('0')){ // End of frame reached?
                        Debug.Log("End of frame (last masker) reached, aborting.");
                        break;
                    }
                    float curPosX = System.BitConverter.ToSingle(buffer, i + 4); // X-position                    
                    float curPosY = System.BitConverter.ToSingle(buffer, i + 4); // Y-position
                    float curAngle = System.BitConverter.ToSingle(buffer, i + 4); // Angle
                    markers[i / 13] = new Marker(curID, curPosX, curPosY, curAngle); // Add new marker to array
                    Debug.Log(markers[i / 13].toStr(i / 13)); // Print debug message containing marker data
                }
            }else{
                Debug.Log("Number of bytes read from the stream not equal to buffer length!");
            }
        }
    }

    // Wrap up
    void OnApplicationQuit(){
        if (!socketReady)
            return;
        theStream.Close();
        mySocket.Close();
        socketReady = false;
    }
}