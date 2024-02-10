using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public string layerToIgnore1 = "BlockGenerator";
    public string layerToIgnore2 = "Block";
    public string layerToIgnore3 = "Target"; 
    public DropTypesEnumSO.DropType dropType; 
    [SerializeField] Sprite[] dropImages;
  
    public void RandomDrop(){
        DropTypesEnumSO.DropType[] enumValues = (DropTypesEnumSO.DropType[])Enum.GetValues(typeof(DropTypesEnumSO.DropType));
        int randomIndex = UnityEngine.Random.Range(0, enumValues.Length);
        dropType = enumValues[randomIndex];
        if(GizmoManager.Instance.hasDropPhaser){
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(layerToIgnore1));
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(layerToIgnore2));
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(layerToIgnore3));
        }
        
        GetComponentInChildren<SpriteRenderer>().sprite = dropImages[randomIndex];
    }
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.name == "WallBottom"){
            Destroy(gameObject);
        }
    }
}
