using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class ChooseUI : MonoBehaviour, IPointerDownHandler
{
    
    [SerializeField] private Image prompt;
    [SerializeField] private Sprite[] prompts;
    [SerializeField] private Sprite[] lvlUps;
    [SerializeField] private Sprite accept;
    [SerializeField] private Sprite confirm;
    [SerializeField] private Sprite close;
    [SerializeField] private GameObject choosePad;
    [SerializeField] private GameObject chooseLvlUp;
    [SerializeField] private GameObject givenOne;
    [SerializeField] private GameObject chooseUIElements;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button rollButton;
    [SerializeField] private Button toggleButton;
    [SerializeField] private GameObject[] chosenPads;
    [SerializeField] private GameObject[] chosenLvlUps; 
    [SerializeField] private TextMeshProUGUI[] LvlRarity;
    [SerializeField] private Sprite goldImg;
    [SerializeField] private Sprite showButton;
    [SerializeField] private Sprite hideButton;
    private bool choisesHidden = false;
    private int rollCost = 1;

    private int confirmClicks = 3;
    private int choise = 0;
    private void Awake(){
        confirmButton.onClick.AddListener(()=>{
            ConfirmButtonAction();
            SoundController.Instance.PlaySoundClick();
        });
        rollButton.onClick.AddListener(()=>{
            RollButtonAction();
            SoundController.Instance.PlaySoundClick();    
        });
        toggleButton.onClick.AddListener(()=>{
            ToggleButtonAction();
            SoundController.Instance.PlaySoundClick();    
        });
    }
    private void Start(){

        if(!GameManager.Instance.IsChoosingPad()){
            choosePad.SetActive(false);
        }
        if(!GameManager.Instance.IsChoosingLvlUp()){
            chooseLvlUp.SetActive(false);
        }
        toggleButton.gameObject.SetActive(false);
        givenOne.SetActive(false);
        SetChosen(GameManager.Instance.IsChoosingPad()?chosenPads:chosenLvlUps);   
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
       GameObject clickedObject = pointerEventData.pointerCurrentRaycast.gameObject;
         SoundController.Instance.PlaySoundSelect(); 
         ToggleActive(int.Parse(clickedObject.name));
         SetChosen(GameManager.Instance.IsChoosingPad()?chosenPads:chosenLvlUps);   
       Debug.Log(choise);
    }
    private void ConfirmButtonAction(){
        if(GameManager.Instance.IsChoosingPad()){
            if(choise==0){
                if(confirmClicks==0){
                    GameManager.Instance.PadChosen(choise);
                }else{
                    confirmClicks--;
                    prompt.GetComponent<Image>().sprite = prompts[confirmClicks];
                    if(confirmClicks==0){
                        givenOne.SetActive(true);
                        confirmButton.GetComponent<Image>().sprite = accept;
                        choosePad.SetActive(false);
                    }
                }
            }else{ 
                GameManager.Instance.PadChosen(choise);
            }
        } else{
            GameManager.Instance.LvlUpChosen(choise);
        }
    }
    private void RollButtonAction(){
        if(GameManager.Instance.GetGold()>=rollCost)
        {
            
            GameManager.Instance.AddGold(-rollCost);
            GameManager.Instance.RollUp();
            SetLvlUpUI(GameManager.Instance.GetLvlUps(),rollCost!=9?rollCost+1:9);
            
        }
    }
    private void ToggleButtonAction(){
        choisesHidden = !choisesHidden;
        toggleButton.GetComponent<Image>().sprite = choisesHidden?showButton:hideButton; 
        chooseLvlUp.SetActive(!choisesHidden);
        chooseUIElements.SetActive(!choisesHidden);
    }
    public void SetLvlUpUI(int[] powerups, int newRollCost){
        choisesHidden = false;
        toggleButton.GetComponent<Image>().sprite = hideButton; 
        toggleButton.gameObject.SetActive(true);
        GameSceneUI.Instance.SetLvlUpButton(false);
        givenOne.SetActive(false);
        choosePad.SetActive(false);
        chooseLvlUp.SetActive(true);
        rollCost = newRollCost;
        rollButton.GetComponentInChildren<TextMeshProUGUI>().text=rollCost.ToString();
        for (int i = 0; i <3 ; i++)
        {   if(powerups[i]>=0){
                chooseLvlUp.transform.Find("Choise"+(i+1).ToString()).GetComponent<Image>().sprite=lvlUps[powerups[i]];
                if(powerups[i]>=0 && powerups[i]<=9){
                    string rarity = GameManager.Instance.GetRarity(powerups[i]);
                    int currentLvl = GameManager.Instance.GetCurrentLvl(powerups[i]);
                    int maxLvl = GameManager.Instance.GetMaxLvl(powerups[i]);
                    LvlRarity[i].text = rarity + " - Level " + (currentLvl==maxLvl?"MAX":currentLvl.ToString());
                } else {
                    LvlRarity[i].text = "";
                }
            } else {
                
                chooseLvlUp.transform.Find("Choise"+(i+1).ToString()).GetComponent<Image>().sprite = goldImg;
            }
           
            
        }
        confirmButton.GetComponent<Image>().sprite = close;
        choise=0;
        SetChosen(chosenLvlUps);
    }
    private void SetChosen(GameObject[] chosen){
        for (int i = 0; i < chosen.Length; i++)
        {
            if(i+1==choise){
                chosen[i].SetActive(true);
            } else {
                chosen[i].SetActive(false);
            }
        }
    }
    private void ToggleActive(int name){
        if (name==choise){
            choise = 0;
            if(GameManager.Instance.IsChoosingLvlUp()){
                confirmButton.GetComponent<Image>().sprite = close;
            }
        } else {
            choise = name;
            confirmButton.GetComponent<Image>().sprite = confirm;
        }
    }
    public int GetChoise(){
        return choise;
    }
}
