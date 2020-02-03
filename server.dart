// build this dart and change the serverIP variable

import 'dart:io';
import 'dart:convert';
import 'dart:typed_data';


String serverIP = '192.168.1.27';

ServerSocket server;
AsciiDecoder asciiDecoder = AsciiDecoder(allowInvalid: false); // not ascii bytes cause an error

List<Socket> clients = [];
List<int> nbMessages = [0, 0];
bool gameStarted=false;
int playerTurn = 0; // player 1 starts the game


void main() {
  ServerSocket.bind(serverIP, 50000)
  .then((ServerSocket socket) {
      server = socket;
      server.listen((Socket client) {
        handleConnection(client);
      });
  });
}

void resetGameVariables(){
    gameStarted=false;
    playerTurn=0;
    nbMessages=[0,0];
    for(int i=0;i<2;i++){ // destroying the first two connections
      clients[0].destroy();
      clients.removeAt(0);
    }
}

void handleConnection(Socket client){
  print('Connection from '
    '${client.remoteAddress.address}:${client.remotePort}');

  if(clients.length<=2){
    if(clients.length==2){
      // reset
      resetGameVariables();
    }
    clients.add(client);
    client
      .skipWhile((event)=> !gameStarted) // react only once the game has started
      .where((event)=> (event.length>1 && event[0] <= 57 && event[0]>=48)) // keep only numbers
      .forEach(handleMessage(clients.length-1));
    client.write("Player"+clients.length.toString()+"\n"); // don't forget to end the line
  }

  if(clients.length==2){
    //start the game
    gameStarted = true;
    // clients[0].write("game starts!\n");
    // clients[1].write("game starts!\n");
  }
}


bool checkFormat(){ return false;} // for the .where() part

Function handleMessage(client_number){ // element is a ascii string (data sent over the network by the client) - Uint8List element
  return (element){
    nbMessages[client_number]++;
    print("client ${client_number + 1}, $element num: ${nbMessages[client_number]}\n");

    List<int> request = convertAscii(element);
    if(request[0]==playerTurn+1){ // if it is the right player turn
      // we o send the request to each client:
      clients[client_number].add(element);
      clients[1-client_number].add(element);
      // next player turn
      if(request.length>=6 && request[5]==6){ // if there is a transformation (pawn becoming something else)
        // do not pass the turn
        print("6e element de la requete: '${request[5]}'");
      }else{
        playerTurn = (playerTurn+1) % 2;
      }
    }

    if(request[0]==3){
      // transform request, not a move
      // we simply send it back 
      clients[client_number].add(element);
      clients[1-client_number].add(element);

      playerTurn = (playerTurn+1) % 2;
    }
  };
}

List<int> convertAscii(Uint8List asciiList){
  List<int> list = [];

  for (int el in asciiList) {
    list.add(el-48);
  }
  
  return list;
}

class GameState{} // if we currently want to check if the game state changes are ok