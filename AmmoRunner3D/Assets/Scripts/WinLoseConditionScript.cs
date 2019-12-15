using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoseConditionScript : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject gameWonPanel;
    public Slider goalBar;
    public static int alliesWithoutAmmo = 0;               //if 5 allies end up without ammo you lose the game
    private int timesMovedForward = 0;
    public int winningCondition = 1;                        //set this to how many times you need to move forward to win the game

    public static bool mineActivated = false;

    private bool wait = false;
    private int goal = 0;

    // Start is called before the first frame update
    void Start()
    {
        goalBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (goal == 100)
        {
            /*timesMovedForward++;
            if (timesMovedForward == winningCondition)
            {*/
                gameWonPanel.SetActive(true);
            //}
            goal = 0;
        }
        if(alliesWithoutAmmo == 5 || mineActivated)
        {
            //lose game
            mineActivated = false;
            gameOverPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
        if (!wait)
        {
            wait = true;
            StartCoroutine(IncreaseGoalBar());
        }
    }

    IEnumerator IncreaseGoalBar()
    {
        goal++;
        int time = (1 + alliesWithoutAmmo)*3;
        yield return new WaitForSeconds(time);
        goalBar.value = goal;
        wait = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
