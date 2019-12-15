using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 12f;
    Vector3 forward, right;

    private float initialMoveSpeed;

    public int[] ammoInventory;    //0 = light ammo, 1 = medium ammo, 2 = heavy ammo
    int weight = 0, weightLimit = 100, Ammo1Weight = 20, Ammo2Weight = 30, Ammo3Weight = 40;   //Ammo1 = light ammo, Ammo2 = medium ammo, Ammo3 = heavy ammo
    public Text lightAmmoText, mediumAmmoText, heavyAmmoText, resupplyText, allyText; //update text from this script
    public Slider weightLimitBar;

    private float k;
    private bool stopMultipleParticleEffects = false;

    // Start is called before the first frame update
    void Start()
    {
        initialMoveSpeed = moveSpeed;
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
        k = initialMoveSpeed * (1f - (weight * 0.5f) / 100);
    }



    //Triggering and Interacting with the resupply point
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Resupply")
        {
            resupplyText.gameObject.SetActive(true);
        }
        else if(other.gameObject.tag == "Ally")
        {
            allyText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Resupply")
        {
            resupplyText.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Ally")
        {
            allyText.gameObject.SetActive(false);
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
                Inventory();
                weightLimitBar.value = weight;
                moveSpeed = k;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && (weight + Ammo2Weight) <= weightLimit) //pick up medium ammo
            {
                ammoInventory[1]++;
                mediumAmmoText.text = ammoInventory[1].ToString();
                Inventory();
                weightLimitBar.value = weight;
                moveSpeed = k;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3) && (weight + Ammo3Weight) <= weightLimit) //pick up heavy ammo
            {
                ammoInventory[2]++;
                heavyAmmoText.text = ammoInventory[2].ToString();
                Inventory();
                weightLimitBar.value = weight;
                moveSpeed = k;
            }

            if (Input.GetKeyDown(KeyCode.R))    //dump all ammo
            {
                ammoInventory[0] = 0;
                ammoInventory[1] = 0;
                ammoInventory[2] = 0;
                lightAmmoText.text = "0";
                mediumAmmoText.text = "0";
                heavyAmmoText.text = "0";
                Inventory();
                weightLimitBar.value = 0;
                moveSpeed = initialMoveSpeed;
            }
        }
        else if(other.gameObject.tag == "Ally")
        {
            if(other.gameObject.GetComponent<AllyScript>().destroyed) allyText.gameObject.SetActive(false);     //prevents text remaining on screen after ally is destroyed

            int tempAmmoVar = other.gameObject.GetComponent<AllyScript>().ammoType;
            int remainingAmmo = other.gameObject.GetComponent<AllyScript>().ammo;
            if (tempAmmoVar == 0)
            {
                if(ammoInventory[0] != 0 && remainingAmmo <= 60)
                {
                    allyText.text = "Ally needs light ammo.\nPress R to give ammo.";
                }
                else if(ammoInventory[0] == 0 && remainingAmmo <= 60)
                {
                    allyText.text = "Ally needs light ammo.";
                }
                else
                {
                    allyText.text = "Ally doesn't need to reload.";
                }
            }
            else if(tempAmmoVar == 1)
            {
                if (ammoInventory[1] != 0 && remainingAmmo <= 60)
                {
                    allyText.text = "Ally needs medium ammo.\nPress R to give ammo.";
                }
                else if (ammoInventory[1] == 0 && remainingAmmo <= 60)
                {
                    allyText.text = "Ally needs medium ammo.";
                }
                else
                {
                    allyText.text = "Ally doesn't need to reload.";
                }
            }
            else
            {
                if (ammoInventory[2] != 0 && remainingAmmo <= 60)
                {
                    allyText.text = "Ally needs heavy ammo.\nPress R to give ammo.";
                }
                else if (ammoInventory[2] == 0 && remainingAmmo <= 60)
                {
                    allyText.text = "Ally needs heavy ammo.";
                }
                else
                {
                    allyText.text = "Ally doesn't need to reload.";
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && ammoInventory[tempAmmoVar] != 0 && remainingAmmo <= 60)    //"give" (hey there's the theme) ammo to ally
            {
                ammoInventory[tempAmmoVar]--;
                if(tempAmmoVar == 0)
                    lightAmmoText.text = ammoInventory[0].ToString();
                else if(tempAmmoVar == 1)
                    mediumAmmoText.text = ammoInventory[1].ToString();
                else
                    heavyAmmoText.text = ammoInventory[2].ToString();
                other.gameObject.GetComponent<AllyScript>().AmmoDelivered();
                Inventory();
                moveSpeed = k;
                weightLimitBar.value = weight;
            }
        }
        else if(other.gameObject.tag == "Mine" && !stopMultipleParticleEffects)
        {
            stopMultipleParticleEffects = true;
            StartCoroutine(ActivateMine(other.gameObject));
        }
    }

    IEnumerator ActivateMine(GameObject g)
    {
        g.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.3f);
        Destroy(g);
        WinLoseConditionScript.mineActivated = true;
    }
}
