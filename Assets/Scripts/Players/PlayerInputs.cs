using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Players
{
    public class PlayerInputs : MonoBehaviour
    {
        public Vector3 leftMoveStick;
        public Vector3 rightMoveStick;

        void OnLeftMove(InputValue value)
        {
            var axis = value.Get<Vector2>();

            leftMoveStick = new Vector3(axis.x, 0, axis.y);
        }

        void OnRightMove(InputValue value)
        {
            var axis = value.Get<Vector2>();

            rightMoveStick = new Vector3(axis.x, 0, axis.y);
        }
    }
}
