using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class WhiteBall : BaseBall
{
    [SerializeField] ShotPower shotPower;
    [SerializeField] private GameObject hand;
    [SerializeField] private Referee referee;
    [SerializeField] private DrawLines drawLines;
    [SerializeField] private CueConteoller cue;
    private float sleepThresholdwhiteBall = 0.01f;

    private float checkRadius = 0.22f;
    private float zPos = 0.2373768f;

    private Camera mainCamera;
    private bool foul;
    private bool poted = false;


   private float minX = -5.80f, maxX = -2.75f;
   private float minY = -2.75f, maxY = 3.2f;

    private float maxXafterShot = 6.64f;

    private bool isDragging = false;
    private Collider ballCollider;

    public delegate void hit(int number);
    public event hit onHit;

    public event EventHandler onWhiteBallPoted;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        shotPower.OnShot += ShotPower_OnShot;
        rb.sleepThreshold = sleepThresholdwhiteBall;
        ballManager.AddBall(this);
        ballCollider = GetComponent<Collider>();
        referee.OnFoul += Referee_OnFoul;
        mainCamera = Camera.main;
        foul = true;
    }

    private void Referee_OnFoul(object sender, EventArgs e)
    {
        foul = true;
        if (poted)
        {
            gameObject.SetActive(true);
            FindNewLocation();
            ballManager.AddBall(this);

            poted = false;
        }
    }

    private void ShotPower_OnShot(object sender, ShotPower.Shotinfo e)
    {

        rb.AddForce(e.ShotDirection * e.ShotPower, ForceMode.Impulse);
        foul =false;
        hand.SetActive(false);

        if(maxX != maxXafterShot)
        {
            maxX = maxXafterShot;
        }
    }


    private void Update()
    {

        if (foul)
        {
            hand.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        rb.isKinematic = true;
                        isDragging = true;
                        drawLines.Hide();
                        cue.Hide();

                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                rb.isKinematic = false;
                isDragging = false;
                drawLines.Show();
                cue.Show();
                cue.SetPositionTowhiteBall();

            }
            if (isDragging)
            {

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, zPos)); 

                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 point = ray.GetPoint(distance);

                    point.x = Mathf.Clamp(point.x, minX, maxX);
                    point.y = Mathf.Clamp(point.y, minY, maxY);
                    point.z = zPos;


                    bool canMove = true;

                    Collider[] colliders = Physics.OverlapSphere(point, checkRadius);

                    foreach (var col in colliders)
                    {
                        if (col != ballCollider && col.gameObject.layer == LayerMask.NameToLayer("Balls"))
                        {
                            canMove = false;
                            break;
                        }
                    }
                    if (canMove)
                    {
                        transform.position = point;


                    }


                }
            }

        }

        hand.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        hand.transform.rotation = Quaternion.identity;

        base.SetTheShadow();
        base.CheckIfStopped();


    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.TryGetComponent<BaseBall>(out BaseBall ball))
        {
            onHit?.Invoke(ball.RetetnBallNumber());
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Pot")
        {
            ballManager.PotedBall(this);
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            onWhiteBallPoted?.Invoke(this, EventArgs.Empty);
            poted = true;
            gameObject.SetActive(false);

        }
    }



    public void FindNewLocation()
    {

        Vector3 newPosition = Vector3.zero;
        float checkRadius = 0.5f;
        int maxAttempts = 100;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = UnityEngine.Random.Range(minX, maxX);
            float randomY = UnityEngine.Random.Range(minY, maxY);
            newPosition = new Vector3(randomX, randomY, zPos);

           

            Collider[] colliders = Physics.OverlapSphere(newPosition, checkRadius);
            bool isColliding = false;

            foreach (var col in colliders)
            {
                if (col.gameObject.layer == LayerMask.NameToLayer("Balls"))
                {
                    isColliding = true;
                    break;
                }
            }

            if (!isColliding)
            {
                break;
            }
        }


        transform.position = newPosition;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }


}
