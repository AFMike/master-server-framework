using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror.Character
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterInput), typeof(CharacterController))]
    public class CharacterTdMovement : CharacterMovement
    {
        [Header("Components"), SerializeField]
        protected CharacterTdLook lookController;

        [Header("Rotation Settings"), SerializeField, Range(5f, 20f)]
        private float rotationSmoothTime = 5f;

        /// <summary>
        /// Направление, в которое постоянно должен смотреть игрок
        /// </summary>
        private Quaternion playerTargetDirectionAngle;

        protected override void UpdateMovement()
        {
            if (characterController.isGrounded)
            {
                var aimDirection = lookController.AimDirection();

                // Если персонаж двигается но не находится в режиме боя
                if (inputController.IsMoving() && !inputController.IsArmed())
                {
                    // Вычисляем новый угол поворота игрока
                    Vector3 t_currentDirection = inputController.MovementAxisDirection();

                    if (!t_currentDirection.Equals(Vector3.zero))
                    {
                        playerTargetDirectionAngle = Quaternion.LookRotation(t_currentDirection) * lookController.GetRotation();
                    }
                }
                // Если персонаж двигается и находится в режиме боя
                else if (inputController.IsMoving() && inputController.IsArmed())
                {
                    playerTargetDirectionAngle = Quaternion.LookRotation(new Vector3(aimDirection.x, 0f, aimDirection.z));
                }
                // Если персонаж не двигается и находится в режиме боя
                else if (!inputController.IsMoving() && inputController.IsArmed())
                {
                    playerTargetDirectionAngle = Quaternion.LookRotation(new Vector3(aimDirection.x, 0f, aimDirection.z));
                }

                // Плавно поворачиваем игрока в направлении целевого угла поворота
                transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetDirectionAngle, Time.deltaTime * rotationSmoothTime);

                // Let's calculate input direction
                var inputAxisAngle = inputController.MovementAxisDirection().Equals(Vector3.zero) ? Vector3.zero : Quaternion.LookRotation(inputController.MovementAxisDirection()).eulerAngles;
                var compositeAngle = inputAxisAngle - transform.eulerAngles;

                calculatedInputDirection = Quaternion.Euler(compositeAngle) * lookController.GetRotation() * transform.forward * inputController.MovementAxisMagnitude();

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

                if (inputController.IsJump() && IsJumpAvailable)
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
