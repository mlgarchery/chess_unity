using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public int[,] pieceAtPosition= new int[8,8];
    void Start()
    {
        // -1 si aucune de ses pièces n'est présente sur cette case
        for(int i=0;i<8;i++){
            for(int j=0;j<8;j++){
                pieceAtPosition[i,j] = -1; // aucune pièce présente sur le 
                // plateau par défaut
            }
        }

        for(int i=0;i<16;i++){
            Transform pieceTransform=transform.GetChild(i);
            pieceAtPosition[(int)pieceTransform.localPosition.x, (int)pieceTransform.localPosition.z]=i;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void deletePieceAt(int x, int z){
        pieceAtPosition[x, z]= -1;
    }
    
    public void movePieceFromTo(int x1, int z1, int x2, int z2){
        int index = pieceAtPosition[x1, z1];
        deletePieceAt(x1, z1);
        pieceAtPosition[x2,z2] = index;
    }

    public void destroyAllPlayerCaspule(){
        foreach(Transform childTransform in this.gameObject.transform){
            if(childTransform.gameObject.name=="PositionCapsule(Clone)"){
                Destroy(childTransform.gameObject);
            }
        }
    }
}
