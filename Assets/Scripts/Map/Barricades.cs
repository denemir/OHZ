using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Barricades : MonoBehaviour
{
    //stats
    public int maxNumberOfBarricades { get; set; }
    private int currentNumberOfBarricades;
    public float barricadeRepairTime; //how much time it takes to place back one piece

    //barricade pieces
    public GameObject barricadesRegion;
    public BarricadePiece[] barricadePieces;
    public int numOfPieces;

    //interaction 
    public KeyCode interactKey;
    private bool isInitialized;
    public Player interactingPlayer;
    private Interactable interactable;

    //player states
    private Dictionary<Player, bool> playerStates = new Dictionary<Player, bool>();
    public UnityEvent repairBarricades;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Interactable>() != null)
        {
            interactable = GetComponent<Interactable>();
        }
        else Debug.Log("Interactable component not found on barricade prefab. Please attach Interactable script to Barricade Prefab.");

        if (GetComponent<Interactable>().interactions != null)
        {
            InitializeInteractions();
            isInitialized = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentNumberOfBarricades != numOfPieces)
        {
            interactable.activeInteraction = interactable.interactions[0];
        } else interactable.activeInteraction = null;
    }

    //interactions
    public void InitializeInteractions()
    {
        //if barricade is not complete
        interactable.interactions.Add(new Interactable.Interaction
        {
            prompt = "Hold F to repair the barricade",
            action = repairBarricades,
            key = interactKey

        });

        //set interaction
        interactable.activeInteraction = interactable.interactions[0];
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
