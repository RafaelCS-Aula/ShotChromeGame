using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;

/// <summary>
/// Handles the effects of the buttons
/// </summary>
public class ButtonFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Button's audioSource
    private AudioSource audioSource;

    [SerializeField] private bool hasUnderline;


    [ShowIf("hasUnderline"), SerializeField] private GameObject underLine;

    // AudioClips to use when the player hovers over
    // the buttons or clicks on them
    [SerializeField] private AudioClip hoverAudio;
    [SerializeField] private AudioClip clickAudio;

    [SerializeField] private Color clickColor;

    private TextMeshProUGUI tmPro;
    private Image underLineImage;
    private Color originalColor;

    /// <summary>
    /// Sets what the class does when it loads
    /// </summary>
    void Start()
    {
        tmPro = GetComponentInChildren<TextMeshProUGUI>();
        if (hasUnderline) underLineImage = underLine.GetComponent<Image>();

        audioSource = GetComponent<AudioSource>();

        if (hasUnderline) underLine.SetActive(false);
        originalColor = tmPro.color;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Displays the underline
        if (hasUnderline) underLine.SetActive(true);
        else tmPro.color = clickColor;

        // Plays one shot of the hoverAudio
        audioSource.PlayOneShot(hoverAudio);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Stops displaying the underline
        if (hasUnderline) underLine.SetActive(false);
        else tmPro.color = originalColor;
    }

    public void ClickEffects()
    {
        tmPro.color = clickColor;
        if (hasUnderline) underLineImage.color = clickColor;

        // Plays one shot of the given ClickAudio
        audioSource.PlayOneShot(clickAudio);
    }

    public void OnDisable()
    {
        if (hasUnderline)
        {
            underLine.SetActive(false);
            underLineImage.color = originalColor;
        }
        tmPro.color = originalColor;
    }
}