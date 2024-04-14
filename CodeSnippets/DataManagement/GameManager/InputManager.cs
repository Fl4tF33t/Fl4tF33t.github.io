using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    //base input touch systemn for mobile devices
    public event Action<Vector2> OnTouchTap;
    public event Action<Vector2> OnTouchInput;

    public event Action<Vector2> OnTouchPressStarted;
    public event Action<Vector2> OnTouchPressCanceled;

    //public delegate void TouchTapEvent();
    //public event TouchTapEvent OnTouchTap;
    //public delegate void TouchHoldEvent();
    //public event TouchHoldEvent OnTouchHold;
    //public delegate void TouchPressEvent();
    //public event TouchPressEvent OnTouchPress;

    PlayerInputActions playerInputActions;
    EventSystem eventSystem;

    protected override void Awake()
    {
        base.Awake();
        eventSystem = EventSystem.current;
        playerInputActions = new PlayerInputActions(); 
        playerInputActions.Player.Enable();
    }

    private void Start()
    {
        playerInputActions.Player.TouchTap.performed += TouchTap_performed;
        //playerInputActions.Player.TouchHold.performed += TouchHold_performed;
        playerInputActions.Player.TouchInput.performed += TouchInput_performed;
        playerInputActions.Player.TouchPress.started += TouchPress_started;
        playerInputActions.Player.TouchPress.performed += TouchPress_performed;
        playerInputActions.Player.TouchPress.canceled += TouchPress_canceled;
    }

    private void TouchInput_performed(InputAction.CallbackContext obj)
    {
        //Debug.Log("Touch input performed");
        Vector2 touchPosition = playerInputActions.Player.TouchPosition.ReadValue<Vector2>();
        //Debug.Log(touchPosition);
        OnTouchInput?.Invoke(touchPosition);
    }

    private void TouchPress_performed(InputAction.CallbackContext obj)
    {
        //Vector2 touchPosition = playerInputActions.Player.TouchPosition.ReadValue<Vector2>();
        //Debug.Log("Press " + obj.phase);
        //OnTouchPress?.Invoke(touchPosition);
    }

    private void TouchPress_canceled(InputAction.CallbackContext obj)
    {
        Vector2 touchPosition = playerInputActions.Player.TouchPosition.ReadValue<Vector2>();
        //Debug.Log("Canel;e");
        OnTouchPressCanceled?.Invoke(touchPosition);
    }

    private void TouchPress_started(InputAction.CallbackContext obj)
    {
        Vector2 touchPosition = playerInputActions.Player.TouchPosition.ReadValue<Vector2>();
        //Debug.Log(touchPosition);
        OnTouchPressStarted?.Invoke(touchPosition);
    }

    private void TouchHold_performed(InputAction.CallbackContext obj)
    {
        //Debug.Log("Hold " + obj.phase);
    }

    private void TouchTap_performed(InputAction.CallbackContext obj)
    {
        //Debug.Log("Tap " + obj.phase);

        // Get the touch position
        Vector2 touchPosition = playerInputActions.Player.TouchPosition.ReadValue<Vector2>();

        // Perform a raycast from the touch position
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = touchPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        // Check if any UI element was hit by the raycast
        bool isPointerOverUI = results.Count > 0;

        // If not over a UI element, invoke the OnTouchTap event
        if (!isPointerOverUI)
        {
            Debug.Log("Tap performed");
            OnTouchTap?.Invoke(touchPosition);
        }
    }
}
