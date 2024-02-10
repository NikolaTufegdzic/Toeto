using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{   
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private SettingsUI settingsUI;
    void Awake(){
        resumeButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            GameManager.Instance.TogglePauseGame(); 
        });
        settingsButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            SettingsUI.Instance.SetSettingsState(true); 
        });
        mainMenuButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start(){
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e){
        Show();
    }
    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e){
        Hide();
    } 
   private void Show(){
        gameObject.SetActive(true);
   }

   private void Hide(){
         gameObject.SetActive(false);
   }
}
