using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter, IkitchenObjectParent{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
     public override void Interact(Player player){
        if(!player.HasKitchenObject()){
            //player is not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            //OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);    
        }
        else{
            //player is carrying smth
            
        }
        
     }

}
