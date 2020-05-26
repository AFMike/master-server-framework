using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror.Character
{
    [RequireComponent(typeof(CharacterInput), typeof(CharacterController))]
    public class CharacterMovement : CharacterBehaviour
    {
        #region INSPECTOR

        [Header("Gravity Settings"), SerializeField]
        protected float gravityMultiplier = 3f;
        [SerializeField, Range(0, 100)]
        protected float stickToGroundPower = 5f;

        [Header("Movement Settings"), SerializeField, Range(0, 100)]
        protected float walkSpeed = 5f;
        [SerializeField, Range(0, 100)]
        protected float runSpeed = 10f;

        [Header("Jump Settings"), SerializeField]
        protected bool jumpIsAllowed = true;
        [SerializeField, Range(0, 100)]
        protected float jumpPower = 8f;
        [SerializeField, Range(0, 100)]
        protected float jumpRate = 1f;

        [Header("Components"), SerializeField]
        protected CharacterInput inputController;
        [SerializeField]
        protected CharacterController characterController;

        #endregion

        /// <summary>
        /// Current calculated movement direction
        /// </summary>
        protected Vector3 calculatedMovementDirection = new Vector3();
        protected Vector3 calculatedInputDirection = new Vector3();
        protected float nextJumpTime = 0f;

        /// <summary>
        /// Check if this behaviour is ready
        /// </summary>
        public override bool IsReady => inputController && characterController;

        /// <summary>
        /// Speed of the character
        /// </summary>
        public float CurrentMovementSpeed { get; protected set; }

        /// <summary>
        /// Check if jumping is available for the character
        /// </summary>
        public bool IsJumpAvailable { get; protected set; }

        /// <summary>
        /// If character is currently walking
        /// </summary>
        public bool IsWalking { get; protected set; }

        /// <summary>
        /// If character is currently running
        /// </summary>
        public bool IsRunning { get; protected set; }

        protected void Update()
        {
            if (isLocalPlayer && IsReady)
            {
                UpdateJumpAvailability();
                UpdateMovementStates();
            }
        }

        protected void FixedUpdate()
        {
            if (isLocalPlayer && IsReady)
            {
                UpdateMovement();
            }
        }

        public override void OnStartLocalPlayer()
        {
            SetupInputController();
            SetupCharacterController();
        }

        private void SetupCharacterController()
        {
            if (characterController == null)
            {
                characterController = GetComponentInChildren<CharacterController>();
            }
        }

        /// <summary>
        /// Setup input controller
        /// </summary>
        protected virtual void SetupInputController()
        {
            if (inputController == null)
            {
                inputController = GetComponentInChildren<CharacterInput>();
            }
        }

        protected virtual void UpdateJumpAvailability()
        {
            if (jumpIsAllowed)
            {
                IsJumpAvailable = Time.time >= nextJumpTime;
            }
            else
            {
                IsJumpAvailable = jumpIsAllowed;
            }
        }

        protected virtual void UpdateMovementStates()
        {
            IsWalking = inputController.MovementInput.magnitude > 0.1f;
            IsRunning = IsWalking && inputController.RunInput;
        }

        protected virtual void UpdateMovement()
        {
            if (characterController.isGrounded)
            {
                calculatedInputDirection = transform.forward * inputController.MovementInput.y + transform.right * inputController.MovementInput.x;

                if (IsRunning)
                {
                    CurrentMovementSpeed = runSpeed;
                }
                else if (IsWalking)
                {
                    CurrentMovementSpeed = walkSpeed;
                }

                calculatedMovementDirection.y = -stickToGroundPower;
                calculatedMovementDirection.x = calculatedInputDirection.x * CurrentMovementSpeed;
                calculatedMovementDirection.z = calculatedInputDirection.z * CurrentMovementSpeed;

                if (inputController.JumpInput && IsJumpAvailable)
                {
                    calculatedMovementDirection.y = jumpPower;
                    nextJumpTime = Time.time + jumpRate;
                }
            }
            else
            {
                calculatedMovementDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }

            characterController.Move(calculatedMovementDirection * Time.fixedDeltaTime);
        }
    }
}
