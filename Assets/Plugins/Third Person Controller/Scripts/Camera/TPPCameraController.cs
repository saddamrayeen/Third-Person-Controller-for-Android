using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPCameraController : MonoBehaviour
{
    // touch field

    public FixedTouchField touchField;
    // float axis
    float Yaxis;
    float Xaxis;

    // for positions
    Transform followTarget; // this will be the target that camera will follow
    Transform foucusTarget;// this will be the target that camera will focuse
    Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraFollowSpeed = 0;
    public float cameraPivotSpeed = 2f;
    public float cameraLookSpeed = 2f;

    // for rotations
    public float lookAngle;
    public float pivotAngle;

    // camera pivot

    Transform cameraPivot;
    float minPivotAngle = -35,
        maxPivotAngle = 35;

    // for camera collision
    private float defualtPosition; // player defualt transform
    Transform cameraTransform; // actual camera transform
    public float cameraCollisionRedius = 0.2f; // camera collision radius
    public float cameraCollisionOffset = 0.2f; // camera collision offset
    public float minCollisionOffset = 0.2f; // minimum collision offset
    public LayerMask collisionLayers; // layers we want the camera to collide with

    private float minRotation = -40f;// minimum rotation
    private float maxRotation = 80f;// // maximum rotation

    private Vector3 cameraVectorPosition;

    private void Awake()
    {

        cameraTransform = Camera.main.transform;

        cameraPivot = transform.GetChild(0).transform;
        defualtPosition = cameraTransform.localPosition.z;

        followTarget = FindObjectOfType<PlayerController>().transform;

    }

    private void Update()
    {
        Yaxis = touchField.TouchDist.x;
        Xaxis = touchField.TouchDist.y;
        Xaxis = Mathf.Clamp(Xaxis, minRotation, maxRotation);
    }

    private void FixedUpdate()
    {
        HandleAllCamerUpdates();
    }

    public void HandleAllCamerUpdates()
    {
        FollowTarget();
        RotateCamera();
        BoundCamera();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(
            transform.position,
            followTarget.position,
            ref cameraFollowVelocity,
            cameraFollowSpeed
        );

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        lookAngle += (Yaxis * cameraLookSpeed);
        pivotAngle -= (Xaxis * cameraPivotSpeed);

        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;

        Quaternion targetRotation = Quaternion.Euler(rotation);

        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);

        cameraPivot.localRotation = targetRotation;
    }

    private void BoundCamera()
    {
        float targetPosition = defualtPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (
            Physics.SphereCast(
                cameraPivot.transform.position,
                cameraCollisionRedius,
                direction,
                out hit,
                Mathf.Abs(targetPosition),
                collisionLayers
            )
        )
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = -(distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset)
        {
            targetPosition = targetPosition - minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
