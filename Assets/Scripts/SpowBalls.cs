using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpowBalls : MonoBehaviour
{
    [SerializeField] private Vector2[] position;
    [SerializeField] private List<GameObject> ballsPryfab;
    [SerializeField] private BallManager ballManager;
    private float Zposition = 0.2373768f;





    private void Start()
    {

        List<GameObject> allBalls = new List<GameObject>(ballsPryfab);

        GameObject ball8 = ballsPryfab[7]; 

        allBalls.Remove(ball8);

        List<GameObject> solids = new List<GameObject>();
        List<GameObject> stripes = new List<GameObject>();

        for (int i = 0; i < 15; i++)
        {
            if (i == 7) continue; 

            if (i < 7) solids.Add(ballsPryfab[i]);
            else stripes.Add(ballsPryfab[i]);
        }

        Shuffle(solids);
        Shuffle(stripes);

        List<GameObject> finalOrder = new List<GameObject>(new GameObject[15]);

        finalOrder[4] = ball8;
        finalOrder[10] = solids[0];
        finalOrder[14] = stripes[0];

        solids.RemoveAt(0);
        stripes.RemoveAt(0);

        List<GameObject> remaining = new List<GameObject>();
        remaining.AddRange(solids);
        remaining.AddRange(stripes);

        Shuffle(remaining);

        int pos = 0;
        for (int i = 0; i < 15; i++)
        {
            if (finalOrder[i] != null) continue;
            finalOrder[i] = remaining[pos];
            pos++;
        }

        for (int i = 0; i < 15; i++)
        {
            Vector3 spawnPos = new Vector3(position[i].x, position[i].y, Zposition);
            Quaternion desiredRotation = Quaternion.Euler(0f, 180f, 0f);
            GameObject newBall = Instantiate(finalOrder[i], spawnPos, desiredRotation);

            BaseBall baseBall = newBall.GetComponent<BaseBall>();

            baseBall.ballManager = ballManager;
            ballManager.AddBall(baseBall);


        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randIndex = Random.Range(i, list.Count);
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }







}
