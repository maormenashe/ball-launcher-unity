using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private Rigidbody2D pivotRigidbody;
    [SerializeField]
    private float detachDelay = 0.2f;
    [SerializeField]
    private float respawnDelay = 0.5f;

    private Rigidbody2D currentBallRigidBody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        SpawnNewBall();
    }

    void Update()
    {
        if (currentBallRigidBody == null)
        {
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.IsPressed())
        {
            if (isDragging)
            {
                LaunchBall();
            }


            return;
        }

        isDragging = true;
        currentBallRigidBody.bodyType = RigidbodyType2D.Kinematic;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 touchPositionInWorldSpace = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = touchPositionInWorldSpace;
    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivotRigidbody.position, Quaternion.identity);

        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivotRigidbody;
    }

    private void LaunchBall()
    {
        currentBallRigidBody.bodyType = RigidbodyType2D.Dynamic;
        currentBallRigidBody = null;

        Invoke(nameof(DetachBall), detachDelay);

        isDragging = false;
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }
}
