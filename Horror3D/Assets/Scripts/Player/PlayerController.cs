using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private float rotationY = 0;

    public int ZoomFOV = 35;
    public int initialFOV;
    public float cameraZoomSmooth = 1;

    private bool isZoomed = false;
    public bool canMove = true;

    private Rigidbody rb;

    private bool isLeftFoot = true;
    private float stepMagnitude = 2f;
    private float distanceWalked = 0f;
    private Vector3 lastSoundPos;

    public string footstepFolder = "Footsteps_Metal_Walk";
    private AudioClip[] footstepClips;

    private CapsuleCollider capsule;
    private float originalHeight;
    private Vector3 originalCenter;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        if (capsule == null)
        {
            Debug.LogError("CapsuleCollider required on player for crouch functionality.");
            enabled = false;
            return;
        }

        rb.freezeRotation = true;
        rb.useGravity = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startYScale = transform.localScale.y;

        originalHeight = capsule.height;
        originalCenter = capsule.center;

        lastSoundPos = transform.position;

        footstepClips = Resources.LoadAll<AudioClip>(footstepFolder);
        if (footstepClips.Length == 0)
            Debug.LogWarning("Nie znaleziono dźwięków kroków w folderze: " + footstepFolder);
    }

    private void Update()
    {
        // Handle movement input
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float inputX = canMove ? Input.GetAxis("Horizontal") : 0;
        float inputZ = canMove ? Input.GetAxis("Vertical") : 0;

        Vector3 desiredMove = (forward * inputZ + right * inputX).normalized;
        moveDirection = desiredMove * (isRunning ? runSpeed : walkSpeed);

        // Handle looking (camera rotation)
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

        // Handle zoom
        if (Input.GetButtonDown("Fire2"))
            isZoomed = true;
        if (Input.GetButtonUp("Fire2"))
            isZoomed = false;

        if (isZoomed)
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ZoomFOV, Time.deltaTime * cameraZoomSmooth);
        else
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, initialFOV, Time.deltaTime * cameraZoomSmooth);

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            capsule.height = crouchYScale;
            capsule.center = new Vector3(capsule.center.x, crouchYScale / 2f, capsule.center.z);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                float headClearance = originalHeight - crouchYScale + 0.1f;

                int playerLayer = LayerMask.NameToLayer("Player");
                int mask = ~(1 << playerLayer); 

                if (!Physics.Raycast(transform.position, Vector3.up, headClearance, mask, QueryTriggerInteraction.Ignore))
                {
                    capsule.height = originalHeight;
                    capsule.center = originalCenter;
                }
            }

        }

        // Handle footsteps
        distanceWalked += (transform.position - lastSoundPos).magnitude;
        if (distanceWalked >= stepMagnitude)
        {
            PlayFootstep();
            distanceWalked = 0;
        }
        lastSoundPos = transform.position;
    }

    private void FixedUpdate()
    {
        // Only set horizontal velocity, keep vertical (gravity) velocity unchanged
        Vector3 velocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        rb.linearVelocity = velocity;
    }

    private void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0)
            return;

        AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];

        if (isLeftFoot)
        {
            if (!leftFoot.isPlaying)
                leftFoot.PlayOneShot(randomClip);
        }
        else
        {
            if (!rightFoot.isPlaying)
                rightFoot.PlayOneShot(randomClip);
        }
        isLeftFoot = !isLeftFoot;
    }
}
