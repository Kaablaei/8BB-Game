
using UnityEngine;


public class BaseBall : MonoBehaviour
{
    [SerializeField] private int ballNumber;
    public BallManager ballManager;

    public Transform shadowSprite;
    public Transform dropSprite;
    protected Rigidbody rb;

    public bool IsMoving = false;
    private float stopThreshold = 0.55f;





    private void Start()
    {
        rb = GetComponent<Rigidbody>();


        rb.sleepThreshold = 0.01f;

    }

    private void Update()
    {
        SetTheShadow();
        CheckIfStopped();
    }

    protected void CheckIfStopped()
    {
        if (rb.linearVelocity.magnitude > stopThreshold || rb.angularVelocity.magnitude > stopThreshold)
        {
            IsMoving = true;
        }

        else if (rb.linearVelocity.magnitude < stopThreshold && rb.angularVelocity.magnitude < stopThreshold)
        {

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            IsMoving = false;
        }
    }



    public int RetetnBallNumber()
    {
        return ballNumber;
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Pot")
        {

            ballManager.PotedBall(this);

            Destroy(gameObject);
        }
    }


    protected void SetTheShadow()
    {
        shadowSprite.rotation = Quaternion.identity;
        dropSprite.rotation = Quaternion.identity;

        Vector3 shadowPos = shadowSprite.position;
        shadowPos.y = transform.position.y;
        shadowPos.x = transform.position.x;
        shadowSprite.position = shadowPos;



        Vector3 dropPos = dropSprite.position;
        dropPos.x = transform.position.x;
        dropPos.y = transform.position.y;
        dropSprite.position = dropPos;

    }

}
