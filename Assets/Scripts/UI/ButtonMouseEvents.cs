using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonMouseEvents : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{   
    [SerializeField] private GameObject text;


    private void Start(){
        text.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.SetActive(false);
    }

}