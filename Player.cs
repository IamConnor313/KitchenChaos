// using System.Collections;
// using System.Collections.Generic;
// using System.Numerics;
//using System.Numerics;
using System;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class Player : MonoBehaviour{

    public static Player Instance{ get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public ClearCounter selectedCounter; 
    }

    [SerializeField] private float moveSpeed = 7f; //private but can be accessed thru Unity as editable field 
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;

    private void Awake(){
        if(Instance != null){
            Debug.LogError("There is more than 1 player instance");
        }
        Instance = this; 
    }

    private void Start(){
        gameInput.OnInteractAction += GameInput_OnInteractAction;

    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e ){
       if(selectedCounter != null){
          selectedCounter.Interact();
       }
    }
    private void Update(){
        HandleMovement();
        HandleInteractions();


    }

    public bool IsWalking(){
        return isWalking;

    }

    private void HandleInteractions(){
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0.0f, inputVector.y);

        if(moveDir != Vector3.zero){
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        RaycastHit raycastHit; 
        if(Physics.Raycast(transform.position, lastInteractDir, out raycastHit, interactDistance, countersLayerMask)){ //More data given when colliding with object
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)){
                //Has ClearCounter
                if(clearCounter != selectedCounter){
                    SetSelectedCounter(clearCounter);

                }
            }
            else{
                SetSelectedCounter(null);

            
            }
        }
        else{
            SetSelectedCounter(null);
        }
    }
        
    

    private void HandleMovement(){
        
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0.0f, inputVector.y);
        
        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance); //Point 1 is bottom to Point 2 is top
        
        if(!canMove){
            //Cannot move towards direction 
            
            //Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance); //Point 1 is bottom to Point 2 is top

            if(canMove){
                //Can move only on X
                moveDir = moveDirX;
            } 
            else{
                //Can't move on X
                //Try to move on Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance); //Point 1 is bottom to Point 2 is top
                if(canMove){
                    moveDir = moveDirZ;
                }
                else{
                    //Can't move at all - idk why CM put this in here 
                }

            }

        }
        if(canMove){
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f; 
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
       

    }

    private void SetSelectedCounter(ClearCounter selectedCounter){
        this.selectedCounter = selectedCounter; 
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter 
        });
    }
}
