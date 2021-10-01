using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    public float baseHeight = 2;
    public float cameraBaseHeight = 0.7f;
    public float crouchHeight = 0.8f;
    public float cameraCrouchHeight = 0f;
    private float height;

    private Vector3 cameraHeight;
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera.transform.position = new Vector3(0f, cameraBaseHeight, 0f);
        cameraHeight.y = cameraBaseHeight;
    }

    void Update()
    {
		if(Input.GetKey("k"))
		{
			Cursor.visible = true;
		}
		if(Input.GetKey("l"))
		{
			Cursor.visible = false;
		}
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            characterController.height = Mathf.Lerp(characterController.height, crouchHeight, 10 * Time.deltaTime);
            cameraHeight.y = Mathf.Lerp(playerCamera.transform.localPosition.y, cameraCrouchHeight, 10 * Time.deltaTime);
        }else{
            characterController.height = Mathf.Lerp(characterController.height, baseHeight, 10 * Time.deltaTime);
            cameraHeight.y = Mathf.Lerp(playerCamera.transform.localPosition.y, cameraBaseHeight, 10 * Time.deltaTime);
        }
        playerCamera.transform.localPosition = cameraHeight;
    }
}