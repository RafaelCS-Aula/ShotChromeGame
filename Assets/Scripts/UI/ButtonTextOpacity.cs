using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTextOpacity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Text buttonText;
    private Color32 hoverColor = new Color32(192, 168, 135, 100); 
    private Color32 normalColor = new Color32(192, 168, 135, 255); 

    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }
}