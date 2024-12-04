using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace View
{
    public class PlayerInputReceiver : MonoBehaviour
    {
        private Vector3 _movement;
        private Vector2 _mousePosition;
        [SerializeField] private bool _isJumping;
        [SerializeField] private bool _isDashing;
        private bool _isFlying;
        private bool _isFireing;
        
        private void OnMovement(InputValue value)
        {
            _movement = value.Get<Vector3>();
        }

        private void OnJump(InputValue value)
        {
            _isJumping = value.isPressed;
        }

        private void OnLook(InputValue value)
        {
            _mousePosition = value.Get<Vector2>();
        }

        private void OnFly(InputValue value)
        {
            _isFlying = value.isPressed;
        }

        private void OnCancelFly(InputValue value)
        {
            _isFlying = false;
        }

        private void OnDash(InputValue value)
        {
            _isDashing = value.isPressed;
        }

        private void OnFire(InputValue value)
        {
            _isFireing = value.isPressed;
        }

        public Vector3 GetMovement()
        {
            return _movement;
        }

        public Vector2 GetMousePosition()
        {
            return _mousePosition;
        }

        public bool IsJumping()
        {
            return _isJumping;
        }

        public bool IsFlying()
        {
            return _isFlying;
        }

        public bool IsDashing()
        {
            return _isDashing;
        }

        public bool IsFireing()
        {
            return _isFireing;
        }
        
        public void ResetJumping()
        {
            _isJumping = false;
        }

        public void ResetDashing()
        {
            _isDashing = false;
        }

        public void ResetFireing()
        {
            _isFireing = false;
        }
    }
}