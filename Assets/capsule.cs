using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capsule : MonoBehaviour
{
    // Start is called before the first frame update

    public List<int> initialPiecePosition;
    void Start()
    {
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

    void OnMouseUp(){
        // destroy all capsule of the player (this method should be located in player.cs, and called by the different gameobjects)
        // https://answers.unity.com/questions/183649/how-to-find-a-child-gameobject-by-name.html
        // send the request for the move
        string playerName = GameObject.Find("PlayerPerspective").GetComponent<playerPerspective>().playerName;
        int playerNumber = (int)playerName[6];

        // make the request
        List<int> request = new List<int>{-1,-1,-1,-1,-1};
        request[0] = playerNumber-48; // its a char
        request[1] = initialPiecePosition[0];
        request[2] = initialPiecePosition[1];
        request[3] = (int)transform.localPosition.x;
        request[4] = (int)transform.localPosition.z;
        
        // send it to the dedicated server
        GameObject.Find("PlayerPerspective").GetComponent<playerPerspective>().writeRequest(request);
        
        Debug.Log("request sent ! first el: " + request[0]);

        // delete all capsule
        GameObject.Find(playerName).GetComponent<player>().destroyAllPlayerCaspule();
    }

    
}
