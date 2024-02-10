using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

public class GameInput : MonoBehaviour
{   
    public event EventHandler OnLaunch;
    public event EventHandler PointFirst;
    public event EventHandler PointSecond;
    public event EventHandler OnPauseAction;
    public event EventHandler OnLevelUpAction;
    public event EventHandler NothingAction;
    public static GameInput Instance { get; private set;}

    public enum Binding{
        Shoot,
        ShootAlt,
        Redirect,
        RedirectAlt,
        Launch,
        LaunchAlt,
        LevelUp,
        LevelUpAlt,
        Nothing,
        NothingAlt,
        Up,
        UpAlt,
        Down,
        DownAlt,
        Right,
        RightAlt,
        Left,
        LeftAlt,
        Pause,
        PauseAlt,
    }

    PlayerInputActions playerInputActions;

    private void Awake() {
         if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerPad.Enable();
        playerInputActions.PlayerAbilities.Enable();
        playerInputActions.PlayerInteractions.Enable();
        playerInputActions.PlayerPad.Launch.performed += GameInput_OnLaunch;
        playerInputActions.PlayerAbilities.PointAbility1.performed += GameInput_PointFirst;
        playerInputActions.PlayerAbilities.PointAbility2.performed += GameInput_PointSecond;
        playerInputActions.PlayerInteractions.Escape.performed += GameInput_OnPauseAction;
        playerInputActions.PlayerInteractions.LevelUp.performed += GameInput_OnLevelUpAction;
        playerInputActions.PlayerInteractions.Nothing.performed += GameInput_NothingAction;

    }
    private void OnDestroy(){
        playerInputActions.PlayerPad.Launch.performed -= GameInput_OnLaunch;
        playerInputActions.PlayerAbilities.PointAbility1.performed -= GameInput_PointFirst;
        playerInputActions.PlayerAbilities.PointAbility2.performed -= GameInput_PointSecond;
        playerInputActions.PlayerInteractions.Escape.performed -= GameInput_OnPauseAction; 
        playerInputActions.PlayerInteractions.LevelUp.performed -= GameInput_OnLevelUpAction;
        playerInputActions.PlayerInteractions.Nothing.performed -= GameInput_NothingAction;
        playerInputActions.Dispose();
    }
    public Vector3 GetPadMovementLeftRight() {
        Vector2 inputVector = playerInputActions.PlayerPad.LeftRight.ReadValue<Vector2>();
        return (Vector3)inputVector;
    }
     private void GameInput_OnPauseAction(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
       OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
     private void GameInput_OnLevelUpAction(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
       OnLevelUpAction?.Invoke(this, EventArgs.Empty);
    }
    private void GameInput_NothingAction(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
       NothingAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void GameInput_OnLaunch(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnLaunch?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_PointFirst(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        PointFirst?.Invoke(this, EventArgs.Empty);
    }
     private void GameInput_PointSecond(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        PointSecond?.Invoke(this, EventArgs.Empty);
    }
    public string GetBindingText(Binding binding){
        switch (binding)
        {
            case Binding.Shoot:
                return playerInputActions.PlayerAbilities.PointAbility1.bindings[0].ToDisplayString();
            case Binding.ShootAlt:
                return playerInputActions.PlayerAbilities.PointAbility1.bindings[1].ToDisplayString();
            case Binding.Redirect:
                return playerInputActions.PlayerAbilities.PointAbility2.bindings[0].ToDisplayString();
            case Binding.RedirectAlt:
                 return playerInputActions.PlayerAbilities.PointAbility2.bindings[1].ToDisplayString();
            case Binding.Launch:
                 return playerInputActions.PlayerPad.Launch.bindings[0].ToDisplayString();
            case Binding.LaunchAlt:
                 return playerInputActions.PlayerPad.Launch.bindings[1].ToDisplayString();
            case Binding.LevelUp:
                 return playerInputActions.PlayerInteractions.LevelUp.bindings[0].ToDisplayString();
            case Binding.LevelUpAlt:
                 return playerInputActions.PlayerInteractions.LevelUp.bindings[1].ToDisplayString();
            case Binding.Nothing:
                 return playerInputActions.PlayerInteractions.Nothing.bindings[0].ToDisplayString();
            case Binding.NothingAlt:
                 return playerInputActions.PlayerInteractions.Nothing.bindings[1].ToDisplayString();
            case Binding.Up:
                 return playerInputActions.PlayerPad.UpDown.bindings[1].ToDisplayString();
            case Binding.UpAlt:
                 return playerInputActions.PlayerPad.UpDown.bindings[4].ToDisplayString();
            case Binding.Down:
                 return playerInputActions.PlayerPad.UpDown.bindings[2].ToDisplayString();;
            case Binding.DownAlt:
                 return playerInputActions.PlayerPad.UpDown.bindings[5].ToDisplayString();
            case Binding.Right:
                 return playerInputActions.PlayerPad.LeftRight.bindings[1].ToDisplayString();
            case Binding.RightAlt:
                 return playerInputActions.PlayerPad.LeftRight.bindings[4].ToDisplayString();
            case Binding.Left:
                 return playerInputActions.PlayerPad.LeftRight.bindings[2].ToDisplayString();
            case Binding.LeftAlt:
                 return playerInputActions.PlayerPad.LeftRight.bindings[5].ToDisplayString();
            case Binding.Pause:
                 return playerInputActions.PlayerInteractions.Escape.bindings[0].ToDisplayString();
            case Binding.PauseAlt:
                  return playerInputActions.PlayerInteractions.Escape.bindings[1].ToDisplayString();
            default:
                 return "";
        }
    }

    public void RebindBinding( Binding binding, Action onActionRebound){
         InputAction inputAction;
         int keyIndex = 0;
         switch (binding)
          {
               case Binding.Shoot:
                    inputAction=playerInputActions.PlayerAbilities.PointAbility1;
                    keyIndex = 0;
               break;
               case Binding.ShootAlt:
                    inputAction=playerInputActions.PlayerAbilities.PointAbility1;
                    keyIndex = 1 ;
               break;
               case Binding.Redirect:
                    inputAction=playerInputActions.PlayerAbilities.PointAbility2;
                    keyIndex = 0;
               break;
               case Binding.RedirectAlt:
                    inputAction=playerInputActions.PlayerAbilities.PointAbility2;
                    keyIndex =1;
               break;
               case Binding.Launch:
                    inputAction=playerInputActions.PlayerPad.Launch;
                    keyIndex =0;
               break;
               case Binding.LaunchAlt:
                    inputAction=playerInputActions.PlayerPad.Launch;
                    keyIndex =1;
               break;
               case Binding.LevelUp:
                    inputAction=playerInputActions.PlayerInteractions.LevelUp;
                    keyIndex =0;
               break;
               case Binding.LevelUpAlt:
                    inputAction=playerInputActions.PlayerInteractions.LevelUp;
                    keyIndex =1;
               break;
               case Binding.Nothing:
                    inputAction=playerInputActions.PlayerInteractions.Nothing;
                    keyIndex =0;
               break;
               case Binding.NothingAlt:
                    inputAction=playerInputActions.PlayerInteractions.Nothing;
                    keyIndex =1;
               break;
               case Binding.Up:
                    inputAction=playerInputActions.PlayerPad.UpDown;
                    keyIndex = 1;
               break;
               case Binding.UpAlt:
                    inputAction=playerInputActions.PlayerPad.UpDown;
                    keyIndex =4;
               break;
               case Binding.Down:
                    inputAction=playerInputActions.PlayerPad.UpDown;
                    keyIndex = 2;
               break;
               case Binding.DownAlt:
                    inputAction=playerInputActions.PlayerPad.UpDown;
                    keyIndex = 5;
               break;
               case Binding.Right:
                    inputAction=playerInputActions.PlayerPad.LeftRight;
                    keyIndex = 1;
               break;
               case Binding.RightAlt:
                    inputAction=playerInputActions.PlayerPad.LeftRight;
                    keyIndex =4;
               break;
               case Binding.Left:
                    inputAction=playerInputActions.PlayerPad.LeftRight;
                    keyIndex = 2;
               break;
               case Binding.LeftAlt:
                    inputAction=playerInputActions.PlayerPad.LeftRight;
                    keyIndex = 5;
               break;
               case Binding.Pause:
                    inputAction=playerInputActions.PlayerInteractions.Escape;
                    keyIndex = 0;
               break;
               case Binding.PauseAlt:
                    inputAction=playerInputActions.PlayerInteractions.Escape;
                    keyIndex = 1;
               break;
               default:
                    inputAction=playerInputActions.PlayerInteractions.Nothing;
               break;      
          } 
        inputAction.Disable();
        inputAction.PerformInteractiveRebinding(keyIndex)
          .OnComplete(callback => {
               inputAction.Enable();
               onActionRebound();
          })
          .Start();
    }

}
