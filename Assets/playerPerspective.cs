using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using UnityEngine.UI;
public class playerPerspective : MonoBehaviour
{
    // Start is called before the first frame update
    // public String playerName;
    // private String server = "192.168.0.25";
    // public String server = "ec2-18-212-170-213.compute-1.amazonaws.com";
    // private String server = "18.212.170.213";



    TcpClient tcp_socket;
    NetworkStream net_stream;
    StreamWriter socket_writer;
    StreamReader socket_reader;

    public String playerName;

    int deadZoneLastIndex=0;

    int pawnIndex = -1;

    bool isGameLaunched = false;

    void Awake(){
    }

    
    void Start()
    {
        pauseGame();        
    }


    public String getServerField(){
        InputField serverField = GameObject.Find("ServerField").GetComponent<InputField>();
        String serverIp;
        if(serverField.text==""){
            serverIp = serverField.placeholder.GetComponent<Text>().text;
        }else{
            serverIp = serverField.text;
        }
        return serverIp;
    }

    public int getPortField(){
        InputField portField = GameObject.Find("PortField").GetComponent<InputField>();
        String portText;
        if(portField.text==""){
            portText = portField.placeholder.GetComponent<Text>().text;
        }else{
            portText = portField.text;
        }
        int port = int.Parse(portText);
        return port;
    }

    public String ConnectToGame(String server, int port){
        try{
            tcp_socket = new TcpClient(server, port);
            net_stream = tcp_socket.GetStream();
            socket_reader = new StreamReader(net_stream);
            socket_writer = new StreamWriter(net_stream);
            return "Connected!";
        }
        catch(SocketException e){
            Debug.Log(e);
            return "Unable to connect";
        }
    }

    public void connectAndLaunch(){
        if(tcp_socket!=null) return;
        String resultConnection = ConnectToGame(getServerField(), getPortField());

        Text connectionInfoLabel = GameObject.Find("ConnectionInfoLabel").GetComponent<Text>();
        if(resultConnection!="Connected!"){
            connectionInfoLabel.color = Color.red;
        }else{
            connectionInfoLabel.color = Color.white;
        }
        connectionInfoLabel.text = resultConnection;

        if(resultConnection=="Connected!"){
            // delay
            delayLaunch();
            // launchGame();
        }else{
            pauseGame();
        }
    }
    void delayLaunch(){
        Invoke("launchGame",1);
    }
    public void pauseGame(){
        GameObject.Find("Player1").GetComponent<player>().disableScripts();
        GameObject.Find("Player2").GetComponent<player>().disableScripts();
        isGameLaunched = false;
    }

    public void launchGame(){
        playerName = readLineSocket(); // get the playerName (first response from the server)
        if(playerName=="Player2"){ // playing with the dark pieces
            Quaternion rotation = Quaternion.AngleAxis(180, Vector3.up);
            this.transform.rotation = rotation;
        }

        isGameLaunched = true;
        GameObject.Find("Player1").GetComponent<player>().enableScripts();
        GameObject.Find("Player2").GetComponent<player>().enableScripts();
        GameObject.Find("NetworkMenu").SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(!isGameLaunched) return;
        // String received_data = readLineSocket();
        String received_data = readLineSocket();
        if (received_data.Length!=0)
        {
            Debug.Log(received_data);
        	// Do something with the received data,
            // Debug.Log("received network data ! Lenght:" + received_data.Length.ToString());
            int[] requestEnd = convertToInt(received_data.Substring(1, received_data.Length-1));

            if(received_data[0]=='3'){
                // transform pawn into something else
                transformPawn(requestEnd);
            }
            else{

                char playerNumero = received_data[0];
                //convert chars to int

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

                int pieceIndex = getPlayerPieceIndexAt(requestEnd[0], requestEnd[1], playerMoving);
                // puis on bouge la piece du playerMoving
                movePiece(pieceIndex, playerMoving, requestEnd);

                // transform pawn to something else if he reaches the last row!
                if(requestEnd.Length>=5){
                    GameObject.Find(playerMoving).GetComponent<player>().disableScripts();
                    
                    if(playerMoving==playerName){ // on affiche le GUI pour choisir la pièce
                        pawnIndex = pieceIndex;
                        GameObject.Find("Menu").transform.GetChild(0).gameObject.SetActive(true); // PawnMenu active
                        
                    }
                }

                
            }
            
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
    
    public void sendPawnTransformRequest(String pieceType){
        Dictionary<String, int> typeIndex = new Dictionary<string, int>(){
            {"Rook", 0},
            {"Knight", 1},
            {"Bishop", 2},
            {"Queen", playerName=="Player1" ? 3 : 4}, // queen is not at the same index
            // depending on if its white or black
        };

        List<int> request = new List<int>{
            3, // indication a transform request (not a player name)
            playerName=="Player1" ? 1 : 2, // player numero
            pawnIndex-8, // pawnIndex in the player hierarchy (-8 so we only have a one digit number)
            typeIndex[pieceType] // number designating the piece we want the pawn to be transformed into
        };
        writeRequest(request);
    }

    public void transformPawn(int[] request){
        String playerString = "Player" + request[0].ToString();
        GameObject player = GameObject.Find(playerString);
        GameObject piece = Instantiate(player.transform.GetChild(request[2]).gameObject, player.transform);
        
        Transform pawnTransform = player.transform.GetChild(request[1]+8); // we get the original index by adding 8
        piece.transform.localPosition = new Vector3(pawnTransform.localPosition.x, 0, pawnTransform.localPosition.z);

        // remove the GameObject from the game
        Destroy(pawnTransform.gameObject); 
        piece.transform.SetSiblingIndex(request[1]+8); // set piece at the index of the old pawn gameobject
        // deactivate the GUI menu
        GameObject.Find("Menu").transform.GetChild(0).gameObject.SetActive(false);
        // reactivate the pieces scripts for the player playing this move:
        player.GetComponent<player>().enableScripts();
    }

    public bool isAPawn(int pieceIndex, String playerName){
        Transform pieceTransform = GameObject.Find(playerName).transform.GetChild(pieceIndex);
        String name = pieceTransform.gameObject.name;
        if(name.Length>=4 && name.Substring(0,4)=="Pawn"){
            return true;
        }
        return false;
    }


    void movePiece(int pieceIndex, String playerMoving, int[] requestEnd){
        
        Transform piece = GameObject.Find(playerMoving).transform.GetChild(pieceIndex);
        // changing the state : (in player)
        GameObject.Find(playerMoving).GetComponent<player>().movePieceFromTo(requestEnd[0], requestEnd[1], requestEnd[2], requestEnd[3]);
        // moving the game object
        piece.localPosition = new Vector3(requestEnd[2], 0, requestEnd[3]);
    }

    void moveToDeadZone(Transform pieceTransform, string playerName){
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

    int[] convertToInt(string chaine){
        int[] arr = new int[chaine.Length];
        for(int i=0;i<chaine.Length;i++){
            arr[i] = (int)(chaine[i]-48);
        }
        return arr;
    }

    // void deactivatePlayer(String playerName){
    //     GameObject.Find(playerName).SetActive(false);
    // }
}
