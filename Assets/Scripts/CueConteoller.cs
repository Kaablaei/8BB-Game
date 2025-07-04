using UnityEngine;

public class CueConteoller : MonoBehaviour
{

    [SerializeField] private GameObject whiteBall;
    [SerializeField] private GameObject behindCue;
    [SerializeField] private ShotPower shotPower;
    [SerializeField] private BallManager ballMovementManager;

    private Vector3 defaultPosition;


    private float rotationSpeed = 0.75f;
    private bool isRotating = false;
    private float lastAngle;

    private void Start()
    {
        SetPositionTowhiteBall();
        shotPower.OnShot += ShotPower_OnShot;
        ballMovementManager.onAllBallsStoped += BallMovementManager_onAllBallsStoped; ;
    }


    public void PullBack(float amount)
    {
        float sensetiv = 0.4f;
        Vector3 targetPos = Vector3.Lerp(defaultPosition, behindCue.transform.position, amount * sensetiv);
        transform.position = targetPos;
    }

    public void SetPositionTowhiteBall()
    {
        float CueZpos = -0.5f;
        transform.position = new Vector3(whiteBall.transform.position.x, whiteBall.transform.position.y, CueZpos);
        defaultPosition = transform.position;
    }

    private void Update()
    {
        if(shotPower.IsShoting==false)
        {
            RotationHandel();
        }
       
    }


    private void BallMovementManager_onAllBallsStoped(object sender, System.EventArgs e)
    {
        SetPositionTowhiteBall();
        Show();

    }

    private void ShotPower_OnShot(object sender, ShotPower.Shotinfo e)
    {
        Hide();
    }



    public void Hide()
    {
        gameObject.SetActive(false);
    } 
    public void Show()
    {
        gameObject.SetActive(true);
    }


    void RotationHandel()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isRotating = true;
            lastAngle = GetMouseAngle();
        }

        if (Input.GetMouseButton(0) && isRotating)
        {
            float currentAngle = GetMouseAngle();
            float deltaAngle = Mathf.DeltaAngle(lastAngle, currentAngle); 

            transform.RotateAround(whiteBall.transform.position, Vector3.forward, deltaAngle * rotationSpeed);

            lastAngle = currentAngle;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }
    }

    private float GetMouseAngle()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = worldMousePos - whiteBall.transform.position;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

}
