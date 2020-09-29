using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop : piece
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

    public static List<int[]> returnMoveCoords(GameObject parentPlayer, GameObject otherPlayer, Transform currentTransform){

        List<int[]> list = new List<int[]>{};

        int[,] pieceAt = parentPlayer.GetComponent<player>().pieceAtPosition;
        int[,] pieceAtEnemy= otherPlayer.GetComponent<player>().pieceAtPosition;
        
        int x = (int)currentTransform.localPosition.x;
        int z = (int)currentTransform.localPosition.z;

        // Forward left
        int minForwardLeft = Mathf.Min(x, 7-z);
        for(int n=1; n<=minForwardLeft; n++){
            if(pieceAt[x-n,z+n]==-1 && pieceAtEnemy[7-(x-n),7-(z+n)]==-1){
                list.Add(new int[]{x-n, z+n});
            }else{ // we can't go further, we have to stop the execution
                if(pieceAtEnemy[7-(x-n),7-(z+n)]!=-1){
                    list.Add(new int[]{x-n, z+n});
                }
                break;
            }
        }

        // Forward right
        int minForwardRight = Mathf.Min(7-x, 7-z);
        for(int n=1; n<=minForwardRight; n++){
            if(pieceAt[x+n,z+n]==-1 && pieceAtEnemy[7-(x+n),7-(z+n)]==-1){
                list.Add(new int[]{x+n, z+n});
            }else{ // we can't go further, we have to stop the execution
                if(pieceAtEnemy[7-(x+n),7-(z+n)]!=-1){
                    list.Add(new int[]{x+n, z+n});
                }
                break;
            }
        }

        // backward left
        int minBackWardLeft = Mathf.Min(x, z);
        for(int n=1; n<=minBackWardLeft; n++){
            if(pieceAt[x-n,z-n]==-1 && pieceAtEnemy[7-(x-n),7-(z-n)]==-1){
                list.Add(new int[]{x-n, z-n});
            }else{ // we can't go further, we have to stop the execution
                if(pieceAtEnemy[7-(x-n),7-(z-n)]!=-1){
                    list.Add(new int[]{x-n, z-n});
                }
                break;
            }
        }

        // backward left
        int minBackWardRight = Mathf.Min(7-x, z);
        for(int n=1; n<=minBackWardRight; n++){
            if(pieceAt[x+n,z-n]==-1 && pieceAtEnemy[7-(x+n),7-(z-n)]==-1){
                list.Add(new int[]{x+n, z-n});
            }else{ // we can't go further, we have to stop the execution
                if(pieceAtEnemy[7-(x+n),7-(z-n)]!=-1){
                    list.Add(new int[]{x+n, z-n});
                }
                break;
            }
        }

        return list;
        
    }

}
