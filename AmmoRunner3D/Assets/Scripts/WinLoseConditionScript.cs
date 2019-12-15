using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseConditionScript : MonoBehaviour
{
    public GameObject gameOverPanel;

    public static int alliesWithoutAmmo = 0;               //if all allies end up without ammo you lose the game
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(alliesWithoutAmmo == 3)
        {
            //lose game
            gameOverPanel.SetActive(true);
            //Time.timeScale = 0.0f;
        }
    }

    void RestartGame()
    {
        
    }
}
