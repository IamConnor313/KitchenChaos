using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter{
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player){
        if(!HasKitchenObject()){
            //There is no KitchenObject here
            if(player.HasKitchenObject()){
                //Player is carrying smth
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else{
                //Player is not carrying anything

            }
        } 
        else{
            //There is a KitchenObject here
            if(player.HasKitchenObject()){
                //Player carrying smth
            }
            else{
                //Player not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }

        }
    }

    
}
