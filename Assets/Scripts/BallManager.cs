using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallManager : MonoBehaviour
{

    [SerializeField] ShotPower shotPower;

    private List<BaseBall> balls = new List<BaseBall>();
    private bool SholdChekOnce = false;

    public event EventHandler onAllBallsStoped;

    public delegate void BallPoted(int ballNumber);
    public event BallPoted OnBallPoted;

    public bool IsWhiteBallInPot { get; private set; }

    public void SetWhiteBallPoted()
    {
        IsWhiteBallInPot = true;
    }

    public void SetWhiteBallReturned()
    {
        IsWhiteBallInPot = false;
    }



    private void Awake()
    {
        shotPower.OnShot += ShotPower_OnShot;
    }
   
    public void AddBall(BaseBall ball)
    {
        balls.Add(ball);
    }

    public void PotedBall(BaseBall ball)
    {
        OnBallPoted?.Invoke(ball.RetetnBallNumber());
        balls.Remove(ball);
    }

    public void RemoveBall(BaseBall ball)
    {
        balls.Remove(ball);
    }



    public bool AreAllBallsStopped()
    {
        foreach (BaseBall ball in balls)
        {
            if (ball.IsMoving)
                return false;
        }

        return true;
    }  
    public bool AreAllBallsStoppedSec()
    {
        foreach (BaseBall ball in balls)
        {
            if (ball.IsMoving)
                return false;
        }

        return true;
    }

    private void LateUpdate()
    {
        if (SholdChekOnce)
        {
            if (AreAllBallsStopped())
            {
                if (AreAllBallsStoppedSec())
                {
                    onAllBallsStoped?.Invoke(this, EventArgs.Empty);
                    SholdChekOnce = false;
                }
            }
        }
    }
    private void ShotPower_OnShot(object sender, ShotPower.Shotinfo e)
    {
        StartCoroutine(DelayedCheck());
    }

    private IEnumerator DelayedCheck()
    {
        yield return new WaitForSeconds(0.1f);
        SholdChekOnce = true;
    }




}
