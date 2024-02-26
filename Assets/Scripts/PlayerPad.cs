using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPad : MonoBehaviour
{
    public static PlayerPad Instance { get; private set;}

    private float padSpeed = 3f;
    private int padSize = 5;
    public bool isBallLaunched;
    private bool wallCollision;
    private bool padMobile; 
    private float launchTimer=0f;
    private float launchTimeLimit=5;
    
    private void Awake() {
        Instance = this;  
    }

    private void Start() {
        padMobile = true;
        isBallLaunched = false;
        wallCollision = false;
        GameInput.Instance.OnLaunch += GameInput_OnLaunch;
        if (!GizmoManager.Instance.hasBallCatcher){
            gameObject.transform.Find("BallCatcher").gameObject.SetActive(false);
        }
    }
    private void OnDestroy(){
        GameInput.Instance.OnLaunch -= GameInput_OnLaunch;
    }

    private void Update(){    
        
        Vector3 inputVector = GameInput.Instance.GetPadMovementLeftRight();
         if(GameManager.Instance.IsGameOver()){
            gameObject.SetActive(false);
        }
        if(wallCollision){
            if(((transform.position.x<0) && (inputVector.x>0)) || ((transform.position.x>0) && (inputVector.x<0))){
                wallCollision = false;
            } 
        }
        if(!wallCollision&&!(GameManager.Instance.IsBallLaunching()&&!GizmoManager.Instance.hasLaunchMobility)){ 
            padMobile = true;
        }        
        else{
            padMobile = false;
        }
        if(padMobile){
            MovePlayerPad(inputVector);
        }

        LaunchTimer();
    }
    private void LaunchTimer(){
        if(GameManager.Instance.IsBallLaunching()){
            if(launchTimer<launchTimeLimit){
                launchTimer += Time.deltaTime;
            } else{
                GameManager.Instance.ForcedLaunch();
                launchTimer = 0;
            }
        } else {
            launchTimer = 0;
        }
    }
    public void TurnOnCatcher(){
        gameObject.transform.Find("BallCatcher").gameObject.SetActive(true);
    }
    private void MovePlayerPad(Vector3 moveVector){
         transform.position += moveVector*padSpeed*Time.deltaTime;
    }
    private void GameInput_OnLaunch(object sender, System.EventArgs e){
        isBallLaunched = true;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.name == "Walls"){
            wallCollision = true;
        }

        if (collision.gameObject.GetComponent<Drop>() != null){
            Destroy(collision.gameObject);
            DropManager.Instance.DropHandler(collision.gameObject);
        }
    }

    public void ExtendPad(){
        padSize++;
        float x = gameObject.transform.localScale.x+0.2f;
        float y = gameObject.transform.localScale.y;
        float z = gameObject.transform.localScale.z;
        gameObject.transform.localScale = new UnityEngine.Vector3 (x,y,z);
    }
    public void ReducePad(){
        padSize--;
        float x = gameObject.transform.localScale.x-0.2f;
        float y = gameObject.transform.localScale.y;
        float z = gameObject.transform.localScale.z;
        gameObject.transform.localScale = new UnityEngine.Vector3 (x,y,z);
    }

    public void SlowerPad(){
        padSpeed -= 0.1f;
    }

    public void FasterPad(){
        padSpeed += 0.1f;
    }

    public bool IsBallLaunched(){
        return isBallLaunched;
    }
    public int GetPadSize(){
        return padSize;
    }
    public float GetPadSpeed(){
        return padSpeed;
    }

    public void AddLaunchTime(float amount){
        launchTimeLimit+=amount;
    }
}
