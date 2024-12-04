using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using View;

namespace Controller
{
    public class PlayerMovementCtrl : MonoBehaviour
    {
        [Header("Body")]
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravity;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform bodyPart;
        
        private CharacterController _playerController;
        private PlayerInputReceiver _playerInputReceiver;
        
        private Vector3 _movementVector;
        private Vector3 _velocity;
        [SerializeField] private bool _isGrounded;
        
        [Header("Head")]
        [SerializeField] Transform head;
        [SerializeField] private float mouseX;
        [SerializeField] private float mouseY;
        
        [Header("Combat")]
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireRate;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed;
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _playerController = GetComponent<CharacterController>();
            _playerInputReceiver = GameObject.Find("==GameManager==/PlayerInputManager").GetComponent<PlayerInputReceiver>();
        }

        private void Update()
        {
            Move();
            Jump();
            MoveHead();
            Fly();
            StartCoroutine(Dash());
            StartCoroutine(Fire());
        }

        private void Move()
        {
            _movementVector = _playerInputReceiver.GetMovement();
            _playerController.Move(head.rotation * _movementVector * (Time.deltaTime * movementSpeed));
            transform.rotation = Quaternion.Euler(0, head.rotation.y, 0);
        }
        
        private void Jump()
        {
            _isGrounded = Physics.CheckSphere(groundChecker.position, 1f, groundLayer);
            
            if (_isGrounded)
            {
                if (_playerInputReceiver.IsJumping())
                {
                    _velocity.y = Mathf.Sqrt(2 * jumpHeight * -gravity);
                    _playerInputReceiver.ResetJumping();
                }
            }
            else if (!_isGrounded && _playerInputReceiver.IsFlying())
            {
                _velocity.y = Mathf.Sqrt(2 * jumpHeight * -gravity);
            }
            else
            {
                _velocity.y += gravity * Time.deltaTime * 2;
            }
            
            _playerController.Move(_velocity * Time.deltaTime);
        }

        private void MoveHead()
        {
            // mouseX += _playerInputReceiver.GetMousePosition().normalized.x;
            // mouseY += _playerInputReceiver.GetMousePosition().normalized.y;
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");
            if (mouseY > 40) mouseY = 40;
            if (mouseY < -40) mouseY = -40;
            
            head.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
        }

        private void Fly()
        {
            if (!_isGrounded && _playerInputReceiver.IsFlying())
            {
                _velocity.y = Mathf.Sqrt(2 * jumpHeight * -gravity);
            }
            
            _playerController.Move(_velocity * Time.deltaTime);
        }

        private IEnumerator Dash()
        {
            if (_playerInputReceiver.IsDashing())
            {
                _playerController.Move(head.rotation * Vector3.forward * (Time.deltaTime * 120));
                yield return new WaitForSeconds(0.1f);
                _playerInputReceiver.ResetDashing();
            }
        }

        private IEnumerator Fire()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    Debug.Log(hit.collider.name + " " +  hit.point);
                    if (_playerInputReceiver.IsFireing())
                    {
                        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                        bullet.GetComponent<Rigidbody>().MovePosition(hit.point);
                        _playerInputReceiver.ResetFireing();
                    }
                }
            }
            yield return new WaitForSeconds(fireRate);
        }
    }
}
