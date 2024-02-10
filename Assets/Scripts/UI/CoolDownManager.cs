using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownManager : MonoBehaviour
{
    [SerializeField] private Image firstProgress;
    [SerializeField] private Image secondProgress;
    [SerializeField] private Image xpProgress;



    private void Update()
    {
        ProgressBar(firstProgress,PlayerBall.Instance.GetFirstCD(),PlayerBall.Instance.GetFirstTimer(),true);
        ProgressBar(secondProgress,PlayerBall.Instance.GetSecondCD(),PlayerBall.Instance.GetSecondTimer(),true);
        int lvl = GameManager.Instance.GetLevel();
        int barXP = GameManager.Instance.GetXp()-GameManager.Instance.LvLFormula(lvl-1);
        int neededXP = GameManager.Instance.LvLFormula(lvl) - GameManager.Instance.LvLFormula(lvl-1); 
        ProgressBar(xpProgress,neededXP,barXP,false);
    }
    private void ProgressBar(Image bar, float cd, float timer,bool showOnZero){
        if(timer == 0){
            if(showOnZero){
                bar.fillAmount = 1f;
            } else {
                bar.fillAmount = 0f;    
            }
            
        }else{
            bar.fillAmount = timer/cd;
        }
    }
}
