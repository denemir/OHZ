using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform playerTransform;
    public float arrowOffset = 2f;
    public float arrowOffsetY = .8f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ////keyboard
        Vector3 arrowDirection = playerTransform.forward;
        Vector3 arrowPosition = playerTransform.position + arrowDirection * arrowOffset;
        arrowPosition.y -= arrowOffsetY;
        transform.position = arrowPosition;

        //transform.position = transform.position + offset;
        //transform.rotation = rotateOffset;
    }

    public void Rotate(float rotationAngle)
    {
        //transform.rotation = Quaternion.Euler(90f, 0f, rotationAngle); //rotate around z axis
        transform.rotation = Quaternion.Euler(90f, rotationAngle, 0f);
    }
}
