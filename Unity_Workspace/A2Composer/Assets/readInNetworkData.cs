using UnityEngine;
using System;
using System.Net.Sockets;

public class readInNetworkData : MonoBehaviour {
    Boolean socketReady = false;
    TcpClient mySocket;
    NetworkStream theStream;
    byte[] buffer;
    int bufferLength;
    Marker[] markers;
    long frameCounter = 0;
    bool markersSetForFrame = false;

    [Header("Socket Settings")]
    public String Host = "192.168.0.5";
    public Int32 Port = 10000;
    [Header("Data Stream Settings")]
    public int maxMarkerCount = 1;
    public int bytesPerMarker = 16;

    // Return markers array (used by setupScene.cs)
    public Marker[] getMarkers() {
        return markers;
    }

    // Have markers been set for current frame?
    public bool markersSet(){
        return markersSetForFrame;
    }

    // Initialization
    void Start(){
        bufferLength = bytesPerMarker * maxMarkerCount;
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
                for (int i = 0; i < bufferLength; i += bytesPerMarker){
                    int curID = System.BitConverter.ToInt32(buffer, i); // ID
                    if (curID == -1){ // End of frame reached?
                        Debug.Log("Last masker reached, suspending loop for current frame " + frameCounter + ".");
                        frameCounter++;
                        markers[i / bytesPerMarker + 1].setID(-1);
                        break;
                    }else if (transform.name.Equals("Marker" + i / bytesPerMarker)){
                        float curPosX = System.BitConverter.ToSingle(buffer, i + 4); // X-position
                        float curPosY = System.BitConverter.ToSingle(buffer, i + 8); // Y-position
                        float curAngle = System.BitConverter.ToSingle(buffer, i + 12); // Angle
                        markers[i / bytesPerMarker] = new Marker(curID, curPosX, curPosY, curAngle); // Add new marker to array
                        //Debug.Log(markers[i / 16].toStr(i / 16)); // Print debug message containing marker data

                        // For testing only
                        //string markerStr =  "Marker " + i/bytesPerMarker + " data:\n" +
                        //                    "\tID: " + curID + "\n" +
                        //                    "\tPosition: (" + curPosX + "/" + curPosY + ")\n" +
                        //                    "\tAngle: " + curAngle + "\n-----------------------";
                        //Debug.Log(markerStr);

                        //// Transforming the game object (cube)
                        //transform.Translate(curPosX, curPosY, 0);
                        //transform.Rotate(transform.up, curAngle);
                    }
                }
                markersSetForFrame = true;
            }else{
                Debug.Log("Number of bytes read from stream not equal to buffer length!");
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