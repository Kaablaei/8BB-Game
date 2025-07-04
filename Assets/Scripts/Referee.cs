using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Referee : MonoBehaviour
{

    [SerializeField] private WhiteBall whiteBall;
    [SerializeField] private ShotPower shotpower;
    [SerializeField] private BallManager ballManager;
    //
    [SerializeField] private Image leftGreenTimer;
    [SerializeField] private Image rhightGreenTimer;
    //

    private int firstBallHit = -1;
    private bool isfirstBallPoted = false;
    private bool isWhiteBallPoted = false;

    public event EventHandler OnFoul;

    private List<int> Potedball = new List<int>();

    private int solidBalls = 7;
    private int stripBalls = 7;
    private bool PlayerLose = false;

    void Start()
    {
        whiteBall.onHit += WhiteBall_onHit;
        whiteBall.onWhiteBallPoted += WhiteBall_onWhiteBallPoted;
        ballManager.onAllBallsStoped += BallManager_onAllBallsStoped;
        ballManager.OnBallPoted += BallManager_OnBallPoted;
        shotpower.OnShot += Shotpower_OnShot;
    }

    private void Shotpower_OnShot(object sender, ShotPower.Shotinfo e)
    {
        print("Shotpower_OnShot");
        firstBallHit = -1;
        isWhiteBallPoted = false;
        Potedball.Clear();
    }

    private void BallManager_OnBallPoted(int ballNumber)
    {

        Potedball.Add(ballNumber);

        if (ballNumber >= 1 && ballNumber <= 7)
        {
            solidBalls--;
        }
        else if (ballNumber >= 9 && ballNumber <= 15)
        {
            stripBalls--;
        }

        if (!isfirstBallPoted)
        {
            print("isfirstBallPoted");
            if (ballNumber >= 1 && ballNumber <= 7)
            {
                Player.Instance.playerType = PlayerType.Solid;
                Player.Instance.isPlayerHaveType = true;

            }
            else if (ballNumber >= 9 && ballNumber <= 15)
            {
                Player.Instance.playerType = PlayerType.Stripes;
                Player.Instance.isPlayerHaveType = true;
            }
            else if (ballNumber == 8)
            {
                //lose
                PlayerLose = true;
            }

            isfirstBallPoted = true;

        }


        if (Player.Instance.isPlayerHaveType)
        {
            if (Player.Instance.playerType == PlayerType.Stripes && stripBalls <= 0)
            {
                Player.Instance.playerType = PlayerType.PotedAll;
                print("only 8");
            }
            else if (Player.Instance.playerType == PlayerType.Solid && solidBalls <= 0)
            {
                Player.Instance.playerType = PlayerType.PotedAll;
                print("only 8");

            }
        }

        if (ballNumber == 8)
        {
            if (Player.Instance.isPlayerHaveType)
            {
                if (Player.Instance.playerType == PlayerType.Stripes)
                {
                    if (stripBalls <= 0)
                    {
                        //you win
                    }
                    else
                    {
                        PlayerLose = true;
                    }

                }
                else if (Player.Instance.playerType == PlayerType.Solid)
                {
                    if (solidBalls <= 0)
                    {
                        //you win
                    }
                    else
                    {
                        PlayerLose = true;
                    }

                }
            }
            else
            {
                PlayerLose = true;
            }

        }


    }

    private bool CheckFoul()
    {
        if (isWhiteBallPoted)
        {
            return true;
        }

        if (firstBallHit == -1)
        {
            return true;
        }
        if (Player.Instance.isPlayerHaveType)
        {
            if (Player.Instance.playerType == PlayerType.Solid)
            {

                if (firstBallHit >= 9 && firstBallHit <= 15 || firstBallHit == 8)
                    return true;
            }
            else if (Player.Instance.playerType == PlayerType.Stripes)
            {

                if (firstBallHit >= 1 && firstBallHit <= 7 || firstBallHit == 8)
                    return true;
            }
        }

        return false;
    }

    private bool CheckChangeTurn()
    {
        if (Potedball.Count == 0)
        {
            return true;
        }

        foreach (int ballnumber in Potedball)
        {
            print(ballnumber);


            if (Player.Instance.isPlayerHaveType)
            {
                if (Player.Instance.playerType == PlayerType.Stripes)
                {
                    if (ballnumber >= 1 && ballnumber <= 7)
                    {
                        return true;
                    }

                }
                else if (Player.Instance.playerType == PlayerType.Solid)
                {
                    if (ballnumber >= 9 && ballnumber <= 15)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    private void BallManager_onAllBallsStoped(object sender, EventArgs e)
    {
        print("BallManager_onAllBallsStoped");

        if (PlayerLose)
        {
            print("PlayerLose");
            return;
        }

        if (CheckFoul())
        {
            print("Foul occurred!");
            OnFoul?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (CheckChangeTurn())
        {
            ChangeTurn();
            return;
        }

    }

    private void ChangeTurn()
    {
        print("Change Turn");
    }
    private void WhiteBall_onHit(int number)
    {
        if (firstBallHit == -1)
        {
            firstBallHit = number;

        }
    }
    private void WhiteBall_onWhiteBallPoted(object sender, EventArgs e)
    {
        print("WhiteBall_onWhiteBallPoted");
        isWhiteBallPoted = true;
    }

}
