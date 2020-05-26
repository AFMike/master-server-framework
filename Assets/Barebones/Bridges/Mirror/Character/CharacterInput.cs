using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barebones.Bridges.Mirror.Character
{
    public class CharacterInput : CharacterBehaviour
    {
        private Vector2 mouseInput = new Vector2();
        private Vector2 movementInput = new Vector2();

        public virtual Vector2 MouseInput
        {
            get
            {
                mouseInput.x = Input.GetAxis("Mouse X");
                mouseInput.y = Input.GetAxis("Mouse Y");
                return mouseInput;
            }
        }

        public virtual Vector2 MovementInput
        {
            get
            {
                movementInput.x = Input.GetAxis("Horizontal");
                movementInput.y = Input.GetAxis("Vertical");
                return movementInput;
            }
        }

        public virtual bool JumpInput => Input.GetButton("Jump");
        public virtual bool RunInput => Input.GetKey(KeyCode.LeftShift);
    }
}