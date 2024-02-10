using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreenUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button theGameButton;
    [SerializeField] private Button[] codeButtons;
    [SerializeField] private GameObject[] codesActivated;
    private int[] enteredCode;
    private int[,] codeNumbers;
    private int arrayIndex = 0;
    private void Awake(){
        enteredCode= new int[10000];;
        mainMenuButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        theGameButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            Loader.Load(Loader.Scene.GameScene);
        });
        for (int i = 0; i < 10; i++)
        {
            int buttonIndex = i;
                codeButtons[i].onClick.AddListener(()=>{
                SoundController.Instance.PlaySoundClick();
                CodeEnter(buttonIndex); 
            });
            
        }
        Time.timeScale = 1f;
         codeNumbers = new int[,]
        {
            {0, 0, 3, 3, 0, 0, 1, 1, 3, 3, 1, 1}, // 10 000 ammo
            {5, 3, 1, 1, 3, 5, 2, 1, 3, 3, 1, 2}, // 10 000 power
            {0, 1, 3, 1, 5, 5, 4, 3, 2, 3, 5, 3}, // gizmo 0
            {0, 1, 5, 6, 4, 3, 2, 1, 4, 6, 5, 5},  // gizmo 1
            {0, 2, 2, 3, 2, 3, 5, 4, 5, 4, 0, 2},  // gizmo 2
            {0, 2, 1, 2, 1, 2, 3, 2, 3, 2, 0, 2},  // gizmo 3
            {0, 3, 6, 5, 7, 2, 9, 2, 2, 3, 0, 3},  // gizmo 4
            {0, 3, 5, 1, 5, 2, 5, 3, 8, 8, 0, 3},  // gizmo 5
            {0, 4, 8, 7, 8, 7, 1, 1, 9, 9, 4, 0},  // gizmo 6
            {8, 5, 9, 4, 7, 2, 7, 6, 6, 7, 0, 1},  // gizmo 7
            {0, 1, 6, 5, 8, 4, 9, 8, 0, 5, 3, 2},  // gizmo 8
            {1, 1, 8, 7, 8, 7, 9, 9, 2, 3, 2, 1},  // 1 000 gold
            {9, 8, 6, 7, 4, 4, 2, 6, 4, 2, 0, 1},  // gizmo svi
            {8, 8, 9, 7, 4, 5, 6, 2, 8, 8, 4, 3},  // super nothing
        };
    }

    private void CodeEnter(int code){
        enteredCode[arrayIndex] = code;
        arrayIndex++;
        if(arrayIndex>=12){
            CheckCode();
            Debug.Log(arrayIndex);
        } 
        
    }

    private void CheckCode(){
        for (int i = 0; i < 14; i++)
        {
            bool codeCheck = true;
            for (int j = 0; j < 12; j++)
            {
                if(enteredCode[(arrayIndex-12+j)]!=codeNumbers[i,j]){
                    codeCheck = false;
                } 
            }
            if(codeCheck){
                CodesActivated.Instance.AddCodeActivated(i);
                codesActivated[i].SetActive(true);
            }
        }
    }
}
