using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewVerticalVisibility : MonoBehaviour
{
    public Player player;
    public LayerMask visibilityLayer; //layer affecting visibility

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //iterating all objects on visible layer
            Collider[] colliders = Physics.OverlapSphere(new Vector3(GetHeadPosition().x, GetHeadPosition().y + 4.5f, GetHeadPosition().z), 10f, visibilityLayer); //i love magic numbers, dont you?

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.transform.position.y > GetHeadPosition().y + 3.5 && WithinXBounds(collider) && WithinZBounds(collider) || ((collider.gameObject.transform.position.y > player.transform.position.y && collider.bounds.size.y >= GetHeadPosition().y && WithinXBounds(collider) && WithinZBounds(collider) && collider.gameObject.transform.position.z < player.transform.position.z - 1)))
                    collider.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                else collider.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

                //if (collider.gameObject.transform.position.y > player.transform.position.y && collider.bounds.size.y >= GetHeadPosition().y && WithinXBounds(collider) && WithinZBounds(collider) && collider.gameObject.transform.position.z < player.transform.position.z)
                //    collider.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                //else collider.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }
    Vector3 GetHeadPosition()
    {
        return player.transform.position + Vector3.up/** 1.5f*/; // Adjust the value based on your player's head position
    }
    bool WithinXBounds(Collider collider)
    {
        return (collider.gameObject.transform.position.x - GetHeadPosition().x < 10) && (collider.gameObject.transform.position.x - GetHeadPosition().x > -10);
    }
    bool WithinZBounds(Collider collider)
    {
        return (collider.gameObject.transform.position.z - GetHeadPosition().z < 10) && (collider.gameObject.transform.position.x - GetHeadPosition().x > -10);
    }
}
