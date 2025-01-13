using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> OnMovementEvent;
    public event Action<bool> OnJumpEvent;
    public event Action<bool> OnAttackEvent;

    private Controls controls;

    private void Awake()
    {
        Debug.Log("gere");
    }

    private void OnEnable()
    {
        if(controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }

        controls.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 movementDirection = context.ReadValue<Vector2>();
        OnMovementEvent?.Invoke(movementDirection);
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnJumpEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            OnJumpEvent?.Invoke(false);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            OnAttackEvent?.Invoke(true);
        }
        else
        {
            OnAttackEvent?.Invoke(false);
        }
    }
}
