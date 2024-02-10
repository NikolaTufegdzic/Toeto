using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAim : MonoBehaviour
{
    public Quaternion aimRotation;
    private Vector3 forward = new Vector3(0,1,0);
    private Vector3 rotator = new Vector3(0,0,-1);
    private RaycastHit2D rayHit;
    [SerializeField] private GameObject aim;
    [SerializeField] private LayerMask layerMask;
    private Vector3 directionVector;
    private void Start(){
        aimRotation = Quaternion.Euler(0, 0 , 90);
    }
    private void Update()
    {  
       if(GameManager.Instance.IsBallLaunching() && GizmoManager.Instance.hasLaunchAim){ 
        if(GizmoManager.Instance.hasFreeAim ){
            aimRotation = Quaternion.Euler(0,0,Vector3.SignedAngle(PlayerBall.Instance.BallCursorVector(), forward,rotator));
            directionVector = PlayerBall.Instance.GetBallDirection();
        } else {
            aimRotation = Quaternion.Euler(0, 0 , 0);
            directionVector = new Vector3(0,1,0);
        }
        transform.rotation = aimRotation;
        rayHit = Physics2D.Raycast(PlayerBall.Instance.GetBallPosition(), directionVector , 30f, layerMask);
        float distance = rayHit.distance;
        aim.GetComponent<RectTransform>().sizeDelta = new Vector2(0.1f,distance);
        aim.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,distance/2);
       } else {
         aim.GetComponent<RectTransform>().sizeDelta = new Vector2(0.1f,0);   
       }
    }
}
