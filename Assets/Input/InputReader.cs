using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> OnMovementEvent;
    public event Action<bool> OnJumpEvent;
    public event Action<bool> OnAttackEvent;

    private Controls controls;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
            Debug.Log("InputReader initialized and callbacks set.");
        }

        controls.Enable();
        Debug.Log("Controls enabled.");

        LoadBindings();
    }

    private void OnDisable()
    {
        controls?.Disable();
        Debug.Log("Controls disabled.");
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 movementDirection = context.ReadValue<Vector2>();
        OnMovementEvent?.Invoke(movementDirection);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnJumpEvent?.Invoke(false);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttackEvent?.Invoke(true);
        }
        else
        {
            OnAttackEvent?.Invoke(false);
        }
    }

    public InputAction JumpAction => controls.Player.Jump;
    public InputAction AttackAction => controls.Player.Attack;
    public InputAction MoveAction => controls.Player.Movement;

    public void StartRebind(string actionName, string bindingName = "", Action<string> onComplete = null)
    {
        InputAction actionToRebind = actionName switch
        {
            "Jump" => JumpAction,
            "Attack" => AttackAction,
            "Move" => MoveAction,
            _ => null
        };

        if (actionToRebind == null)
        {
            Debug.LogError($"[Rebind] No action found with the name: {actionName}. Cannot rebind.");
            return;
        }

        int bindingIndex = -1;

        if (string.IsNullOrEmpty(bindingName))
        {
            // Non-composite
            bindingIndex = 0;
        }
        else
        {
            // Composite part (e.g., "up", "down")
            for (int i = 0; i < actionToRebind.bindings.Count; i++)
            {
                if (actionToRebind.bindings[i].isPartOfComposite &&
                    actionToRebind.bindings[i].name.ToLower() == bindingName.ToLower())
                {
                    bindingIndex = i;
                    break;
                }
            }

            if (bindingIndex == -1)
            {
                Debug.LogError($"[Rebind] Binding '{bindingName}' not found in action '{actionName}'.");
                return;
            }
        }

        Debug.Log($"[Rebind] Starting rebind for '{actionName}/{bindingName}'. Press a new key... (ESC to cancel)");

        // Disable the action before rebinding
        actionToRebind.Disable();

        actionToRebind.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable(); // Re-enable after rebinding
                SaveBindings();

                string newBinding = actionToRebind.bindings[bindingIndex].effectivePath;
                Debug.Log($"[Rebind] Rebinding complete! '{actionName}/{bindingName}' is now bound to: {newBinding}");
                onComplete?.Invoke(newBinding);
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                actionToRebind.Enable(); // Re-enable even if canceled
                Debug.Log($"[Rebind] Rebinding for '{actionName}/{bindingName}' was canceled.");
            })
            .Start();
    }



    public void SaveBindings()
    {
        try
        {
            string rebinds = controls.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString("rebinds", rebinds);
            PlayerPrefs.Save();
            Debug.Log("[Rebind] Bindings saved successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Rebind] Failed to save bindings: {ex.Message}");
        }
    }

    public void LoadBindings()
    {
        if (PlayerPrefs.HasKey("rebinds"))
        {
            try
            {
                string rebinds = PlayerPrefs.GetString("rebinds");
                controls.LoadBindingOverridesFromJson(rebinds);
                Debug.Log("[Rebind] Bindings loaded successfully from PlayerPrefs.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Rebind] Failed to load bindings: {ex.Message}");
            }
        }
        else
        {
            Debug.Log("[Rebind] No saved bindings found in PlayerPrefs.");
        }
    }

    public string GetBindingDisplayName(string actionName, string bindingName = "")
    {
        InputAction action = actionName switch
        {
            "Jump" => JumpAction,
            "Attack" => AttackAction,
            "Move" => MoveAction,
            _ => null
        };

        if (action == null)
            return "[N/A]";

        int bindingIndex = -1;

        if (string.IsNullOrEmpty(bindingName))
        {
            bindingIndex = 0;
        }
        else
        {
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (action.bindings[i].isPartOfComposite &&
                    action.bindings[i].name.ToLower() == bindingName.ToLower())
                {
                    bindingIndex = i;
                    break;
                }
            }
        }

        if (bindingIndex >= 0 && bindingIndex < action.bindings.Count)
        {
            return InputControlPath.ToHumanReadableString(
                action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );
        }

        return "[Not Bound]";
    }

    public void ResetBindings()
    {
        controls.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey("rebinds");
        PlayerPrefs.Save();
        Debug.Log("[Rebind] All bindings reset to default.");
    }
}
