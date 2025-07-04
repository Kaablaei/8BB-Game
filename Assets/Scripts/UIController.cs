using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private BallManager ballManager;
    [SerializeField] private GameObject solidsText;
    [SerializeField] private GameObject stripesText;
    [SerializeField] private GameObject Wintext;
    [SerializeField] private GameObject Lose;

    [SerializeField] private GameObject allSolidBalls;
    [SerializeField] private GameObject allStripesBalls;
    [SerializeField] private GameObject LeftEghtBall;
    [SerializeField] private GameObject RightEghtBall;

    [SerializeField] List<GameObject> ballsIcone;

    private bool isBallsReplased = false;



    private int solidsCounter = 7;

    private int stripesCounter = 7;


    private bool isFirstBallpoted = false;

    void Start()
    {
        ballManager.OnBallPoted += BallManager_OnBallPoted;
    }

    private void BallManager_OnBallPoted(int ballNumber)
    {

        if (ballNumber >= 1 && ballNumber <= 7)
        {
            //solids

            if (!isFirstBallpoted)
            {
                solidsText.SetActive(true);
                 
                ShowBalls();
                isFirstBallpoted = true;
            }

            ballsIcone[ballNumber - 1].SetActive(false);

            solidsCounter = solidsCounter - 1;


            if (solidsCounter <= 0)
            {
                if (isBallsReplased)
                {
                    RightEghtBall.SetActive(true);
                }
                else
                {
                    LeftEghtBall.SetActive(true);
                }
            }

        }
        else if (ballNumber >= 9 && ballNumber <= 15)
        {
            //stripes
            if (!isFirstBallpoted)
            {
                stripesText.SetActive(true);
                ReplaceBalls();
                ShowBalls();
                isFirstBallpoted = true;

            }
            ballsIcone[ballNumber - 1].SetActive(false);

         

            stripesCounter = stripesCounter - 1;

            if (stripesCounter <= 0)
            {
                if (isBallsReplased)
                {
                    LeftEghtBall.SetActive(true);
                }
                else
                {
                    RightEghtBall.SetActive(true);
                }
            }

         


        }
    }

    private void ShowBalls()
    {
        foreach (var ball in ballsIcone)
        {
            if (ball != ballsIcone[7])
            {
                ball.SetActive(true);
            }
        }
    }
    private void ReplaceBalls()
    {
        Vector3 solidlastpos = allSolidBalls.transform.position;
        allSolidBalls.transform.position = allStripesBalls.transform.position;
        allStripesBalls.transform.position = solidlastpos;
        isBallsReplased = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { ReplaceBalls(); }
    }
}