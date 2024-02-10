using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;
    private void Awake(){
        playButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            if(SettingsUI.Instance.GetSkipTutorial()){
                Loader.Load(Loader.Scene.GameScene);
            }else{
                Loader.Load(Loader.Scene.TutorialScene);
            }
            CodesActivated.Instance.ClearList();
        });
         settingsButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            SettingsUI.Instance.SetSettingsState(true); 
        });
        quitButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            Application.Quit();
        });
        Time.timeScale = 1f;
    }
    
}
