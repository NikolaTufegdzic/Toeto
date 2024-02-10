using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levels;
    [SerializeField] private TMP_Text stages;
    [SerializeField] private TMP_Text drops;
    [SerializeField] private TMP_Text blocks;
    [SerializeField] private TMP_Text gold;
    [SerializeField] private TMP_Text time;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Image result;
    [SerializeField] private Sprite gameWon;
    [SerializeField] private Sprite gameOver;
    [SerializeField] private SettingsUI settingsUI;
    private bool soundPlayed =false;
    void Awake(){
        retryButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            Loader.Load(Loader.Scene.GameScene);
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
    void Start()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StateChanged += GameManager_StateChanged;
    }

    private void GameManager_StateChanged(object sender, System.EventArgs e){
        if(GameManager.Instance.IsGameOver() || GameManager.Instance.IsGameWon()){
            if(!soundPlayed){
                SoundController.Instance.PlaySoundSlide();
                soundPlayed = true;
            }
            GameSceneUI.Instance.SetChooseCursor();
            gameObject.SetActive(true);
            if(GameManager.Instance.IsGameWon()){
                result.sprite = gameWon;
            } else {
                result.sprite = gameOver;
            }
            levels.text = GameManager.Instance.GetLevel().ToString();
            stages.text = GameManager.Instance.GetStagesPassed().ToString();
            drops.text = GameManager.Instance.GetDropsCollected().ToString();
            blocks.text = GameManager.Instance.GetBlocksPopped().ToString();
            gold.text = GameManager.Instance.GetTotalGold().ToString();
            float totalTime = GameManager.Instance.GetTotalTime();
            int hours = (int)(totalTime / 3600);
            int minutes = (int)((totalTime % 3600) / 60);
            float seconds = totalTime % 60;
            time.text =  $"{hours:D2}:{minutes:D2}:{seconds:F2}";
        }
    }
  
}
