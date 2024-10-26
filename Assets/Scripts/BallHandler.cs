using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        if (currentBallRigidBody == null)
        {
            return;
        }

        bool hasTouches = Touch.activeTouches.Count > 0;
        if (!hasTouches)
        {
            if (isDragging)
            {
                LaunchBall();
            }


            return;
        }

        isDragging = true;
        currentBallRigidBody.bodyType = RigidbodyType2D.Kinematic;

        Vector2 touchPosition = Vector2.zero;

        foreach (Touch touch in Touch.activeTouches)
        {
            touchPosition += touch.screenPosition;
        }

        touchPosition /= Touch.activeTouches.Count;

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
