using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Barricades : MonoBehaviour
{
    //stats
    public int maxNumberOfBarricades { get; set; }
    public int currentNumberOfBarricades;
    public float barricadeRepairTime; //how much time it takes to place back one piece

    //barricade pieces
    public GameObject barricadesRegion;
    public BarricadePiece[] barricadePieces;
    public int numOfPieces;

    //interaction
    private Interactable interactable;

    public string repairPrompt;
    public UnityEvent repairBarricades;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //zombie functions
    public void DestroyBarricadePiece() //enemy takes away a piece of the barricade
    {

    }

    //player functions
    public void RestoreBarricade()
    {
        RewardPlayer();
    }
    public void RewardPlayer() //check if player can receive points first (max number of barricades repaired)
    {

    }

    //getters & setters (switch barricades.length for number of barricade pieces currently up ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public bool isBarricadeDestroyed()
    {
        return (barricadePieces.Length == 0);
    } 
    public bool isBarricadeRepaired()
    {
        return (barricadePieces.Length == numOfPieces);
    }
}
