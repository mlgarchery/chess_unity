using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : piece
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
        return returnMoveCoords(parentPlayer, otherPlayer, transform);
    }

    static int[][] directionPositions(int i, int j, int x, int z){
        // ex: with forward move you have two possibilities, left and right
        int[][] arr = new int[2][];
        if(Mathf.Abs(i)==2){
            arr[0] = new int[2]{x+i, z+1};
            arr[1] = new int[2]{x+i, z-1};
        }
        if(Mathf.Abs(j)==2){
            arr[0] = new int[2]{x+1, z+j};
            arr[1] = new int[2]{x-1, z+j};
        }
        return arr;
    }

    public static List<int[]> returnMoveCoords(GameObject parentPlayer, GameObject otherPlayer, Transform currentTransform){
        List<int[]> list = new List<int[]>{};

        int[,] pieceAt = parentPlayer.GetComponent<player>().pieceAtPosition;
        int[,] pieceAtEnemy= otherPlayer.GetComponent<player>().pieceAtPosition;
        
        int x = (int)currentTransform.localPosition.x;
        int z = (int)currentTransform.localPosition.z;

        // forward
        List<int[]> directions = new List<int[]>{
            new int[]{0,2},
            new int[]{0,-2},
            new int[]{2, 0},
            new int[]{-2, 0},
        };

        foreach (int[] d in directions){
           int[][] positions = directionPositions(d[0], d[1], x, z);
           foreach (int[] position in positions){
               int i = position[0];
               int j = position[1];
                if(position[0]>=0 && position[0]<8 && position[1]>=0 && position[1]<8
                    && pieceAt[i,j]==-1){
                        list.Add(new int[]{i, j});
                }               
           }
        }

        return list;    
    }
}
