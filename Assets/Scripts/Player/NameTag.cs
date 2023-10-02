using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    //nametag class represents the title hovering over the player. includes clan tag, if enabled
    public string text;
    public string clanTag;

    public float offsetX = 0f; //offset for nametag along x-axis (as it appears to the camera)
    public float offsetY = 0f; //offset for nametag along y-axis (as it appears to the camera)
    public float offsetZ = 0f; //offset for nametag along z-axis (as it appears to the camera)

    public Player target;
    private Vector3 pos;

    private Camera cam;

    //public TextMeshPro textMeshPro;
    public GameObject textMeshProPrefab;
    public TextMeshPro textMeshProComponent;

    //public NameTag(string text, Player target)
    //{
    //    this.text = text;
    //    this.target = target;
    //}

    //public NameTag(string text, Player target, Camera cam)
    //{
    //    this.text = text;
    //    this.target = target;
    //    this.cam = cam;
    //}

    //public NameTag(string text, string clanTag, Player target)
    //{
    //    this.text = text;
    //    this.clanTag = clanTag;
    //    this.target = target;
    //}

    // Start is called before the first frame update
    void Start()
    {
        //target = GetComponent<Player>();
        text = target.playerName;
        cam = target.activeCamera;

        pos = target.transform.position; //getting position
        pos.x = pos.x + offsetX; //adding offset to name tag
        pos.y = pos.y + offsetY; //adding offset to name tag
        pos.z = pos.z + offsetZ; //adding offset to name tag
        transform.position = pos;

        GameObject textMeshProInstance = Instantiate(textMeshProPrefab, pos, Quaternion.identity);

        textMeshProComponent = textMeshProInstance.GetComponent<TextMeshPro>();
        textMeshProComponent.transform.SetParent(transform);
        textMeshProComponent.text = text;
        textMeshProComponent.enabled = true;
        textMeshProComponent.color = Color.green;
        textMeshProComponent.fontSize = 12;
        textMeshProComponent.transform.position = pos; //transforming position
        textMeshProComponent.transform.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {   
        pos = target.transform.position; //getting position
        pos.x = pos.x + offsetX; //adding offset to name tag
        pos.y = pos.y + offsetY; //adding offset to name tag
        pos.z = pos.z + offsetZ; //adding offset to name tag
        transform.position = pos; //transforming position

        textMeshProComponent.transform.position = pos; //transforming position
        textMeshProComponent.transform.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
    }
}
