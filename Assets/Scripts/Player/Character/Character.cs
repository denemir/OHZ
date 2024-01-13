using UnityEngine;

public class Character : MonoBehaviour
{
    //public double currentvelocity; //players current velocity
    //public double maxVelocity; //highest value velocity can reach

    //public double acceleration; //rate at which player accelerates at

    public Perks[] activePerks;
    public int maxPerkCount = 4; //default is 4
    public int currentPerkCount;

    //model
    public GameObject model; /*{ get; private set; }*/
    //public GameObject modelPrefab;

    //hands
    public RightHand rightHand;
    //public GameObject rightHandPrefab;
    //private float rightHandOffset = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInChildren<MeshRenderer>()?.gameObject != null)
        {
            model = GetComponentInChildren<MeshRenderer>()?.gameObject;
        }
        else Debug.Log("No character model found.");

        if (GetComponentInChildren<MeshRenderer>()?.gameObject.GetComponentInChildren<RightHand>() != null)
        {
            rightHand = GetComponentInChildren<MeshRenderer>()?.gameObject.GetComponentInChildren<RightHand>();
            //RotateRightHand(GetComponentInParent<Player>().rotationAngle); //rotate hand to be in whatever position player is in
        }
        else Debug.Log("RightHand component not found in character model.");

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotateRightHand(float rotateAngle)
    {
        rightHand.transform.rotation = Quaternion.Euler(0f, rotateAngle, 0f);
    }
}
