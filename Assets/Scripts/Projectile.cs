using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Projectile : MonoBehaviour
{
    private Vector3 projectileTarget;
    private int projectilePower=1;
    private float projectileSpeed = 10f;
    private void Update()
    {
        if(GameManager.Instance.IsGameOver()){
            gameObject.SetActive(false);
        }
         transform.position += projectileTarget*projectileSpeed*Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(projectilePower<=0){
             Destroy(gameObject);
        }
        bool bounce = false;
        if(collision.gameObject.layer==11 || collision.gameObject.layer==12){
            Destroy(gameObject);
        } else {
                if (bounce){
                projectileTarget = Vector3.Reflect(projectileTarget.normalized,collision.contacts[0].normal);
            } else{
                Block block = null;
                Debug.Log(collision.gameObject.name);
                BulletTarget bulletTarget = collision.gameObject.GetComponent<BulletTarget>();
                if (bulletTarget != null){
                    block = bulletTarget.GetBlock();
                    Debug.Log("pogodak");
                } 
                if (block != null){
                    int blockHP = block.GetHitPoints();
                   if(projectilePower>0){ 
                        block.GotHit(projectilePower);
                    }
                    projectilePower-=blockHP;
                    
                }
                Drop drop = collision.gameObject.GetComponent<Drop>();
                if(drop != null){
                    Destroy(collision.gameObject);
                    GameManager.Instance.DropHit();
                    projectilePower--;
                }
                if(projectilePower<=0){
                        Destroy(gameObject);
                    
                }

                PolygonCollider2D bulletCollider = GetComponent<PolygonCollider2D>();
                bulletCollider.enabled = false;
                StartCoroutine(EnableBallCollider(0.000002f, bulletCollider));
            }

        }
        
    }
    System.Collections.IEnumerator EnableBallCollider(float delay, PolygonCollider2D objectColider)
    {
        yield return new WaitForSeconds(delay);

        if(objectColider!=null){
            objectColider.enabled = true;
                 
        }
    }

    public void SetTarget(Vector3 target){
        projectileTarget= target;
    }

    public void SetPower(int power){
        projectilePower = power;
    }
    public void SetSpeed(float speed){
        projectileSpeed = speed;
    }
    public void AddPower(int power){
        projectilePower += power;
    }
}
