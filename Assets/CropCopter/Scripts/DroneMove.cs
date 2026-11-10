
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMove : MonoBehaviour
{
    public InputActionReference moveAction;
    public GameObject model;
    public GameObject fence;
    public GridManager gridManager;
    public Canvas selectedBoxUi;
    
    public int speed = 15;
    public float maxTilt = 15f;
    private float tiltSpeed = 5f;

    private Vector3 minPosition;
    private Vector3 maxPosition;

    private Quaternion initialRotation;
    private Rigidbody rigidbody;
    private Vector2 move;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        initialRotation = model.transform.localRotation;

        BoxCollider fenceCollider = fence.GetComponent<BoxCollider>();

        minPosition = fenceCollider.bounds.min;
        maxPosition = fenceCollider.bounds.max;
    }

    void Update()
    {
        move = moveAction.action.ReadValue<Vector2>();

        Vector3 direction = new Vector3(move.x, 0f, move.y);
        Vector3 localDirection = Quaternion.Inverse(initialRotation) * direction;

        model.transform.localRotation = Quaternion.Slerp(
            model.transform.localRotation,
            initialRotation * Quaternion.Euler(localDirection.z * maxTilt, 0f, -localDirection.x * maxTilt),
            tiltSpeed * Time.deltaTime
        );

        (Vector2, GameObject) box = gridManager.GetClosestBox(new Vector2(transform.position.x, transform.position.z));
        selectedBoxUi.transform.position = new Vector3(box.Item1.x, 0.3f, box.Item1.y);
        
        if (box.Item1 == new Vector2(-1, -1))
        {
            selectedBoxUi.gameObject.SetActive(false);
            return;
        }
        else
        {
            selectedBoxUi.gameObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(move.x, 0f, move.y);
        Vector3 mouvement = direction * (speed * Time.fixedDeltaTime);
        Vector3 nextPosition = rigidbody.position + mouvement;

        if (nextPosition.x < minPosition.x || nextPosition.z < minPosition.z ||
            nextPosition.x > maxPosition.x || nextPosition.z > maxPosition.z)
        {
            return;
        }

        rigidbody.MovePosition(nextPosition);
    }
}
