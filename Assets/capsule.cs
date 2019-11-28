using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capsule : MonoBehaviour
{
    // Start is called before the first frame update

    public List<int> initialPiecePosition;

    Renderer rend;
    Color initialMaterialColor;
    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        initialMaterialColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(1)){ // sur un click droit on supprime toute les capsule du joueur
            if(gameObject.name!="PositionCapsule"){ // original obj
                Destroy(gameObject);
            }
        }
    }

    void OnMouseEnter(){
        rend.material.color = Color.white;
    }

    void OnMouseExit(){
        rend.material.color = initialMaterialColor;
    }
    void OnMouseUp(){
        // destroy all capsule of the player (this method should be located in player.cs, and called by the different gameobjects)
        // https://answers.unity.com/questions/183649/how-to-find-a-child-gameobject-by-name.html
        // send the request for the move
        string playerName = GameObject.Find("PlayerPerspective").GetComponent<playerPerspective>().playerName;
        int playerNumber = (int)playerName[6];

        // make the request
        List<int> request = new List<int>{
            playerNumber-48, // its a char
            initialPiecePosition[0],
            initialPiecePosition[1],
            (int)transform.localPosition.x,
            (int)transform.localPosition.z,
        };

        int pieceIndex = GameObject.Find(playerName).GetComponent<player>().pieceAtPosition[
            initialPiecePosition[0],
            initialPiecePosition[1]
        ];

        if(GameObject.Find("PlayerPerspective").GetComponent<playerPerspective>().isAPawn(pieceIndex, playerName) && request[4]==7){ // si c'est un pion qui atteint la dernière ligne
            request.Add(6); // on envoit une requête spéciale, un peu plus longue
        }
        
        // send it to the dedicated server
        GameObject.Find("PlayerPerspective").GetComponent<playerPerspective>().writeRequest(request);

        // delete all capsule
        GameObject.Find(playerName).GetComponent<player>().destroyAllPlayerCaspule();
    }

    
}
