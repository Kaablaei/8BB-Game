using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShotPower : MonoBehaviour
{
    [SerializeField] private GameObject shotCue;
    [SerializeField] private Transform maxPos;
    [SerializeField] private Transform minPos;
    [SerializeField] private GameObject cue;
    [SerializeField] private GameObject behindCue;
    [SerializeField] private GameObject backGraund;



    [SerializeField] private BallManager ballMovementManager;





    private CueConteoller CueConteoller;
    private Vector3 defaultShotCuePosition;
    private float ClicedYPos;
    private ShotPowerVisual Visual;





    public event EventHandler<Shotinfo> OnShot;
    public class Shotinfo { public float ShotPower; public Vector3 ShotDirection; }
    public bool IsShoting = false;

    void Start()
    {
        defaultShotCuePosition = shotCue.transform.position;
        maxPos.position = shotCue.transform.position;
        Visual = GetComponent<ShotPowerVisual>();
        CueConteoller = cue.GetComponent<CueConteoller>();
        ballMovementManager.onAllBallsStoped += BallMovementManager_onAllBallsStoped;
    }

    private void BallMovementManager_onAllBallsStoped(object sender, EventArgs e)
    {
        Show();
    }

    void Update()
    {
        if (IsShoting)
        {
            float currentMouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

            float newYPos = shotCue.transform.position.y + (currentMouseY - ClicedYPos);
            ClicedYPos = currentMouseY;
            newYPos = Mathf.Clamp(newYPos, minPos.position.y, maxPos.position.y);
            Vector3 cuePos = shotCue.transform.position;
            cuePos.y = newYPos;
            shotCue.transform.position = cuePos;
            Visual.setIndicatorColor(cuePos.y, maxPos.position.y);
            float power = (maxPos.position.y - shotCue.transform.position.y) / (maxPos.position.y - minPos.position.y);
            CueConteoller.PullBack(power);
        }


    }

    private void OnMouseDown()
    {
        IsShoting = true;
        ClicedYPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

    }

    private void OnMouseUp()
    {
        IsShoting = false;
        float power = (maxPos.position.y - shotCue.transform.position.y) / (maxPos.position.y - minPos.position.y);
        power = Mathf.Clamp01(power);
        if (power >= 0.02f)
        {
            Vector3 direction = (cue.transform.position - behindCue.transform.position).normalized;
            float ShotpowerSenecetive = 2.8f;
            OnShot?.Invoke(this, new Shotinfo { ShotDirection = direction, ShotPower = power * ShotpowerSenecetive });
            Hide();

            shotCue.transform.position = defaultShotCuePosition;
            Visual.setIndicatorColor(shotCue.transform.position.y, maxPos.position.y);
        }
        shotCue.transform.position = defaultShotCuePosition;
        Visual.setIndicatorColor(shotCue.transform.position.y, maxPos.position.y);


    }


    void Hide()
    {
        shotCue.SetActive(false);
        backGraund.SetActive(false);
        gameObject.SetActive(false);
    }
    void Show()
    {
        shotCue.SetActive(true);
        backGraund.SetActive(true);
        gameObject.SetActive(true);
    }

}
