using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour{


    public event EventHandler OnInteractAction;
    private PlayerInputAction playerInputActions;
    private void Awake(){
        playerInputActions = new PlayerInputAction();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;

    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        OnInteractAction?.Invoke(this, EventArgs.Empty);
        //Question mark evaluates if left side of sign is null and if not then allows right side to run. 
    }

    public Vector2 GetMovementVectorNormalized(){
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        //Configure controls for other input types like controllers

        inputVector = inputVector.normalized;

        return inputVector;
    }
    
}
