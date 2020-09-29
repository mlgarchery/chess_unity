using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rook : piece
{
    int x;
    int z; // initial position
    // Start is called before the first frame update
    protected override void Start()
    {   
        base.Start(); // equivalent de super en python
        // do other things
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected override List<int[]> getPossibleMoveCoords(){
        return returnMoveCoords(parentPlayer, otherPlayer, transform);
    }

    public static List<int[]> returnMoveCoords(GameObject parentPlayer, GameObject otherPlayer, Transform currentTransform){
        List<int[]> list = new List<int[]>{};

        int[,] pieceAt = parentPlayer.GetComponent<player>().pieceAtPosition;
        int[,] pieceAtEnemy= otherPlayer.GetComponent<player>().pieceAtPosition;
        
        int x = (int)currentTransform.localPosition.x;
        int z = (int)currentTransform.localPosition.z;

        // Forward
        for(int j=z+1; j<8; j++){
            if(pieceAt[x,j]==-1 && pieceAtEnemy[7-x,7-j]==-1){
                list.Add(new int[]{x, j});
            }else{ // we can't go further, we have to stop the execution
                if(pieceAtEnemy[7-x,7-j]!=-1){
                    list.Add(new int[]{x, j});
                }
                break;
            }
        }
        //Backward
        for(int j=z-1; j>=0; j--){
            if(pieceAt[x,j]==-1 && pieceAtEnemy[7-x,7-j]==-1){
                list.Add(new int[]{x, j});
            }else{ // we can't go further, we have to stop the execution
                if(pieceAtEnemy[7-x,7-j]!=-1){
                    list.Add(new int[]{x, j});
                }
                break;
            }
        }

        //Right
        for(int i=x+1; i<8; i++){
            if(pieceAt[i,z]==-1 && pieceAtEnemy[7-i,7-z]==-1){
                list.Add(new int[]{i, z});
            }else{
                if(pieceAtEnemy[7-i,7-z]!=-1){
                    list.Add(new int[]{i, z});
                }
                break;
            }
        }

        // Left 
        for(int i=x-1; i>=0; i--){
            if(pieceAt[i,z]==-1 && pieceAtEnemy[7-i,7-z]==-1){
                list.Add(new int[]{i, z});
            }else{
                if(pieceAtEnemy[7-i,7-z]!=-1){
                    list.Add(new int[]{i, z});
                }
                break;
            }
        }
        
        return list;
    }

}
