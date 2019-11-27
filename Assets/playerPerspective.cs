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
    private String server = "192.168.0.25";
    // public String server = "ec2-18-212-170-213.compute-1.amazonaws.com";
    // private String server = "18.212.170.213";
    private int port = 50000;

    TcpClient tcp_socket;
    NetworkStream net_stream;
    StreamWriter socket_writer;
    StreamReader socket_reader;

    public String playerName;

    int deadZoneLastIndex=0;

    int pawnIndex = -1;


    void Awake(){
        tcp_socket = new TcpClient(server, port);
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


    // Update is called once per frame
    void Update()
    {
        // String received_data = readLineSocket();
        String received_data = readLineSocket();
        if (received_data.Length!=0){
        	// Do something with the received data,
            // Debug.Log("received network data ! Lenght:" + received_data.Length.ToString());
            
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

            int pieceIndex = getPlayerPieceIndexAt(requestEnd[0], requestEnd[1], playerMoving);
            // puis on bouge la piece du playerMoving
            movePiece(pieceIndex, playerMoving, requestEnd);

            // transform pawn to something else if he reaches the last row!
            if(isAPawn(pieceIndex, playerMoving)){ // its a pawn
                GameObject.Find(playerMoving).GetComponent<player>().disableScripts();
                
                if(playerMoving==playerName){ // on affiche le GUI pour choisir la pièce
                    pawnIndex = pieceIndex;
                    GameObject.Find("Menu").transform.GetChild(0).gameObject.SetActive(true);
                    
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
    

    public void transformPawn(String pieceType){
        Dictionary<String, int> typeIndex = new Dictionary<string, int>(){
            {"Rook", 0},
            {"Knight", 1},
            {"Bishop", 2},
            {"Queen", playerName=="Player1" ? 3 : 4}, // queen is not at the same index
            // depending on if its white or black
        };
        GameObject player = GameObject.Find(playerName);
        GameObject piece = Instantiate(player.transform.GetChild(typeIndex[pieceType]).gameObject, player.transform);
        
        Transform pawnTransform = player.transform.GetChild(pawnIndex);
        piece.transform.localPosition = new Vector3(pawnTransform.localPosition.x, 0, pawnTransform.localPosition.z);

        piece.transform.SetSiblingIndex(pawnIndex);
        // remove the GameObject from the game
        Destroy(pawnTransform.gameObject);
        pawnIndex = -1; // reset the pawn index
        // deactivate the GUI menu
        GameObject.Find("Menu").transform.GetChild(0).gameObject.SetActive(false);
        // reactivate the pieces scripts:
        GameObject.Find(playerName).GetComponent<player>().enableScripts();
    }

    bool isAPawn(int pieceIndex, String playerName){
        Transform pieceTransform = GameObject.Find(playerName).transform.GetChild(pieceIndex);
        String name = pieceTransform.gameObject.name;
        if(name.Length>=4 && name.Substring(0,4)=="Pawn"){
            return true;
        }
        return false;
    }


    void movePiece(int pieceIndex, String playerMoving, List<int> requestEnd){
        
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

    // void deactivatePlayer(String playerName){
    //     GameObject.Find(playerName).SetActive(false);
    // }
}
