using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private Button endGameButton;
    [SerializeField] private Image creditsImage;
    [SerializeField] private Sprite hasNothing;
    [SerializeField] private Sprite hasSomething; 
    [SerializeField] private GameObject codeObject;
    [SerializeField] private Image[] code;
    [SerializeField] private Sprite[] musicSprites;
    [SerializeField] private Sprite[] threeLinesSprites;
    [SerializeField] private Sprite[] miscSprites;
    [SerializeField] private Sprite[] hollowSprites;
    [SerializeField] private Sprite[] basicSprites;
    private int[,] codeNumbers;
    private float timer = 0;
    private void Awake()
    {
        endGameButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            EndGame();
        });
    }
    private void Start()
    {   
        codeNumbers = new int[,]
        {
            {0, 0, 3, 3, 0, 0, 1, 1, 3, 3, 1, 1},
            {5, 3, 1, 1, 3, 5, 2, 1, 3, 3, 1, 2},
            {0, 1, 3, 1, 5, 5, 4, 3, 2, 3, 5, 3},
            {0, 1, 5, 6, 4, 3, 2, 1, 4, 6, 5, 5},
            {0, 2, 2, 3, 2, 3, 5, 4, 5, 4, 0, 2},
            {0, 2, 1, 2, 1, 2, 3, 2, 3, 2, 0, 2},
            {0, 3, 6, 5, 7, 2, 9, 2, 2, 3, 0, 3},
            {0, 3, 5, 1, 5, 2, 5, 3, 8, 8, 0, 3},
            {0, 4, 8, 7, 8, 7, 1, 1, 9, 9, 4, 0},
            {8, 5, 9, 4, 7, 2, 7, 6, 6, 7, 0, 1},
            {0, 1, 6, 5, 8, 4, 9, 8, 0, 5, 3, 2},
            {1, 1, 8, 7, 8, 7, 9, 9, 2, 3, 2, 1},
            {9, 8, 6, 7, 4, 4, 2, 6, 4, 2, 0, 1},
            {8, 8, 9, 7, 4, 5, 6, 2, 8, 8, 4, 3},
        };
        GameManager.Instance.StateChanged += GameManager_StateChanged;
        GameInput.Instance.NothingAction += GameInput_NothingAction;
        SetCode();
        Hide();
    }
    private void OnDestroy(){
        GameInput.Instance.NothingAction -= GameInput_NothingAction;
    }
    private void Update(){
        timer += Time.deltaTime;
     }

    private void GameManager_StateChanged(object sender, System.EventArgs e){
        
        if(GameManager.Instance.IsCredits()){
            Show();
             Debug.Log(GizmoManager.Instance.hasNothing);
            creditsImage.sprite = GizmoManager.Instance.hasNothing?hasNothing:hasSomething;
        }
    }
    private void GameInput_NothingAction(object sender, System.EventArgs e) {
        Debug.Log("da");
        Debug.Log(GizmoManager.Instance.hasNothing);
        if(GizmoManager.Instance.hasNothing&&(timer>10f)){
            Debug.Log("bre");
            codeObject.SetActive(true);
        }
    }
    private void SetCode(){
        int randomNumber = UnityEngine.Random.Range(0, 14);
        for (int i = 0; i < 12; i++)
        {
            switch (codeNumbers[randomNumber,i])
            {
                case 4:
                    code[i].sprite = musicSprites[UnityEngine.Random.Range(0, 7)];
                break;
                case 6:
                    code[i].sprite = threeLinesSprites[UnityEngine.Random.Range(0, 4)];
                break;
                case 7:
                    code[i].sprite = miscSprites[UnityEngine.Random.Range(0, 8)];
                break;
                case 9:
                    code[i].sprite = miscSprites[UnityEngine.Random.Range(0, 5)];
                break;
                default:
                    code[i].sprite = basicSprites[codeNumbers[randomNumber,i]];
                break;
            }
        }
    }
    private void EndGame(){
        GameManager.Instance.SetGameWon();
    }
    public void Show(){
        gameObject.SetActive(true);
    }
    public void Hide(){
        gameObject.SetActive(false);
    }

}
