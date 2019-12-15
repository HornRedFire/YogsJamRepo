using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 4f;
    Vector3 forward, right;

    public int[] ammoInventory;    //0 = light ammo, 1 = medium ammo, 2 = heavy ammo
    int weight = 0, weightLimit = 100, Ammo1Weight = 20, Ammo2Weight = 30, Ammo3Weight = 40;   //Ammo1 = light ammo, Ammo2 = medium ammo, Ammo3 = heavy ammo
    public Text lightAmmoText, mediumAmmoText, heavyAmmoText, resupplyText; //update text from this script
    public Slider weightLimitBar;


    // Start is called before the first frame update
    void Start()
    {
        //movement based
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }



    // Update is called once per frame
    void Update()
    {
        //if (Input.anyKey)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            Move();

        Inventory();
    }



    //Functions
    void Move()
    {
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

        transform.forward = heading;
        transform.position += rightMovement;
        transform.position += upMovement;
    }

    void Inventory()
    {
        weight = (Ammo1Weight * ammoInventory[0]) + (Ammo2Weight * ammoInventory[1]) + (Ammo3Weight * ammoInventory[2]);
    }



    //Triggering and Interacting with the resupply point
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Resupply")
        {
            resupplyText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Resupply")
        {
            resupplyText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Resupply")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && (weight + Ammo1Weight) <= weightLimit) //pick up light ammo
            {
                ammoInventory[0]++;
                lightAmmoText.text = ammoInventory[0].ToString();
                weightLimitBar.value = weight;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && (weight + Ammo2Weight) <= weightLimit) //pick up medium ammo
            {
                ammoInventory[1]++;
                mediumAmmoText.text = ammoInventory[1].ToString();
                weightLimitBar.value = weight;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && (weight + Ammo3Weight) <= weightLimit) //pick up heavy ammo
            {
                ammoInventory[2]++;
                heavyAmmoText.text = ammoInventory[2].ToString();
                weightLimitBar.value = weight;
            }

            if (Input.GetKeyDown(KeyCode.R))    //dump all ammo
            {
                ammoInventory[0] = 0;
                ammoInventory[1] = 0;
                ammoInventory[2] = 0;
                lightAmmoText.text = "0";
                mediumAmmoText.text = "0";
                heavyAmmoText.text = "0";
                weightLimitBar.value = 0;
            }
        }
    }
}
