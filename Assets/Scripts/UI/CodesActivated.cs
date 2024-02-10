using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodesActivated : MonoBehaviour
{
    public static CodesActivated Instance { get; private set;}
    private List<int> codesActivated = new List<int>();
    private void Awake()
    {
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

    public void AddCodeActivated(int code){
        codesActivated.Add(code);
        SoundController.Instance.PlaySoundLevelUp(new Vector3(0,0,-9.5f));
    }
    public List<int> GetCodesActivated(){
        return codesActivated;
    }
    public void ClearList(){
        codesActivated.Clear();
    }
}
