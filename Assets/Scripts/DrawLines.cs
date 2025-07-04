using System.Runtime.CompilerServices;
using UnityEngine;

public class DrawLines : MonoBehaviour
{
    [SerializeField] private WhiteBall whiteBall;
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject wrongBall;
    [SerializeField] private Transform behindCue;
    [SerializeField] private Transform cue;
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private LineRenderer whiteBallPath;
    [SerializeField] private LineRenderer targetBallPath;
    [SerializeField] private ShotPower shotPower;
    [SerializeField] private BallManager ballMovementManager;
    private BaseBall Ballhit;

    private bool IsShoting = true;

    float whiteBallRadius = 0.215f;


    private void Start()
    {

        shotPower.OnShot += ShotPower_OnShot;
        ballMovementManager.onAllBallsStoped += ballMovementManager_onAllBallsStoped;
    }

    private void ballMovementManager_onAllBallsStoped(object sender, System.EventArgs e)
    {
        targetBallPath.SetPosition(0, Vector3.zero);
        targetBallPath.SetPosition(1, Vector3.zero);
        whiteBallPath.SetPosition(0, Vector3.zero);
        whiteBallPath.SetPosition(1, Vector3.zero);
        Show();
    }

    private void ShotPower_OnShot(object sender, ShotPower.Shotinfo e)
    {
        Hide();
    }



    private void Update()
    {
        DrowAimLine();
    }


    private void DrowAimLine()
    {


        Vector3 startPosition = whiteBall.transform.position;
        Vector3 direction = (cue.transform.position - behindCue.transform.position).normalized;
        float maxDistance = 50f;
        RaycastHit hit;

        if (Physics.SphereCast(startPosition, whiteBallRadius, direction, out hit, maxDistance))
        {

            float circleRadius = 0.19f;
            startPosition.z = -0.5f;

            Vector3 hitPoint = hit.point;
            hitPoint.z = -0.5f;



            Vector3 adjustedHit = hitPoint + hit.normal * circleRadius;
            adjustedHit.z = -0.5f;

            aimLine.SetPosition(0, startPosition);
            aimLine.SetPosition(1, adjustedHit);
            circle.transform.position = adjustedHit;

            if (hit.collider.gameObject.TryGetComponent<BaseBall>(out Ballhit))
            {
                int ballnumber = Ballhit.RetetnBallNumber();

                bool isWrong = false;
                if (Player.Instance.isPlayerHaveType)
                {
                    if (Player.Instance.playerType == PlayerType.Solid)
                        isWrong = !(ballnumber >= 1 && ballnumber <= 7);
                    else if (Player.Instance.playerType == PlayerType.Stripes)
                        isWrong = !(ballnumber >= 9 && ballnumber <= 15);
                    else if (Player.Instance.playerType == PlayerType.PotedAll)
                        isWrong = ballnumber != 8;
                }

               
                wrongBall.SetActive(isWrong);

                if (isWrong)
                {
                    whiteBallPath.enabled = false;
                    targetBallPath.enabled = false;
                    return; 
                }

                if (IsShoting)
                {
                    whiteBallPath.enabled = true;
                    targetBallPath.enabled = true;
                }

                Vector3 hitBallPosition = hit.transform.position;
                hitBallPosition.z = -0.5f;
                targetBallPath.SetPosition(0, hitBallPosition);
                Vector3 r2dir = hitBallPosition - adjustedHit;
                Ray r2 = new Ray(hitBallPosition, r2dir);

                Vector3 pos3 = r2.origin + 1.5f * r2dir;
                pos3.z = -0.5f;
                targetBallPath.SetPosition(1, pos3);


                Vector3 l = 1.5f * r2dir;
                l = Quaternion.Euler(0, 0, -90) * l + adjustedHit;
                l.z = -0.5f;


                whiteBallPath.SetPosition(0, adjustedHit);

                float angleBeetwen = AngleBetweenThreePoints(cue.transform.position, adjustedHit, l);


                if (angleBeetwen < 90.0f || angleBeetwen > 270.0f)
                {
                    l = 1.5f * r2dir;
                    l = Quaternion.Euler(0, 0, 90) * l + adjustedHit;
                    l.z = -0.5f;
                }

                whiteBallPath.SetPosition(1, l);
            }
            else
            {
                whiteBallPath.enabled = false;
                targetBallPath.enabled = false;
                wrongBall.SetActive(false);

            }
        }

    }
    public float AngleBetweenThreePoints(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        float a = pointB.x - pointA.x;
        float b = pointB.y - pointA.y;
        float c = pointB.x - pointC.x;
        float d = pointB.y - pointC.y;

        float atanA = Mathf.Atan2(a, b) * Mathf.Rad2Deg;
        float atanB = Mathf.Atan2(c, d) * Mathf.Rad2Deg;

        float output = atanB - atanA;
        output = Mathf.Abs(output);



        return output;
    }

   public void Hide()
    {
        IsShoting = false;
        circle.SetActive(false);
        aimLine.enabled = false;
        whiteBallPath.enabled = false;
        targetBallPath.enabled = false;
        wrongBall.SetActive (false);
    }

   public void Show()
    {
        IsShoting = true;
        circle.SetActive(true);
        aimLine.enabled = true;
        whiteBallPath.enabled = true;
        targetBallPath.enabled = true;
    }


}