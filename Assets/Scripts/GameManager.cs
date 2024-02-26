using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    private bool createNewLvlUps = true;
    private int rollCost = 1; 
    public event EventHandler StateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    [SerializeField] private GameObject stage;
    [SerializeField] private GameObject chooseUI;
    [SerializeField] private PowerUpHandler[] powerUpLvls;
    private bool redirectLuck = false;
    private int[] levelUps;
    private int dropHitGold=0;
    private int dropPower=0;
    private int stageBlocks =0;
    private int dropsCollected=0;
    private int redirectPower = 0;
    private int stagesPassed = 0;
    private int luckyRedirectDrops = 0;
    private float dropChance = 0.15f;
    private int ballsLeft;
    private int gold = 0;
    private int totalGold;
    private int xp;
    private int lvl;
    private int blocksPopped = 0;
    private bool isGamePaused = false;
    private bool superNothingUsed = false;
    private int lvlUpPoints = 0;
    private float totalTime = 0;
    private enum State{
        BallLaunch,
        GamePlaying,
        GameOver,
        ChoosingPad,
        ChoosingLvlUp,
        GameWon,
        GizmoShop,
        Credits,
    }
    
    [System.Serializable]
    public struct PowerUpHandler{
        public bool reachedMax;
        public int currentLvl;
        public int maxLvl;  
        public string rarity;  
    }
    
   
    private State state;
    private State prevousState;
    private State preLvlUpState;

    private void Awake() {
        Instance = this;
        StateChanged += GameManager_StateChanged;
        prevousState = State.ChoosingPad;
        state = State.ChoosingPad;
        preLvlUpState = State.ChoosingPad;
        levelUps = new int[3];
        
    }

    private void Start() {
        ballsLeft = 3; 
        xp = 0;
        lvl = 1;
        prevousState = State.ChoosingPad;
        state = State.ChoosingPad;
        stage.SetActive(false);
        GameInput.Instance.OnLaunch += GameInput_OnLaunch;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnLevelUpAction += GameInput_OnLevelUpAction;
        GameInput.Instance.NothingAction += GameInput_NothingAction;
        PlayerBall.Instance.BallDestroyed += PlayerBall_BallDestroyed;
        PlayerBall.Instance.BallCaught += PlayerBall_BallCaught;
        ActivateCodes();

    }
    private void OnDestroy(){
        GameInput.Instance.OnLaunch -= GameInput_OnLaunch;
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
        GameInput.Instance.OnLevelUpAction -= GameInput_OnLevelUpAction;
        GameInput.Instance.NothingAction -= GameInput_NothingAction;

    }
    private void Update(){
        if(prevousState!=state){
            StateChanged?.Invoke(this,EventArgs.Empty);
        }
        if(state == State.GamePlaying || state == State.BallLaunch){
            totalTime+= Time.deltaTime;
        }
        
    }
    public float GetTotalTime(){
        return totalTime;
    }
    public void ForcedLaunch(){
        state = State.GamePlaying;
        SoundController.Instance.PlaySoundLaunch(PlayerBall.Instance.GetBallPosition());
        if(GizmoManager.Instance.hasLaunchPower){
                PlayerBall.Instance.AddBallPower(1);
        }
    }
    public void PadChosen(int choise){
        chooseUI.SetActive(false);
        stage.SetActive(true);
        state= State.BallLaunch;
        switch (choise)
        {
            case 1:
                GizmoManager.Instance.FreeAimLockedMove();
            break;
            case 2:
                GizmoManager.Instance.LockedAimFreeMove();
            break;
            default:
                GizmoManager.Instance.NoAimNoMove(); 
            break;
        }
        GameSceneUI.Instance.SetGameCursor();
    }
    private void AmmoUpgrade(){
        PlayerBall.Instance.AddAmmoMax(1);
        PlayerBall.Instance.AddAmmo(1);
    }
    private void AmmoCooldown(){
        PlayerBall.Instance.ReduceReplenishTime(1f);
    }
    private void RedirectCooldown(){
        PlayerBall.Instance.ReduceRedirectCD(2f);
    }
    private void BulletPower(){
        PlayerBall.Instance.AddBulletPower(1);
    }
    private void LaunchUpgrade(){
        PlayerPad.Instance.AddLaunchTime(2f);
    }
    private void DropLuck(){
        dropChance += 0.05f;
    }
    private void ExtraBall(){
        ballsLeft++;
    }
    private void PrizeShot(){
        dropHitGold++;
    }
    private void  RedirectPower(){
        redirectPower+=2;
    }
    private void LuckyRedirect(){
        luckyRedirectDrops++;
    }
    private void PowerfulDrops(){
        dropPower++;
    }
    private void TwinBullet(){
        GizmoManager.Instance.hasTwinBullet=true;
    }
    public void DropHit(){
        AddGold(dropHitGold);
    }
    public void LvlUpChosen(int choise){
        if (choise!=0){
            createNewLvlUps = true;
            lvlUpPoints--;
            int chosenPowerUp = levelUps[choise-1];
            switch(chosenPowerUp){
                case 0: 
                    AmmoUpgrade();
                break;
                case 1:
                    AmmoCooldown();
                break;
                case 2:
                    RedirectCooldown();
                break;
                case 3:
                    PrizeShot();
                break;
                case 4:
                    LaunchUpgrade();
                break;
                case 5:
                    DropLuck();
                break;
                case 6:
                    PowerfulDrops();
                break;
                case 7:
                    BulletPower();
                break;
                case 8:
                    RedirectPower();
                break;
                case 9:
                    LuckyRedirect();
                break;
                case 10:
                    ExtraBall();
                break;
                case 11:
                    TwinBullet();
                break;
                default:
                    AddGold(10);
                break;
            }
            if(chosenPowerUp>=0 && chosenPowerUp<=11){
                powerUpLvls[chosenPowerUp].currentLvl++;
                if(powerUpLvls[chosenPowerUp].currentLvl>powerUpLvls[chosenPowerUp].maxLvl){
                    powerUpLvls[chosenPowerUp].reachedMax = true;
                }
            }
            if (lvlUpPoints>0){
                rollCost = 1;
                ChooseLvlUp(false);
            } else {
                LvlChosenBackToGame();
                GameSceneUI.Instance.LvlUpTooltipOff();
            }
            
        } else {
         LvlChosenBackToGame();  
         createNewLvlUps = false; 
         GameSceneUI.Instance.LvlUpTooltipOn();
        }
    }
    private void LvlChosenBackToGame(){
        GameSceneUI.Instance.SetGameCursor();
        chooseUI.SetActive(false);
        state = preLvlUpState;
        if(lvlUpPoints>0){
          GameSceneUI.Instance.SetLvlUpButton(true);  
        }
        Time.timeScale = 1f;
    }
    private void GameInput_OnLaunch(object sender, System.EventArgs e){
        if(!isGamePaused){
             if(state==State.BallLaunch){
                state = State.GamePlaying;
                SoundController.Instance.PlaySoundLaunch(PlayerBall.Instance.GetBallPosition());
                if(GizmoManager.Instance.hasLaunchPower){
                    PlayerBall.Instance.AddBallPower(1);
                }
             }
             
        }
    }
    private void GameInput_OnPauseAction(object sender, System.EventArgs e){
         if(!SettingsUI.Instance.GetSettingsUp()){
            TogglePauseGame();
        } else {
            SettingsUI.Instance.SetSettingsState(false);
            SettingsUI.Instance.WriteSettings();
        }
    }
    private void GameInput_OnLevelUpAction(object sender, System.EventArgs e){
        if(lvlUpPoints>0 && !IsChoosingLvlUp() && (state==State.GamePlaying || state==State.BallLaunch)){
            ChooseLvlUp(true);
        }
    }
    private void GameInput_NothingAction(object sender, System.EventArgs e) {
        if(GizmoManager.Instance.hasSuperNothing && (state==State.GamePlaying || state==State.BallLaunch)){
            Transform blocks = BlockGenerator.Instance.transform;
             for (int i = 0; i < blocks.childCount; i++)
             {
                superNothingUsed = true;
                Block block = blocks.GetChild(i).gameObject.GetComponent<Block>();
                block.GotHit(block.GetHitPoints());
             }
             
        }
    }
    private void PlayerBall_BallDestroyed(object sender, System.EventArgs e){
        ballsLeft--;
        SoundController.Instance.PlaySoundBallLoss(PlayerBall.Instance.GetBallPosition());
        if(ballsLeft==0){
            state = State.GameOver;
        } else{
            state = State.BallLaunch;
        }
    }
    public void AddGold(int amount){
        
        if(GizmoManager.Instance.hasGoldBonus){
            if(PlayerPad.Instance.GetPadSize()==2){
                amount++;
            }
            if(PlayerPad.Instance.GetPadSize()==1){
                amount+=3;
            }
        }
        
        gold += amount;
        totalGold += amount;
    }
    private void PlayerBall_BallCaught(object sender, System.EventArgs e){
            state = State.BallLaunch;
    }

    private void GameManager_StateChanged(object sender, System.EventArgs e){

    }
   
    public bool IsBallLaunching() {
        return state == State.BallLaunch;
    }
     public bool IsGizmoShop() {
        return state == State.GizmoShop;
    }
    public bool IsGameOver() {
        return state == State.GameOver;
    }
    public bool IsGameWon() {
        return state == State.GameWon;
    }
    public bool IsCredits() {
        return state == State.Credits;
    }
    public void SetGameWon(){
        state = State.GameWon;
    }
    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }
    public bool IsChoosingPad() {
        return state == State.ChoosingPad;
    }
    public bool IsChoosingLvlUp() {
        return state == State.ChoosingLvlUp;
    }

    public int GetXp (){
        return xp;
    }
    public int LvLFormula(int lvl){
        if (lvl<0){
            return 0;
        } else {
            return 150*lvl + lvl*(lvl+1)*10;
        }  
    }
    public void AddXp(int amount){
        xp += amount;
        int ballSpeed = (int)(PlayerBall.Instance.GetBallSpeed());
        if(ballSpeed>4&&GizmoManager.Instance.hasXpBonus){
            xp+=(ballSpeed-4)*50;
        }
        int lvlTreshold= LvLFormula(lvl);
        if (xp>= lvlTreshold ){
            LevelUp();
        }
    }
    public int GetLevel(){
        return lvl;
    }
    public int GetBalls(){
        return ballsLeft;
    }
    public void LevelUp(){
        Debug.Log("Levelup " + xp );
        lvl++;
        lvlUpPoints++;
        GameSceneUI.Instance.LvlUpTooltipOn(); 
        SoundController.Instance.PlaySoundLevelUp(new Vector3(0,0,-7.5f));  
    }
    public void ChooseLvlUp(bool changePreLvlUpState){
        if(changePreLvlUpState){
            preLvlUpState = state;
        }
         state = State.ChoosingLvlUp;
         Time.timeScale = 0f;
         chooseUI.SetActive(true);
         if(createNewLvlUps){
            levelUps = GetLvlUps();
         }
         GameSceneUI.Instance.SetChooseCursor();
         chooseUI.GetComponent<ChooseUI>().SetLvlUpUI(levelUps,rollCost-(GizmoManager.Instance.hasRollDiscount?1:0));
    }
    public int[] GetLvlUps(){
        int[] lvlUps={-1,-1,-1};
        for (int i = 0; i < 3; i++)
        { 
            int converted = 0;
            if(PutGoldChoise(lvlUps)){
                converted = -1;
            } else {
                do
                {
                    int randomNumber = UnityEngine.Random.Range(1, 39);
                    switch (randomNumber)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            converted=0;
                            break;

                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            converted=1;
                            break;
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            converted=2;
                            break;
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                            converted=3;
                            break;  
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            converted=4;
                            break; 
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                            converted=5;
                            break;    
                        case 25:
                        case 26:
                        case 27:
                            converted=6;
                            break; 
                        case 28:
                        case 29:
                        case 30:
                            converted=7;
                            break; 
                        case 31:
                        case 32:
                        case 33:
                            converted=8;
                            break; 
                        case 34:
                        case 35:
                            converted=9;
                            break;  
                        case 36:
                        case 37:
                            converted=10;
                            break;      
                        case 38:
                            converted=11;
                            break;                 

                        default:
                            Debug.Log("Default Outcome");
                            break;
                    }
               
                 } while (lvlUps.Contains(converted) || powerUpLvls[converted].reachedMax);
            }
            lvlUps[i]=converted;
            Debug.Log("evo ga "+ converted.ToString());
        }
        levelUps= lvlUps;
        return lvlUps;
    }
    public float GetChance(){
        return dropChance;
    }
    public int GetGold(){
        return gold;
    }
    public int GetTotalGold(){
        return totalGold;
    }

    public void TogglePauseGame(){
        isGamePaused = !isGamePaused;
        if(isGamePaused){
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this,EventArgs.Empty);
        }else{
            if(!IsChoosingLvlUp()){
                Time.timeScale = 1f;
            }
            OnGameUnpaused?.Invoke(this,EventArgs.Empty);
        }   
    }

    public bool IsGamePaused(){
        return isGamePaused;
    }

    public GameObject GetStage(){
        return stage;
    }
    public int GetRedirectPower(){
        return redirectPower;
    }
    public int GetDropPower(){
        return dropPower;
    }
    public int GetLuckyRedirectDrops(){
        return luckyRedirectDrops;
    }
    public bool GetRedirectLuck(){
        return redirectLuck;
    }

    public void SetRedirectLuck(bool state){
        redirectLuck=state;
    }

    public PowerUpHandler GetPowerUpLvl(int i){
        return powerUpLvls[i];
    }
    public bool PutGoldChoise(int[] choises){
        for (int i = 0; i < powerUpLvls.Length; i++)
        {
            if((!choises.Contains(i))&&(!powerUpLvls[i].reachedMax)){
                return false;
            }
        }
        return true;
    } 
    public string GetRarity(int i){
        return powerUpLvls[i].rarity; 
    }
    public int GetStagesPassed(){
        return stagesPassed;
    }
    public int GetCurrentLvl(int i){
        return powerUpLvls[i].currentLvl;
    }
    public int GetMaxLvl(int i){
        return powerUpLvls[i].maxLvl;
    }
    public void DropCollected(){
        dropsCollected++;
    }
    public int GetDropsCollected(){
        return dropsCollected;
    }

    public void BlockPopped(){
        blocksPopped++;
        stageBlocks--;
        Debug.Log("ima ih "+BlockGenerator.Instance.transform.childCount.ToString());
        if (BlockGenerator.Instance.transform.childCount==1||superNothingUsed) {
            superNothingUsed=false;
            if(stagesPassed==2){
                stagesPassed++;
                if(SettingsUI.Instance.GetCredits()){
                Debug.Log("Tu Sam 1");
                state = State.Credits;  
                } else {
                   state= State.GameWon;
                }
                
            }  else {
                if(state!=State.Credits&&state!=State.GameWon){
                    state= State.GizmoShop;
                    GameSceneUI.Instance.StageClearOn();
                    PlayerBall.Instance.RefreshCD();
                }
                
            }  
        } 
    }
    public void NextStage(){
        stagesPassed++;
        state = State.BallLaunch;
        BlockGenerator.Instance.CreateStage();
    }
    public int GetBlocksPopped(){
        return blocksPopped;
    }
    public bool GetSuperNothingUsed(){
        return superNothingUsed;
    }
    public int GetLvlUpPoints(){
        return lvlUpPoints;
    }
    public bool IsChooseState(){
        return ((state==State.ChoosingPad)||(state==State.ChoosingLvlUp));
    }
    public void RollUp(){
        rollCost++;
    }
    public void AddBlockOnStage(){
        stageBlocks++;
    }

    private void ActivateCodes(){
        foreach (int code in CodesActivated.Instance.GetCodesActivated())
        {
            switch (code)
            {    
                case 0:
                    PlayerBall.Instance.AddAmmoMax(9999);
                    PlayerBall.Instance.AddAmmo(9999);
                break;
                case 1:
                    PlayerBall.Instance.AddBallPower(9999);
                break;
                case 2:
                    GizmoManager.Instance.MoveKeyOn();
                break;
                case 3:
                    GizmoManager.Instance.DropPhaserOn();
                break;
                case 4:
                    GizmoManager.Instance.TargetingKeyOn();
                break;
                case 5:
                    GizmoManager.Instance.XpBonusOn();
                break;
                case 6:
                    GizmoManager.Instance.BallCatcherOn();
                break;
                case 7:
                    GizmoManager.Instance.RollDiscountOn();
                break;
                case 8:
                    GizmoManager.Instance.GoldBonusOn();
                break;
                case 9:
                    GizmoManager.Instance.LaunchPowerOn();
                break;
                case 10:
                    GizmoManager.Instance.InspectorOn();
                break;
                case 11:
                    AddGold(500);
                break;
                case 12:
                    GizmoManager.Instance.MoveKeyOn();
                    GizmoManager.Instance.DropPhaserOn();
                    GizmoManager.Instance.TargetingKeyOn();
                    GizmoManager.Instance.XpBonusOn();
                    GizmoManager.Instance.BallCatcherOn();
                    GizmoManager.Instance.RollDiscountOn();
                    GizmoManager.Instance.GoldBonusOn();
                    GizmoManager.Instance.LaunchPowerOn();
                    GizmoManager.Instance.InspectorOn();
                break;
                case 13:
                     GizmoManager.Instance.SuperNothingOn();
                break;
                default:
                break;
            }
        }
    }
}
