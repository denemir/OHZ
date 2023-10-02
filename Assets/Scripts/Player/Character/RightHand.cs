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
        //if (weapon != null)
        //{
        //    if (!holdingWeapon)
        //    {
        //        holdingWeapon = true;
        //    }
        //} else holdingWeapon = false;
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

    //public void PassWeaponDown(Weapon weapon) //for use between player and right hand, Player handshakes with RightHand to provide weapon
    //{

    //}
}
