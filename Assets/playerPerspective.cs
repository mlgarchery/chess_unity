using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
public class playerPerspective : MonoBehaviour
{
    // Start is called before the first frame update
    // public String playerName;
    // public String server = "192.168.0.25";
    // public String server = "ec2-18-212-170-213.compute-1.amazonaws.com";
    public string server = "18.212.170.213";
    public int port = 50000;

    TcpClient tcp_socket;
    NetworkStream net_stream;
    StreamWriter socket_writer;
    StreamReader socket_reader;

    public String playerName;

    internal String input_buffer = "";

    int deadZoneLastIndex=0;


    void Awake(){
        try{
            tcp_socket = new TcpClient(server, port);

        }catch(SocketException socketException){
            Debug.Log("Erreur !" + socketException);
        }
        net_stream = tcp_socket.GetStream();
        socket_reader = new StreamReader(net_stream);
        socket_writer = new StreamWriter(net_stream);
    }

    
    void Start()
    {
        playerName = readLineSocket(); // get the playerName (first response from the server)
        if(playerName=="Player2"){ // playing with the dark pieces
            Quaternion rotation = Quaternion.AngleAxis(180, Vector3.up);
            this.transform.rotation = rotation;
        }
    }

    public String readLineSocket()
    {
        if (net_stream.DataAvailable)
            return socket_reader.ReadLine();
        return "";
    }

    public char[] readBytesSocket(){
        if(net_stream.DataAvailable){
            char[] buffer = {'a', 'a', 'a', 'a', 'a'};
            socket_reader.ReadBlock(buffer, 0, 5);
            return buffer;
        }

        return new char[]{};
    }
    
    public void writeRequest(List<int> request){
        // ints should be in 0..7 (chess board limit)
        foreach(int i in request){
            socket_writer.Write(i);
        }
        socket_writer.Write('\n');
        socket_writer.Flush();
    }
    
    
    public void writeStringToSocket(String line)
    {
        line = line + "\n";
        socket_writer.Write(line);
        socket_writer.Flush();
    }

    // public void writeByteToSocket(List<int> bytes){
    //     socket_writer.Write(bytes);
    //     socket_writer.Flush();
    // }

    // Update is called once per frame
    void Update()
    {
        // string key_stroke = Input.inputString;
        // // Collects keystrokes into a buffer
        // if (key_stroke != ""){
        //     if(key_stroke != " "){
        //         input_buffer += key_stroke;
        //     }

        //     if (key_stroke == " "){
        //     	// Send the buffer, clean it
        //     	Debug.Log("Sending: " + input_buffer);
        //     	// writeStringToSocket(input_buffer);
        //         List<int> request = new List<int>{7,1,2,4,5};
        //         writeRequest(request);
        //         // writeByteToSocket(new List<int>{1,2});
        //     	input_buffer = "";
        //     }

        // }

        // String received_data = readLineSocket();
        String received_data = readLineSocket();
        if (received_data.Length!=0)
        {
        	// Do something with the received data,
            Debug.Log("received network data ! Lenght:" + received_data.Length.ToString());
            
            //convert chars to int
            List<int> requestEnd = convertToInt(skipOne(received_data));
            char playerNumero = received_data[0];

            String playerMoving = "Player"+playerNumero;
            char oppositeNumero;
            if(playerNumero=='1'){
                oppositeNumero='2';
            }else{ oppositeNumero='1';}
            String oppositePlayer = "Player"+oppositeNumero;


            // on vérifie si le joueur adverse du playerMoving
            // a un pion sur la case finale
            int oPieceIndex = getPlayerPieceIndexAt(7-requestEnd[2], 7-requestEnd[3], oppositePlayer);
            if(oPieceIndex!=-1){
                // si oui, on la passe à la dead zone
                Transform oPiece = GameObject.Find(oppositePlayer).transform.GetChild(oPieceIndex);
                moveToDeadZone(oPiece, oppositePlayer);
            }

            // puis on bouge la piece du playerMoving
            int pieceIndex = getPlayerPieceIndexAt(requestEnd[0], requestEnd[1], playerMoving);
            Transform piece = GameObject.Find(playerMoving).transform.GetChild(pieceIndex);
            // changing the state : (in player)
            GameObject.Find(playerMoving).GetComponent<player>().movePieceFromTo(requestEnd[0], requestEnd[1], requestEnd[2], requestEnd[3]);
            // moving the game object
            piece.localPosition = new Vector3(requestEnd[2], 0, requestEnd[3]);
        }
    }

    public void moveToDeadZone(Transform pieceTransform, string playerName){
        GameObject.Find(playerName).GetComponent<player>().deletePieceAt((int)pieceTransform.localPosition.x, (int)pieceTransform.localPosition.z); // change the value in the pieceAtPosition to -1 (no more piece on this case)
        // pieceTransform.parent = gameObject.transform; // piece in the local piece of the deadzone now
        pieceTransform.localPosition = new Vector3(10+(int)(deadZoneLastIndex/8), 0, deadZoneLastIndex%8);
        
        // DEACTIVATE its component! I don't know how to do it for the moment
        Component[] components = pieceTransform.gameObject.GetComponents<Component>();
        // find the script component in the array of component
        // USE .destroyScript()
        deadZoneLastIndex = deadZoneLastIndex + 1;
    }
    int getPlayerPieceIndexAt(int x, int z, String playerName){
        int[,] pieceAtPosition = GameObject.Find(playerName).GetComponent<player>().pieceAtPosition;
        int index = pieceAtPosition[x, z];
        return index;
    }
    char[] skipOne(String chaine){
        char[] newChaine= {'a', 'a', 'a', 'a'};
        for(int i=0;i<chaine.Length-1;i++){
            newChaine[i]=chaine[i+1];
        }
        return newChaine;
    }
    List<int> convertToInt(char[] chaine){
        List<int> list = new List<int>{-1,-1,-1,-1};
        for(int i=0;i<chaine.Length;i++){
            list[i] = (int)(chaine[i])-48;
        }
        return list;
    }
}
