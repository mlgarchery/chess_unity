using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king : piece
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


        List<int[]> positions = new List<int[]>{
            new int[]{0, 1},
            new int[]{0, -1},
            new int[]{1, 0},
            new int[]{1, 1},
            new int[]{1, -1},
            new int[]{-1, 0},
            new int[]{-1, 1},
            new int[]{-1, -1},
        };
        foreach (int[] item in positions){
            int i = item[0];
            int j = item[1];
            if(x+i>=0 && x+i<8 && z+j>=0 && z+j<8 && pieceAt[x+i,z+j]==-1){
                list.Add(new int[]{x+i,z+j});
            }
            
        }

        return list;
    }
}
