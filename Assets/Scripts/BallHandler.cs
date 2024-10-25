using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D currentBallRigidBody;

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!Touchscreen.current.primaryTouch.press.IsPressed())
        {
            currentBallRigidBody.bodyType = RigidbodyType2D.Dynamic;
            return;
        }
        currentBallRigidBody.bodyType = RigidbodyType2D.Kinematic;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 touchPositionInWorldSpace = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = touchPositionInWorldSpace;
    }
}
