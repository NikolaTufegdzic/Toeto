using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FrameSlider : MonoBehaviour
{
    [SerializeField] private GameObject leftSlider;
    [SerializeField] private GameObject rightSlider;
    [SerializeField] private GameObject leftBottom;
    [SerializeField] private GameObject leftTop;
    [SerializeField] private GameObject rightBottom;
    [SerializeField] private GameObject rightTop;
    [SerializeField] private Sprite[] spritesLeft;
    [SerializeField] private Sprite[] spritesRight;
    private int leftYCoord;
    private Vector3 down = new Vector3(0,-1,0);
    RectTransform leftRectTransform;
    RectTransform rightRectTransform;
    Vector2 leftStart;
    Vector2 rightStart;
    private int spriteIDLeft;
    private int spriteIDRight;

    private void Start()
    {
        spriteIDLeft = Random.Range(0, 10);
        spriteIDRight = Random.Range(0, 10);
        leftRectTransform = leftSlider.GetComponent<RectTransform>();
        leftStart = leftRectTransform.anchoredPosition;
        rightRectTransform = rightSlider.GetComponent<RectTransform>();
        rightStart = rightRectTransform.anchoredPosition;
        leftBottom.GetComponent<Image>().sprite = spritesLeft[spriteIDLeft];
        leftTop.GetComponent<Image>().sprite = spritesLeft[(spriteIDLeft!=0)?spriteIDLeft-1:9];
        rightBottom.GetComponent<Image>().sprite = spritesRight[(spriteIDRight!=0)?spriteIDRight-1:9];
        rightTop.GetComponent<Image>().sprite = spritesRight[spriteIDRight];
    }
    private void Update()
    {
        leftSlider.transform.position +=down*Time.deltaTime*35;
        rightSlider.transform.position -=down*Time.deltaTime*35;
        if(leftSlider.GetComponent<RectTransform>().anchoredPosition.y<-1080f){
            spriteIDLeft--;
            if(spriteIDLeft<0){
                spriteIDLeft = 9;
            }
            spriteIDRight--;
            if(spriteIDRight<0){
                spriteIDRight = 9;
            }
            leftBottom.GetComponent<Image>().sprite = spritesLeft[spriteIDLeft];
            rightTop.GetComponent<Image>().sprite = spritesRight[spriteIDRight];

            leftTop.GetComponent<Image>().sprite = spritesLeft[(spriteIDLeft!=0)?spriteIDLeft-1:9];
            rightBottom.GetComponent<Image>().sprite = spritesRight[(spriteIDRight!=0)?spriteIDRight-1:9];
           
            rightRectTransform.anchoredPosition = rightStart;
            leftRectTransform.anchoredPosition = leftStart;
        }
    }
}
