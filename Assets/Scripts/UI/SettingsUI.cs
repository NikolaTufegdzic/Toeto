using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Linq;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set;}
    private bool settingsUp = false;
    private bool tutorialSkip = false;
    private bool credits = false;
    private int musicVolumeSet;
    private int fxVolumeSet;
    private string filePath = "Assets/Settings/Settings.txt";
    [SerializeField] private Button[] musicVolume;
    [SerializeField] private Button[] fxVolume;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button skipTutorialButton;
    [SerializeField] private Button skipCreditsButton;
    [SerializeField] private Button shootButton;
    [SerializeField] private Button shootAltButton;
    [SerializeField] private Button redirectButton;
    [SerializeField] private Button redirectAltButton;
    [SerializeField] private Button launchButton;
    [SerializeField] private Button launchAltButton;
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button levelUpAltButton;
    [SerializeField] private Button nothingButton;
    [SerializeField] private Button nothingAltButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button upAltButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button downAltButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button rightAltButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button leftAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button pauseAltButton;
    [SerializeField] private TMP_Text shootText;
    [SerializeField] private TMP_Text shootAltText;
    [SerializeField] private TMP_Text redirectText;
    [SerializeField] private TMP_Text redirectAltText;
    [SerializeField] private TMP_Text launchText;
    [SerializeField] private TMP_Text launchAltText;
    [SerializeField] private TMP_Text levelUpText;
    [SerializeField] private TMP_Text levelUpAltText;
    [SerializeField] private TMP_Text nothingText;
    [SerializeField] private TMP_Text nothingAltText;
    [SerializeField] private TMP_Text upText;
    [SerializeField] private TMP_Text upAltText;
    [SerializeField] private TMP_Text downText;
    [SerializeField] private TMP_Text downAltText;
    [SerializeField] private TMP_Text rightText;
    [SerializeField] private TMP_Text rightAltText;
    [SerializeField] private TMP_Text leftText;
    [SerializeField] private TMP_Text leftAltText;
    [SerializeField] private TMP_Text pauseText;
    [SerializeField] private TMP_Text pauseAltText;
    [SerializeField] private Sprite blank;
    [SerializeField] private Sprite marked;
    [SerializeField] private GameObject rebind;
    private void Awake(){
       Instance = this;
        for (int i = 0; i < 11; i++)
        {
            int buttonIndex = i;
                musicVolume[i].onClick.AddListener(()=>{
                    SoundController.Instance.PlaySoundClick();
                SetVolume(buttonIndex,true); 
            });
                fxVolume[i].onClick.AddListener(()=>{
                SoundController.Instance.PlaySoundClick();
                SetVolume(buttonIndex,false); 
            });
        }
                closeButton.onClick.AddListener(()=>{
                SoundController.Instance.PlaySoundClick();
                SetSettingsState(false);
                WriteSettings(); 
            });
                skipCreditsButton.onClick.AddListener(()=>{
                SoundController.Instance.PlaySoundClick();
                ToggleCredits();
            });
                skipTutorialButton.onClick.AddListener(()=>{
                SoundController.Instance.PlaySoundClick();
                ToggleTutorialSkip();
            });
       
        shootButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Shoot,shootText);
            });
        shootAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.ShootAlt,shootAltText);
            });
        redirectButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Redirect,redirectText);
            });
        redirectAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.RedirectAlt,redirectAltText);
            });
        launchButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Launch,launchText);
            });
        launchAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.LaunchAlt,launchAltText);
            });
        levelUpButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.LevelUp,levelUpText);
            });
        levelUpAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.LevelUpAlt,levelUpAltText);
            });
        nothingButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Nothing,nothingText);
            });
        nothingAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.NothingAlt,nothingAltText);
            });
        upButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Up,upText);
            });
        upAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.UpAlt,upAltText);
            });
        downButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Down,downText);
            });
        downAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.DownAlt,downAltText);
            });
        rightButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Right,rightText);
            });
        rightAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.RightAlt,rightAltText);
            });
        leftButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Left,leftText);
            });
       
        leftAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.LeftAlt,leftAltText);
            });
        pauseButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.Pause,pauseText);
            });
        pauseAltButton.onClick.AddListener(()=>{
               KeyRebind(GameInput.Binding.PauseAlt,pauseAltText);
            });
           
    }
    private void Start(){
        ReadFile();
        SetSettingsState(false);
        rebind.SetActive(false);
        SetVolume(musicVolumeSet,true);
        skipCreditsButton.gameObject.GetComponent<Image>().sprite = credits?marked:blank; 
        skipTutorialButton.gameObject.GetComponent<Image>().sprite = tutorialSkip?marked:blank; 
        UpdateButtons();
    }
    private void ActionOngoing(TMP_Text text){
        SoundController.Instance.PlaySoundSelect(); 
        rebind.SetActive(true);
        text.text = "";
    }
    private void ActionDone(TMP_Text text, string keyString){
        rebind.SetActive(false);
        FormatButtonText(keyString,text);
        SoundController.Instance.PlaySoundCatch(new Vector3(0, 0, -2f));
    }
    private void KeyRebind( GameInput.Binding binding, TMP_Text text){
        ActionOngoing(text);
        GameInput.Instance.RebindBinding(binding,() => ActionDone(text,GameInput.Instance.GetBindingText(binding)));
    }
    private void ReadFile(){
          if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length == 2)
                {
                    string variableName = parts[0].Trim();
                    string value = parts[1].Trim();
                    switch (variableName)
                    {
                        case "tutorialSkip":
                            tutorialSkip = bool.Parse(value);
                            break;
                        case "credits":
                            credits = bool.Parse(value);
                            break;
                        case "musicVolumeSet":
                            musicVolumeSet = int.Parse(value);
                            SetVolume(musicVolumeSet,true);
                            break;
                        case "fxVolumeSet":
                            fxVolumeSet = int.Parse(value);
                            SetVolume(fxVolumeSet,false);
                            break;
                        default:
                            Debug.LogWarning("Unknown variable in config file: " + variableName);
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid line in config file: " + line);
                }
            }
        }
        else
        {
            Debug.LogError("Config file not found: " + filePath);
        }
    }
    private void UpdateButtons(){
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Shoot),shootText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.ShootAlt),shootAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Redirect),redirectText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.RedirectAlt),redirectAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Launch),launchText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.LaunchAlt),launchAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.LevelUp),levelUpText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.LevelUpAlt),levelUpAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Nothing),nothingText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.NothingAlt),nothingAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Up),upText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.UpAlt),upAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Down),downText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.DownAlt),downAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.RightAlt),rightAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Left),leftText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Right),rightText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.LeftAlt),leftAltText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.Pause),pauseText);
        FormatButtonText(GameInput.Instance.GetBindingText(GameInput.Binding.PauseAlt),pauseAltText);
    }
    private void FormatButtonText(string text, TMP_Text label){
        text = (text=="Escape"?"ESC":text);
        text = (text=="Space"?"SPC":text);
        text = (text=="Up Arrow"?"↑":text);
        text = (text=="Down Arrow"?"↓":text);
        text = (text=="Right Arrow"?"→":text);
        text = (text=="Left Arrow"?"←":text);
        string[] stringArray = { "ESC", "RMB", "LMB", "MMB","SPC" };
        if (stringArray.Contains(text)){
            label.fontSize = 22;
        } else{
            label.fontSize = 36;
        }
        label.text=text;
    }
    private void ToggleTutorialSkip(){
        tutorialSkip = !tutorialSkip;
        skipTutorialButton.gameObject.GetComponent<Image>().sprite = tutorialSkip?marked:blank;
    }
    private void ToggleCredits(){
        credits = !credits;
         skipCreditsButton.gameObject.GetComponent<Image>().sprite = credits?marked:blank; 
    }
    private void SetVolume(int value, bool isMusic){
        if (isMusic) {
            musicVolumeSet = value;
            MusicController.Instance.GetComponent<AudioSource>().volume = value/10f;
        }else{
            fxVolumeSet = value;
        }
        for (int i = 0; i < 11; i++)
        {
            Color color = i<=value?Color.black:Color.white;
            if (isMusic) {
                musicVolume[i].GetComponent<Image>().color = color;
            }else{
                fxVolume[i].GetComponent<Image>().color = color;
            }
        }
        Debug.Log(value);
    }
    
    public void SetSettingsState(bool state){
        gameObject.SetActive(state);
        settingsUp = state;
        if(state){
            skipCreditsButton.gameObject.GetComponent<Image>().sprite = credits?marked:blank; 
            skipTutorialButton.gameObject.GetComponent<Image>().sprite = tutorialSkip?marked:blank; 
            UpdateButtons();
            ReadFile();
            SetVolume(musicVolumeSet,true);
            SetVolume(fxVolumeSet,false);
        }
    }
    public bool GetSkipTutorial(){
        return tutorialSkip;
    }
    public bool GetCredits(){
        return credits;
    }
    public bool GetSettingsUp(){
        return settingsUp;
    }
    public int GetFxVolume(){
        return fxVolumeSet;
    }
    public void WriteSettings()
    {
        string content = $"tutorialSkip = {tutorialSkip}\n" +
                         $"credits = {credits}\n" +
                         $"musicVolumeSet = {musicVolumeSet}\n" +
                         $"fxVolumeSet = {fxVolumeSet}";

        File.WriteAllText(filePath, content);
        Debug.Log("Config file updated.");
    }
   
}
