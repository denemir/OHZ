using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VerticalVisibility : MonoBehaviour
{
    //purpose of this script is to handle raycasting and ensuring that the player is always visible within the frame and not blocked by anything above them.
    public Player player;
    public LayerMask visibilityLayer; //layer affecting visibility
    public LayerMask invisibilityLayer;

    //distance
    public float maxTransparencyDistance = 10f;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //iterating all objects on visible layer
            Collider[] colliders = Physics.OverlapSphere(GetHeadPosition(), 25f, visibilityLayer);

            foreach (Collider collider in colliders)
            {
                IsVisible(transform.position, GetHeadPosition(), collider);
            }
        }

    }

    //visibility functs
    RaycastHit IsVisible(Vector3 from, Vector3 to, Collider obj)
    {
        //Cast a ray from the player to the object
        Vector3 direction = to - from;
        RaycastHit hit;

        //check if there is an obstacle between the player and the camera
        if (Physics.Raycast(from, direction.normalized, out hit, direction.magnitude, visibilityLayer))
        {
            if(hit.collider.gameObject == obj.gameObject)
            {
                obj.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                //temporarily setting object to unviewed layer to check for second hit
                obj.gameObject.layer = 2;

                //check for second hit
                Collider[] colliders = Physics.OverlapSphere(obj.transform.position, 15f, visibilityLayer);

                foreach (Collider collider in colliders)
                {
                    IsVisible(transform.position, GetHeadPosition(), collider);
                }

                //set object back to original layer
                obj.gameObject.layer = 6;

                return hit;
            }
        }
        obj.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        //return otherwise
        return hit;

    } //is player currently visible in below object using raycasts
    Vector3 GetHeadPosition()
    {
        return player.transform.position + Vector3.up * 1.5f; // Adjust the value based on your player's head position
    }

    //getters & setters
    public void SetTarget(Player player)
    {
        this.player = player;
    } //set target player
}
