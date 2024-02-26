using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set;}
    [SerializeField] private AudioClip[] launchSound;
    [SerializeField] private AudioClip[] popSound;
    [SerializeField] private AudioClip[] levelUpSound;
    [SerializeField] private AudioClip[] redirectSound;
    [SerializeField] private AudioClip[] shootSound;
    [SerializeField] private AudioClip[] clickSound;
    [SerializeField] private AudioClip[] thudSound;
    [SerializeField] private AudioClip[] deepThudSound;
    [SerializeField] private AudioClip[] selectSound;
    [SerializeField] private AudioClip[] dropSound;
    [SerializeField] private AudioClip[] coinsSound;
    [SerializeField] private AudioClip[] ballLossSound;
    [SerializeField] private AudioClip[] catchSound;
    [SerializeField] private AudioClip[] slideSound;
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
        
    }
    public void PlaySoundLevelUp (Vector3 position){
        PlaySound(levelUpSound, position);
    }
     public void PlaySoundDrop (){
        PlaySound(dropSound, new Vector3(0, 0, -5));
    }
    public void PlaySoundClick (){
        PlaySound(clickSound, Camera.main.transform.position, 1f);
    }
    public void PlaySoundSelect (){
        PlaySound(selectSound, Camera.main.transform.position, 0.5f);
    }
    public void PlaySoundSlide (){
        PlaySound(slideSound, Camera.main.transform.position, 0.8f);
    }
    public void PlaySoundCoins (){
        PlaySound(coinsSound, Camera.main.transform.position, 0.7f);
    }
    public void PlaySoundShoot (Vector3 position){
        PlaySound(shootSound, position+ new Vector3(0, 0, -5));
    }
    public void PlaySoundCatch (Vector3 position){
        PlaySound(catchSound, position+ new Vector3(0, 0, -7));
    }
    public void PlaySoundBallLoss (Vector3 position){
        PlaySound(ballLossSound, position+ new Vector3(0, 0, -9.5f));
    }
    public void PlaySoundThud (Vector3 position, float volume){
        PlaySound(thudSound, position+ new Vector3(0, 0, -5), volume);
    }
    public void PlaySoundDeepThud (Vector3 position, float volume){
        PlaySound(deepThudSound, position+ new Vector3(0, 0, -5), volume);
    }
    public void PlaySoundRedirect (Vector3 position){
        PlaySound(redirectSound, position+ new Vector3(0, 0, -8.5f));
    }
     public void PlaySoundLaunch (Vector3 position){
        PlaySound(launchSound, position+ new Vector3(0, 0, -5));
    }
    public void PlaySoundPop(Vector3 position){
        PlaySound(popSound, position+ new Vector3(0, 0, -5), 1);
    }
    public void PlaySound(AudioClip[] soundArray, Vector3 position, float volume =1f){
        volume*=SettingsUI.Instance.GetFxVolume();
        float timeScale = Time.timeScale;
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(soundArray[Random.Range(0,soundArray.Length)],position,volume);
        Time.timeScale = timeScale;
    }
}
