using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class GameSceneUI : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private Texture2D pointerCursor;
    [SerializeField] private Texture2D crosshairCursor;
    [SerializeField] private TMP_Text ballSpeed;
    [SerializeField] private TMP_Text padSpeed;
    [SerializeField] private TMP_Text ballPower;
    [SerializeField] private TMP_Text padSize;
    [SerializeField] private TMP_Text goldCoins;
    [SerializeField] private TMP_Text goldCoinsTooltip;
    [SerializeField] private TMP_Text dropChance;
    [SerializeField] private TMP_Text level;
    [SerializeField] private TMP_Text ammo;
    [SerializeField] private TMP_Text ballsLeft;
    [SerializeField] private TMP_Text maxBullets;
    [SerializeField] private TMP_Text reloadTime;
    [SerializeField] private TMP_Text bulletPower;
    [SerializeField] private TMP_Text bulletSpeed;
    [SerializeField] private TMP_Text lvlUpPoints;
    [SerializeField] private TMP_Text lvlUpPointsChoose;
    [SerializeField] private TMP_Text redirectCooldown;
    [SerializeField] private TMP_Text currentStage;
    [SerializeField] private GameObject[] gizmos;
    [SerializeField] private GameObject lvlUpPrompt;
    [SerializeField] private Image nothing;
    [SerializeField] private Sprite[] nothings;
    [SerializeField] private Button[] gizmoShop;
    [SerializeField] private int[] shopPrices;
    [SerializeField] private TMP_Text[] shopPricesText;
    [SerializeField] private Sprite shopCheck;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject stageClear;
    [SerializeField] private GameObject inspectorLabel;
    [SerializeField] private TMP_Text blockHpText;

    public static GameSceneUI Instance { get; private set;}
    private Vector2 cursorOffset;
    private bool isMouseOver = false;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button lvlUpButton;
    private void Awake(){
        Instance = this;
        GizmosOff();
        LvlUpTooltipOff();
        cursorOffset = new Vector2(crosshairCursor.width/2, crosshairCursor.height/2);
        pauseButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            GameManager.Instance.TogglePauseGame();
        });
        lvlUpButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            GameManager.Instance.ChooseLvlUp(true);
            lvlUpButton.gameObject.SetActive(false);
        });
         continueButton.onClick.AddListener(()=>{
            SoundController.Instance.PlaySoundClick();
            StageClearOff();
        });
        for (int i = 0; i < 9; i++){
            int buttonIndex = i;
                gizmoShop[buttonIndex].onClick.AddListener(()=>{
                    SoundController.Instance.PlaySoundClick();
                    GizmoShopClicked(buttonIndex);
            });
        }
        Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
     }

     private void Start(){
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        GameInput.Instance.NothingAction += GameInput_NothingAction;
        SetUpGizmoShop(); 
     }
     private void OnDestroy(){
        GameInput.Instance.NothingAction -= GameInput_NothingAction;
     }
     private void Update(){
        ammo.text = PlayerBall.Instance.GetAmmo().ToString() + "/" +PlayerBall.Instance.GetAmmoMax().ToString();
        ballSpeed.text=PlayerBall.Instance.GetBallSpeed().ToString("F1");
        padSpeed.text=PlayerPad.Instance.GetPadSpeed().ToString("F1");
        ballPower.text=PlayerBall.Instance.GetBallPower().ToString();
        padSize.text=PlayerPad.Instance.GetPadSize().ToString();
        dropChance.text=(GameManager.Instance.GetChance()*100).ToString("F0")+"%";
        goldCoins.text=GameManager.Instance.GetGold().ToString();
        goldCoinsTooltip.text=GameManager.Instance.GetGold().ToString();
        level.text=GameManager.Instance.GetLevel().ToString();
        currentStage.text = (GameManager.Instance.GetStagesPassed()+1).ToString();
        if(GameManager.Instance.GetLevel()>9){
            level.fontSize = 16;
        }
        lvlUpPoints.text = GameManager.Instance.IsChoosingLvlUp()?"":GameManager.Instance.GetLvlUpPoints().ToString();
        lvlUpPointsChoose.text = GameManager.Instance.IsChoosingLvlUp()?GameManager.Instance.GetLvlUpPoints().ToString():"";
        ballsLeft.text= GameManager.Instance.GetBalls().ToString();
        maxBullets.text=PlayerBall.Instance.GetAmmoMax().ToString();
        reloadTime.text=PlayerBall.Instance.GetFirstCD().ToString()+"“";
        bulletPower.text=PlayerBall.Instance.GetBulletPower().ToString();
        bulletSpeed.text=PlayerBall.Instance.GetBulletSpeed().ToString();
        redirectCooldown.text= PlayerBall.Instance.GetSecondCD().ToString()+"“";
        if(GizmoManager.Instance.hasInspector){
             Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                GameObject objectHit = hit.collider.gameObject;
                if(objectHit.layer==10){
                    int hitPoints = objectHit.GetComponent<BulletTarget>().GetBlock().GetHitPoints();
                    inspectorLabel.SetActive(true);
                    blockHpText.text = hitPoints.ToString();
                } else {
                    inspectorLabel.SetActive(false);
                }
            } else {
                inspectorLabel.SetActive(false);
            }
            
        }
    }
    public void StageClearOn(){
        stageClear.SetActive(true);
        SetUpGizmoShop();
    }
    public void StageClearOff(){
        stageClear.SetActive(false);
        GameManager.Instance.NextStage();
    }
     private void GizmoShopClicked(int index){
        if(GameManager.Instance.GetGold()>=shopPrices[index]&&shopPricesText[index].text!=""){
            SoundController.Instance.PlaySoundCoins();
            switch (index){
                case 0:
                    GizmoManager.Instance.MoveKeyOn();
                    break;
                case 1:
                    GizmoManager.Instance.DropPhaserOn();
                    break;
                case 2:
                    GizmoManager.Instance.TargetingKeyOn();
                    break;
                case 3:
                    GizmoManager.Instance.XpBonusOn();
                    break;
                case 4:
                    GizmoManager.Instance.BallCatcherOn();
                    break;
                case 5:
                    GizmoManager.Instance.RollDiscountOn();
                    break;
                case 6:
                    GizmoManager.Instance.GoldBonusOn();
                    break;
                case 7:
                    GizmoManager.Instance.LaunchPowerOn();
                    break;
                case 8:
                    GizmoManager.Instance.InspectorOn();
                    break;
                default:
                    break;
            }
            GameManager.Instance.AddGold(-shopPrices[index]);
            SetUpGizmoShop();
        }
        
     }
     public void LvlUpTooltipOn(){
        lvlUpPrompt.SetActive(true);
        lvlUpButton.gameObject.SetActive(true);
     }
     public void SetLvlUpButton(bool value){
        lvlUpButton.gameObject.SetActive(value);
     }
     public void SetUpGizmoShop(){
        SetUpShopButton(0,GizmoManager.Instance.hasLaunchMobility);
        SetUpShopButton(1,GizmoManager.Instance.hasDropPhaser);
        SetUpShopButton(2,GizmoManager.Instance.hasFreeAim);
        SetUpShopButton(3,GizmoManager.Instance.hasXpBonus);
        SetUpShopButton(4,GizmoManager.Instance.hasBallCatcher);
        SetUpShopButton(5,GizmoManager.Instance.hasRollDiscount);
        SetUpShopButton(6,GizmoManager.Instance.hasGoldBonus);
        SetUpShopButton(7,GizmoManager.Instance.hasLaunchPower);
        SetUpShopButton(8,GizmoManager.Instance.hasInspector);
     }  
     private void SetUpShopButton(int index, bool gizmoActive){
        if(gizmoActive){
            gizmoShop[index].GetComponent<Image>().sprite = shopCheck;
            gizmoShop[index].GetComponent<Image>().color = Color.white;
            shopPricesText[index].text = "";
        }else{
            shopPricesText[index].text = shopPrices[index].ToString();
            if(shopPrices[index]<=GameManager.Instance.GetGold()){
                shopPricesText[index].color= Color.black;
                gizmoShop[index].GetComponent<Image>().color = Color.white;
            } else {
                shopPricesText[index].color = Color.red;
                gizmoShop[index].GetComponent<Image>().color = Color.gray;
            }
        }
     }
     public void LvlUpTooltipOff(){
        lvlUpPrompt.SetActive(false);
     }
     private void GizmosOff(){
        foreach (var gizmo in gizmos)
        {
            gizmo.SetActive(false);
        }
     }
     public void TurnOnGizmo(int i){
        gizmos[i].SetActive(true);
     }
     public bool IsMouseOverUI(){  
        Debug.Log(isMouseOver?"yes":"no");   
        return isMouseOver;      
    }
 
     public void OnPointerEnter(PointerEventData eventData){
        isMouseOver = true;
        Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
    
    }
    
    public void OnPointerExit(PointerEventData eventData){
        isMouseOver = false;
        if(!GameManager.Instance.IsGamePaused()&&!GameManager.Instance.IsChooseState()&&!GameManager.Instance.IsGameOver()&&!GameManager.Instance.IsGameWon()){
             Cursor.SetCursor(crosshairCursor, cursorOffset, CursorMode.Auto);
        }
    }
    
    private void GameManager_OnGamePaused(object sender, System.EventArgs e){
       Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
    }
    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e){
       if(!GameManager.Instance.IsChooseState()){
        Cursor.SetCursor(crosshairCursor, cursorOffset, CursorMode.Auto); 
       }  
       
    } 
    private void GameInput_NothingAction(object sender, System.EventArgs e) {
        if(!nothing.gameObject.activeSelf){
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, 10);
            nothing.sprite = nothings[randomNumber];
            nothing.gameObject.SetActive(true);
            StartCoroutine(WaitToTurnOff(randomNumber));
        }
        
    }
    IEnumerator WaitToTurnOff(int randomNumber)
    {
        yield return new WaitForSeconds(5);
          nothing.gameObject.SetActive(false);
        
    }
    public void SetChooseCursor(){
        Cursor.SetCursor(pointerCursor, Vector2.zero, CursorMode.Auto);
    }
    public void SetGameCursor(){
        Cursor.SetCursor(crosshairCursor, cursorOffset, CursorMode.Auto);
    }
}
