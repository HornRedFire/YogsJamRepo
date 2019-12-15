using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyScript : MonoBehaviour
{
    public int ammo = 0;
    public int ammoType = 0;            //0-light, 1-medium, 2-heavy
    public int rateOfFire = 1;          //amount of ammo used every second
    private ParticleSystem explosion;

    private bool wait = false;          //needed this because Update was being called every frame so it didn't wait until ammo is decreased once, so the game would end as soon as it started
    public bool destroyed = false; 

    // Start is called before the first frame update
    void Start()
    {
        explosion = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait) {
            if (ammo <= 0 && !destroyed)
            {
                destroyed = true;
                StartCoroutine(DestroyAlly());
            }
            else if (ammo > 0)
            {
                wait = true;
                StartCoroutine(DecreaseAmmo());          //decreases ammo every second
            }
        }
        if(ammo < 51)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    IEnumerator DestroyAlly()
    {
        explosion.Play();
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
        WinLoseConditionScript.alliesWithoutAmmo++;         //increase number of allies without ammo
    }

    IEnumerator DecreaseAmmo()
    {
        yield return new WaitForSeconds(1);
        ammo -= rateOfFire;
        wait = false;
    }

    public void AmmoDelivered()
    {
        ammo += 50;
    }
}
