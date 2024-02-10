using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    public static GizmoManager Instance { get; private set;}

    public bool hasLaunchAim = true;
    public bool hasFreeAim = true;
    public bool hasLaunchMobility = true;
    public bool hasBallCatcher = false; 
    public bool hasTwinBullet = false;
    public bool hasDropPhaser = false;
    public bool hasXpBonus = false;
    public bool hasRollDiscount = false;
    public bool hasLaunchPower = false;
    public bool hasGoldBonus = false;
    public bool hasInspector = false;
    public bool hasNothing = false ;
    public bool hasSuperNothing = false;
   
    private void Awake(){
        Instance = this;
    }
    public void FreeAimLockedMove(){
        GameSceneUI.Instance.TurnOnGizmo(2);
        hasLaunchAim = true;
        hasFreeAim = true; 
        hasLaunchMobility = false;
    }

    public void LockedAimFreeMove(){
        GameSceneUI.Instance.TurnOnGizmo(0);
        hasLaunchAim = true;
        hasFreeAim = false; 
        hasLaunchMobility = true;
    }
    public void NoAimNoMove(){
        hasLaunchAim = false;
        hasFreeAim = false; 
        hasLaunchMobility = false;
        hasNothing = true;
    }
    public void MoveKeyOn(){
        GameSceneUI.Instance.TurnOnGizmo(0);
        hasLaunchMobility = true;
    }
    public void DropPhaserOn(){
        GameSceneUI.Instance.TurnOnGizmo(1);
        hasDropPhaser = true;
    }
    public void TargetingKeyOn(){
        GameSceneUI.Instance.TurnOnGizmo(2);
        hasLaunchAim = true;
        hasFreeAim = true;
    }
    public void RollDiscountOn(){
        GameSceneUI.Instance.TurnOnGizmo(3);
        hasRollDiscount = true;
    }
    public void XpBonusOn(){
        GameSceneUI.Instance.TurnOnGizmo(4);
        hasXpBonus = true;
    }
    public void BallCatcherOn(){
        GameSceneUI.Instance.TurnOnGizmo(5);
        hasBallCatcher = true;
        PlayerPad.Instance.TurnOnCatcher();
    }
    public void LaunchPowerOn(){
        GameSceneUI.Instance.TurnOnGizmo(6);
        hasLaunchPower = true;
    }
    public void GoldBonusOn(){
        GameSceneUI.Instance.TurnOnGizmo(7);
        hasGoldBonus = true;
    }
    public void InspectorOn(){
        GameSceneUI.Instance.TurnOnGizmo(8);
        hasInspector = true;
    }
    public void NothingOn(){
        hasNothing = true;
    }
    public void SuperNothingOn(){
        hasSuperNothing = true;
    }
}
