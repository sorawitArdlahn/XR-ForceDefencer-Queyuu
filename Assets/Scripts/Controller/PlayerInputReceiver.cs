using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
{
    private Vector3 _movement;
    private Vector2 _lookPosition;
    private bool _isJumping;
    private bool _isDashing;
    private bool _isFlying;
    private bool _isFire;
    private bool _isSprinting;
    private bool _isDriving = false;
    private bool _isLockOn = false;

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
        _lookPosition = value.Get<Vector2>();
    }

    private void OnFly(InputValue value)
    {
        _isFlying = value.isPressed;
    }

    private void OnDash(InputValue value)
    {
        _isDashing = value.isPressed;
    }

    private void OnFire(InputValue value)
    {
        _isFire = value.isPressed;
    }

    private void OnSprint(InputValue value)
    {
        _isSprinting = value.isPressed;
    }

    private void OnDrive(InputValue value)
    {
        _isDriving = !_isDriving;
    }
    
    private void OnLockOn(InputValue value)
    {
        _isLockOn = !_isLockOn;
    }

    public Vector3 Movement
    {
        get => _movement;
        set => _movement = value;
    }

    public Vector2 LookPosition
    {
        get => _lookPosition;
        set => _lookPosition = value;
    }

    public bool IsJumping
    {
        get => _isJumping;
        set => _isJumping = value;
    }

    public bool IsDashing
    {
        get => _isDashing;
        set => _isDashing = value;
    }

    public bool IsFlying
    {
        get => _isFlying;
        set => _isFlying = value;
    }

    public bool IsFire
    {
        get => _isFire;
        set => _isFire = value;
    }

    public bool IsSprinting
    {
        get => _isSprinting;
        set => _isSprinting = value;
    }

    public bool IsDriving
    {
        get => _isDriving;
        set => _isDriving = value;
    }

    public bool IsLockOn
    {
        get => _isLockOn;
        set => _isLockOn = value;
    }
}
