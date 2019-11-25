using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class queen : piece
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

        List<int[]> rookCoords = rook.returnMoveCoords(parentPlayer, otherPlayer, transform);
        List<int[]> bishopCoords = bishop.returnMoveCoords(parentPlayer, otherPlayer, transform);
        
        foreach (int[] item in bishopCoords){
            rookCoords.Add(item);
            
        }

        return rookCoords;
    }
}
