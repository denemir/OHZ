using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barricades : MonoBehaviour
{
    public int maxNumberOfBarricades { get; set; }
    public int currentNumberOfBarricades;
    
    public GameObject barricadesRegion;
    public GameObject[] barricadePieces;

    public float barricadeRepairTime; //how much time it takes to place back one piece
    //barricade cube span, how much physical space does it take up

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyBarricadePiece() //enemy takes away a piece of the barricade
    {

    }

    public void RestoreBarricade()
    {
        RewardPlayer();
    }

    public void RewardPlayer() //check if player can receive points first (max number of barricades repaired)
    {

    }
}
