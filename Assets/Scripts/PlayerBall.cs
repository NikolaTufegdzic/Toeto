using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public static PlayerBall Instance { get; private set;}
    public event EventHandler BallDestroyed;
    public event EventHandler BallCaught;
    private float ballSpeed = 3.5f;
    private int ballPower = 1;
    private Vector3 ballDirection;
    private Vector3 ballPosition;
    public Projectile projectile;
    private float ammoTimer = 0f;
    private float redirectTimer = 0f;
    private float redirectCooldown = 25f;
    private bool abilityUsed = false;
    private float replenishTime = 8f;
    private Vector3 forward = new Vector3(0,1,0);
    private Vector3 rotator = new Vector3(0,0,-1);
    private int ammoMax = 1;
    private int ammo = 1;
    private int bulletPower = 1;
    private float bulletSpeed = 6f;
    private void Awake(){
        Instance = this;
        BallDestroyed += PlayerBall_BallDestroyed;
        BallCaught += PlayerBall_BallCaught;
    }
    
    private void Start()
    {
        ballDirection = new Vector3(0,1,0); 
        GameInput.Instance.PointFirst += GameInput_PointFirst; 
        GameInput.Instance.PointSecond += GameInput_PointSecond;  
    }
    private void OnDestroy(){
        GameInput.Instance.PointFirst -= GameInput_PointFirst; 
        GameInput.Instance.PointSecond -= GameInput_PointSecond;  
    }
    private void Update()
    {   if(ballPosition.x<-9||ballPosition.x>6||ballPosition.y<-5.5f||ballPosition.y>5.2){
            BallCaught?.Invoke(this,EventArgs.Empty);
        }
        if(GameManager.Instance.IsGameOver()){
            gameObject.SetActive(false);
        }
        if(GameManager.Instance.IsBallLaunching()||GameManager.Instance.IsGizmoShop()||GameManager.Instance.IsCredits()||GameManager.Instance.IsGameWon()){
            transform.position =  PlayerPad.Instance.transform.position + new Vector3(0,0.3f,0);
            if(GizmoManager.Instance.hasLaunchAim){
                if (GizmoManager.Instance.hasFreeAim){
                    ballDirection = BallCursorVector();
                } else {
                    ballDirection = new Vector3(0,1,0);
                } 
            } else {
                ballDirection = new Vector3( UnityEngine.Random.Range(-1f,1f),UnityEngine.Random.Range(-1f,1f),0 );
            }
            ballDirection = ballDirection.normalized;
            ballPosition = transform.position;
        } else {
             transform.position += ballDirection*ballSpeed*Time.deltaTime;
             ballPosition = transform.position;
        }
        
        AmmoReplenish();
        RedirectCooldown();
    }
    public void RefreshCD(){
        ammo = ammoMax;
        redirectTimer = 0;
        abilityUsed = false;
    }
    private void RedirectCooldown(){
        if(abilityUsed){
                if(redirectTimer<redirectCooldown){
                redirectTimer+= Time.deltaTime;
            } else {
                redirectTimer = 0;
                abilityUsed = false;
            }
        }
       
    }
    private void AmmoReplenish(){
        if(ammo<ammoMax){
            if(ammoTimer<replenishTime){
                ammoTimer+= Time.deltaTime;
            }else{
                ammo++;
                ammoTimer = 0;
            }
        } else{
            ammoTimer = 0;
        }
    }
    private void PlayerBall_BallDestroyed(object sender, EventArgs e){
    }
    private void GameInput_PointFirst(object sender, EventArgs e){
        if(!GameManager.Instance.IsGamePaused()&&!GameSceneUI.Instance.IsMouseOverUI()&&!GameManager.Instance.IsChoosingLvlUp()){
            if(ammo>0){
                ammo--;
                BulletCreator();
                if(GizmoManager.Instance.hasTwinBullet){
                    TwinBullet();
                }
            }
        }
    }
    private void BulletCreator(){
        Quaternion spawnRotation= Quaternion.Euler(0,0,Vector3.SignedAngle(PlayerBall.Instance.BallCursorVector(), forward,rotator));
        Projectile newProjectile = Instantiate(projectile, ballPosition, spawnRotation);
        newProjectile.transform.SetParent(GameManager.Instance.GetStage().transform);
        Vector3 bulletTarget = BallCursorVector().normalized;
        bulletTarget.z = 0;
        newProjectile.SetTarget(bulletTarget);
        newProjectile.SetPower(bulletPower);
        newProjectile.SetSpeed(bulletSpeed);
        SoundController.Instance.PlaySoundShoot(ballPosition);
    }
    private void TwinBullet(){  
      StartCoroutine(ShootAnotherBulletAfterDelay(0.1f));
    }

    System.Collections.IEnumerator ShootAnotherBulletAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        BulletCreator();
    }
     private void GameInput_PointSecond(object sender, EventArgs e){
        if(!GameManager.Instance.IsGamePaused()&&!GameSceneUI.Instance.IsMouseOverUI()&&GameManager.Instance.IsGamePlaying()){
             if(!abilityUsed){
                ballDirection=BallCursorVector().normalized;
                abilityUsed = true;
                if(GameManager.Instance.GetLuckyRedirectDrops()>0){
                    GameManager.Instance.SetRedirectLuck(true);
                } 
                ballPower+=GameManager.Instance.GetRedirectPower();
                SoundController.Instance.PlaySoundRedirect(ballPosition);
             }
        }
       
       
    }
    private void PlayerBall_BallCaught(object sender, EventArgs e){
    }
    private void OnCollisionEnter2D(Collision2D collision){
        bool bounce = true;
        Block block = null; 
        ContactPoint2D[] contacts = collision.contacts;
        if(collision.gameObject.layer==3){
            foreach (ContactPoint2D contact in contacts)
            {

                Vector2 closestPoint = GetComponent<Collider2D>().ClosestPoint(contact.point);
                Transform hitObject = FindClosestChild(closestPoint);

                if (hitObject != null)
                {
                    block = hitObject.GetComponent<Block>();
                }
            }
        }
     
        
        if (block != null){
            int blockHP = block.GetHitPoints();
            if(ballPower > blockHP){
                bounce = false;
            }
            block.GotHit(ballPower);
            ballPower -= blockHP;
            if(ballPower<1){
                ballPower = 1;
            }
            
        }
        if (collision.gameObject.name == "WallBottom"){
            BallDestroyed?.Invoke(this,EventArgs.Empty);
        }
         if (collision.gameObject.name == "Walls"){
            BoxCollider2D wallColider = collision.gameObject.GetComponent<BoxCollider2D>();
            wallColider.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(0.002f, wallColider));
            SoundController.Instance.PlaySoundDeepThud(transform.position, 0.8f);
        }
        if ((collision.collider.gameObject != null) && (collision.collider.gameObject.name == "BallCatcher")){
            if(GizmoManager.Instance.hasBallCatcher){
                BallCaught?.Invoke(this,EventArgs.Empty);
                SoundController.Instance.PlaySoundCatch(new Vector3(0, 0, 1));
            }  
        }
        if (collision.gameObject.name == "PlayerPad"){
           SoundController.Instance.PlaySoundDeepThud(transform.position, 1.2f); 
        }
        if (bounce){
            ballDirection = Vector3.Reflect(ballDirection.normalized,collision.contacts[0].normal);
        }
        if(block!=null){
                CircleCollider2D ballCollider = GetComponent<CircleCollider2D>();
                ballCollider.enabled = false;
                StartCoroutine(EnableBallCollider(0.0002f, ballCollider));
            }
    }
    public Transform FindClosestChild(Vector2 point)
    {
        Transform closestChild = null;
        float closestDistance = float.MaxValue;
        Transform parentTransform = BlockGenerator.Instance.transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);

           float distance = Vector2.Distance(point, child.position);

            if (distance < closestDistance && child.gameObject.layer!=6 && child.gameObject.layer!=8 && child.gameObject.layer!=9)
            {
                closestDistance = distance;
                closestChild = child;
            }
        }
        return closestChild;
    }
    System.Collections.IEnumerator EnableColliderAfterDelay(float delay, BoxCollider2D objectColider)
    {
        yield return new WaitForSeconds(delay);
        if(objectColider!=null){
            objectColider.enabled = true;
        }
    }
    System.Collections.IEnumerator EnableBallCollider(float delay, CircleCollider2D objectColider)
    {
        yield return new WaitForSeconds(delay);

        if(objectColider!=null){
            objectColider.enabled = true;
                 
        }
    }

    public void FasterBall(){
        ballSpeed += 0.1f;
    }

    public void SlowerBall(){
        ballSpeed -= 0.1f;
    }

    public int GetBallPower(){
        return ballPower;
    }
    public void AddBallPower(int amount){
        ballPower +=amount;
    }
    public void AddBulletPower(int amount){
        bulletPower+=amount;
    }
    public int GetBulletPower(){
        return bulletPower;
    }
     public float GetBulletSpeed(){
        return bulletSpeed;
    }
    public Vector3 GetBallDirection(){
        return ballDirection;
    }
    public Vector3 GetBallPosition(){
        return ballPosition;
    }
    public Vector3 BallCursorVector(){
        return  MouseManager.Instance.mouseWorldPosition - transform.position;
    }

    public int GetAmmo(){
        return ammo;
    }
    public void SetAmmo(int amount){
       ammo = amount;
        if (ammo>ammoMax){
            ammo= ammoMax;
        } 
    }
    public void AddAmmo(int amount){
        ammo += amount;
        if (ammo>ammoMax){
            ammo= ammoMax;
        }
    }
    public int GetAmmoMax(){
        return ammoMax;
    }
      public float GetBallSpeed(){
        return ballSpeed;
    }
    public void AddAmmoMax(int amount){
        ammoMax += amount;
    }    
    public float GetFirstCD(){
        return replenishTime;
    }
    public float GetFirstTimer(){
        return ammoTimer;
    }
    public float GetSecondCD(){
        return redirectCooldown;
    }
    public float GetSecondTimer(){
        return redirectTimer;
    }
    public void ReduceReplenishTime(float amount){
        replenishTime-=amount;
    }
    public void ReduceRedirectCD(float amount){
        redirectCooldown-=amount;
    }

}
