using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance { get; private set;}

    public Vector3 mousePosition;
    public Vector3 mouseWorldPosition;

    private void Awake(){
        Instance = this;
    }
    
    private void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.z = 0;
        
    }

}
