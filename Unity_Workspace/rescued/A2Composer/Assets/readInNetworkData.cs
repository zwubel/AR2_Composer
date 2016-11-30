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
    bool oneMarkerSet = false;
    setupScene setupScene;

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

    // Initialization
    void Start(){
        bufferLength = bytesPerMarker * maxMarkerCount + 4;
        markers = new Marker[maxMarkerCount + 1];
        setupScene = gameObject.GetComponent<setupScene>();
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
        setupScene.setMarkerArraySet(false);
        oneMarkerSet = false;
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
                        markers[i / bytesPerMarker + 1] = new Marker(-1, 0.0f, 0.0f, 0.0f);
                        break;
                    }else if(curID < 0){
                        Debug.LogError("Marker ID not valid: " + curID);
                    }else { // if (curID == i / bytesPerMarker + 1){
                        float curPosX = System.BitConverter.ToSingle(buffer, i + 4); // X-position
                        float curPosY = System.BitConverter.ToSingle(buffer, i + 8); // Y-position
                        float curAngle = System.BitConverter.ToSingle(buffer, i + 12); // Angle
                        markers[i / bytesPerMarker] = new Marker(curID, curPosX, curPosY, curAngle); // Add new marker to array
                        oneMarkerSet = true;
                        Debug.Log(markers[i / bytesPerMarker].toStr()); // Print debug message containing marker data
                    }
                }
                if(oneMarkerSet)
                    setupScene.setMarkerArraySet(true);
            }
            else{
                Debug.LogError("Number of bytes read from stream NOT equal to buffer length!");
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