using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn : piece
{
    // Start is called before the first frame update
    protected override void Start()
    {
       base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override List<int[]> getPossibleMoveCoords(){
        List<int[]> list = new List<int[]>{};

        int[,] pieceAt = parentPlayer.GetComponent<player>().pieceAtPosition;
        int[,] pieceAtEnemy= otherPlayer.GetComponent<player>().pieceAtPosition;
        
        int x = (int)transform.localPosition.x;
        int z = (int)transform.localPosition.z;

        // forward move
        if(z+1<8 && pieceAt[x,z+1]==-1 && pieceAtEnemy[7-x, 7-(z+1)]==-1){
                list.Add(new int[]{x,z+1});
        }
        // diagonal left
        if(x-1>=0 && z+1<8 && pieceAtEnemy[7-(x-1),7-(z+1)]!=-1){
            // if a enemy piece is here we can move
            list.Add(new int[]{x-1, z+1});
        }
        // diagonal right
        if(x+1<8 && z+1<8 && pieceAtEnemy[7-(x+1),7-(z+1)]!=-1){
            // if a enemy piece is here we can move
            list.Add(new int[]{x+1, z+1});
        }

        // double move at start
        if(z==1){
            if(pieceAt[x,z+2]==-1 && pieceAtEnemy[7-x,7-(z+2)]==-1){
                list.Add(new int[]{x, z+2});
            }
        }

        return list;
    }
}
