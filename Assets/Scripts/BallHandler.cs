using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D currentBallRigidBody;
    [SerializeField]
    private SpringJoint2D currentBallSpringJoint;
    [SerializeField]
    private float detachDelay = 0.5f;

    private Camera mainCamera;

    private bool isDragging;

    void Awake()
    {
        mainCamera = Camera.main;
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
    }
}
