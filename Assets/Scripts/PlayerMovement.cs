using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Vector3 input;
    [SerializeField] Transform cameraObj;
    [SerializeField] float rotSpeed = 5f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] CharacterController charController;
    PlayerGrind grindScript;

    private void Start()
    {
        cameraObj = FindObjectOfType<Camera>().transform;
        charController = GetComponent<CharacterController>();
        grindScript = GetComponent<PlayerGrind>();
    }
    public void HandleMovement(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        input.x = rawInput.x;
        input.z = rawInput.y;
    }

    private void Update()
    {
        if (!grindScript.onRail)
        {
            //HandleRotation();
            Vector3 forward = cameraObj.forward;
            forward.y = 0;
            transform.forward = forward;
            Vector3 movement = input * (moveSpeed * Time.deltaTime);
            charController.Move(transform.TransformDirection(movement));
        }
    }

    void HandleRotation()
    {
        Vector3 targetDir = Vector3.zero;
        targetDir = cameraObj.forward * input.z;
        targetDir += cameraObj.right * input.x;
        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        Quaternion targetRot = Quaternion.LookRotation(targetDir);
        Quaternion playerRot = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        transform.rotation = playerRot;
    }
}
//This a very basic character controller, you do not need to use it.