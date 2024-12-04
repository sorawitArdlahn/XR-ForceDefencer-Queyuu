using System;
using UnityEngine;
using View;

namespace Controller
{
    public class PlayerCameraCtrl : MonoBehaviour
    {
        [SerializeField] private float sensX;
        [SerializeField] private float sensY;
      
        [SerializeField] private Transform orientation;
        
        private float _xRotation;
        private float _yRotation;
        private PlayerInputReceiver _playerInputReceiver;
        private void Awake()
        {
            _playerInputReceiver = GameObject.Find("GameManager/PlayerInputManager").GetComponent<PlayerInputReceiver>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // float mouseX = (float)_playerInputReceiver.GetMousePosition() * Time.deltaTime * sensX;
            // float mouseY = (float)_playerInputReceiver.GetMousePosition() * Time.deltaTime * sensY;
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
            
            _yRotation += mouseX;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
        
        
    }
}
