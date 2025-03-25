using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCam;
    public AudioSource leftFoot;
    public AudioSource rightFoot;

    public float walkSpeed = 3f;
    public float runSpeed = 5f;

    public float lookSpeed = 2f;
    public float lookXLimit = 75f;
    public float cameraRotationSmooth = 5f;

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    public int ZoomFOV = 35;
    public int initialFOV;
    public float cameraZoomSmooth = 1;

    private bool isZoomed = false;

    private bool canMove = true;

    CharacterController characterController;

    private bool isLeftFoot = true;
    private float stepMagnitude = 2f;
    float distanceWalked = 0f;
    private Vector3 lastSoundPos;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock And Hide Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startYScale = transform.localScale.y;

        lastSoundPos = transform.position;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float directionX = canMove ? Input.GetAxis("Vertical") : 0;
        float directionY = canMove ? Input.GetAxis("Horizontal") : 0;
        moveDirection = (forward * directionX) + (right * directionY);

        characterController.Move(moveDirection.normalized * Time.deltaTime * (isRunning ? runSpeed : walkSpeed));

        // foot steps
        distanceWalked += (transform.position - lastSoundPos).magnitude;
        lastSoundPos = transform.position;
        if (distanceWalked>=stepMagnitude)
        {
            PlayFootstep();
            distanceWalked = 0;
        }

        // Camera Movement In Action:
        if (canMove)
        {
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            rotationY += Input.GetAxis("Mouse X") * lookSpeed;

            Quaternion targetRotationX = Quaternion.Euler(rotationX, 0, 0);
            Quaternion targetRotationY = Quaternion.Euler(0, rotationY, 0);

            playerCam.transform.localRotation = Quaternion.Slerp(playerCam.transform.localRotation, targetRotationX, Time.deltaTime * cameraRotationSmooth);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * cameraRotationSmooth);
        }

        // Zooming In Action:
        if (Input.GetButtonDown("Fire2"))
            isZoomed = true;

        if (Input.GetButtonUp("Fire2"))
            isZoomed = false;

        if (isZoomed)
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ZoomFOV, Time.deltaTime * cameraZoomSmooth);
        else
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, initialFOV, Time.deltaTime * cameraZoomSmooth);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (!Physics.Raycast(transform.position, Vector3.up, startYScale - crouchYScale + 0.1f))
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
        }
    }

    void PlayFootstep()
    {
        if (isLeftFoot)
        {
            if (!leftFoot.isPlaying)
                leftFoot.Play();
        }
        else
        {
            if (!rightFoot.isPlaying)
                rightFoot.Play();
        }
        isLeftFoot = !isLeftFoot;
    }
}