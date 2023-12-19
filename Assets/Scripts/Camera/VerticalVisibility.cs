using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VerticalVisibility : MonoBehaviour
{
    //purpose of this script is to handle raycasting and ensuring that the player is always visible within the frame and not blocked by anything above them.
    public Player player;
    public LayerMask visibilityLayer; //layer affecting visibility

    //distance
    public float maxTransparencyDistance = 10f;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //players current position
            Vector3 playerPosition = player.transform.position;

            //iterating all objects on visible layer
            Collider[] colliders = Physics.OverlapSphere(playerPosition, 100f, visibilityLayer);

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
                //RaycastHit secondHit = IsVisible(obj.gameObject.transform.position, GetHeadPosition(), obj);
                //if (secondHit.collider.gameObject != player.gameObject)
                //{
                //    secondHit.collider.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                //}
                //checkForSecondHit(obj.gameObject.transform.position, to, obj);
                return hit;
            }
        }
        obj.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        //return otherwise
        return hit;

    } //is player currently visible in below object using raycasts
    void checkForSecondHit(Vector3 from, Vector3 to, Collider obj)
    {
        RaycastHit hit = IsVisible(from, to, obj);
        //if(hit.collider.gameObject != player)
        //    hit.collider.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }
    void SetTransparency(GameObject obj, float val)
    {
        Renderer render = null;
        //Check for renderer component
        if (obj.GetComponent<Renderer>() != null)
        {
            render = obj.GetComponent<Renderer>();
        }
        else Debug.Log("No renderer found");
        

        if (render != null)
        {
            ////set color settings
            //Color color = render.material.color;
            //color.a = val;
            //render.material.color = color;
            ////Debug.Log("Transparency set to: " + val + " for GameObject " + obj.gameObject.name);
            
            obj.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;    
        }
    } //if an object is in view, set it to have a transparency value
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
