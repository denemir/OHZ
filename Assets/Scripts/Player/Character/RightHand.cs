using UnityEngine;

public class RightHand : MonoBehaviour
{
    // Start is called before the first frame update
    public Weapon weapon;
    private GameObject weaponModel;

    public bool holdingWeapon;
    public bool droppedWeapon { get; private set; }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HoldWeapon(Weapon weapon)
    {
        holdingWeapon = true;
        this.weapon = weapon;

        weapon.transform.SetParent(transform);

    }

    public void DropWeapon()
    {
        weapon.Drop();
        weapon = null;
        holdingWeapon = false;
        droppedWeapon = true;
    }

    public void ToggleDroppedWeapon()
    {
        droppedWeapon = !droppedWeapon;
    }
}
