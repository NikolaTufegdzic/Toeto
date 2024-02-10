using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
      private int hitPoints=1;
      private int xp = 10;
      private int imageId = 0;
      BulletTarget bulletTarget;
 
      public void GotHit(int damage){
         hitPoints -= damage;
         switch(hitPoints){
            case 1:
               SetBlockImage(BlockGenerator.Instance.GetBlockSprite(0),0);
               SoundController.Instance.PlaySoundThud(transform.position, 0.6f);
            break;
            case 2:
               SetBlockImage(BlockGenerator.Instance.GetBlockSprite(1),1);
               SoundController.Instance.PlaySoundThud(transform.position , 0.8f);
            break;
            case 3:
               SetBlockImage(BlockGenerator.Instance.GetBlockSprite(imageId-damage*6),imageId-damage*6);
               SoundController.Instance.PlaySoundThud(transform.position, 1f);
            break;
            default:{ 
               float randValue = Random.value;
               if(randValue<=GameManager.Instance.GetChance()){
                  DropManager.Instance.DropCreator(gameObject.transform.position,Quaternion.identity);
               }
               if(GameManager.Instance.GetRedirectLuck()){
                  for (int i = 0; i < GameManager.Instance.GetLuckyRedirectDrops(); i++)
                  {
                     DropManager.Instance.DropCreator(gameObject.transform.position,Quaternion.identity);
                  }
                  GameManager.Instance.SetRedirectLuck(false);
               }
               GameManager.Instance.AddXp(xp);
               if(!GameManager.Instance.GetSuperNothingUsed()){
                  SoundController.Instance.PlaySoundPop(transform.position);
               }
               Destroy(bulletTarget.gameObject);
               Destroy(gameObject); 
               GameManager.Instance.BlockPopped();
            }
            break;
         }
      }
      public int GetHitPoints(){
         return hitPoints;
      }
      public void ReducehitPoints(int amount){
         hitPoints -= amount;
      }
      public void SetBlockImage(Sprite blockImage, int spriteId){
          GetComponentInChildren<SpriteRenderer>().sprite = blockImage;
          imageId = spriteId;
      }
      public void SetBlockHP(int hp){
         hitPoints = hp;
      }
      public void Rotate(int rotation){
         transform.Rotate(Vector3.forward, rotation);
      }
      public void SetBulletTarget(BulletTarget newBulletTarget){
         bulletTarget = newBulletTarget;
      }
      public BulletTarget GetBulletTarget(){
         return bulletTarget;
      }
}
