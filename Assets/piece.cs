using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piece : MonoBehaviour
{
    // Start is called before the first frame update
    
    protected GameObject parentPlayer;
    protected GameObject otherPlayer;

    String playerName;
    protected virtual void Start()
    {
        parentPlayer = gameObject.transform.parent.gameObject;
        if(parentPlayer.name=="Player1"){
            otherPlayer = GameObject.Find("Player2");
        }else{ otherPlayer = GameObject.Find("Player1"); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void destroyScript(){
        Destroy(this);
    }

    void OnMouseUp(){
        Debug.Log(this.gameObject+ "is clicked");
        playerName = GameObject.Find("PlayerPerspective").GetComponent<playerPerspective>().playerName; 
        // we clean all capsules
        GameObject.Find(playerName).GetComponent<player>().destroyAllPlayerCaspule();
        // then we display capsule (posible moves) for this piece
        if(parentPlayer.name==playerName){
            displayPositions();
        }
    }

    public void displayPositions(){
        List<int[]> list = getPossibleMoveCoords();
        foreach(int[] pos in list){
            GameObject newCapsule = Instantiate(GameObject.Find("PositionCapsule"),  parentPlayer.transform); // set as a child of the player
            newCapsule.GetComponent<capsule>().initialPiecePosition = new List<int>{
                (int)transform.localPosition.x,
                (int)transform.localPosition.z
            };
            newCapsule.transform.localPosition = new Vector3(pos[0], 0, pos[1]);
        }
    }

    protected virtual List<int[]> getPossibleMoveCoords(){
        return new List<int[]>{};
    }
}
