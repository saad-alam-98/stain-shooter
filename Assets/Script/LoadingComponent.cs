using TMPro;
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingComponent : MonoBehaviour
{
    public Image imageOverlay;
    public Transform logo;                
    public TextMeshProUGUI textPresents; 
    public Transform logo2;              
    public TextMeshProUGUI description;  
    public CanvasGroup canvasGroup;

    public Button Continue;

    public float logoScaleDuration = 1f;     
    public float textTypingSpeed = 0.05f;    
    public float fadeDuration = 1f;          
    public float delayBetweenSteps = 0.5f;

    private Action CallBack;

    void Start()
    {
        Continue.onClick.AddListener(OnContinueButtonClick);
    }

    void OnContinueButtonClick()
    {
        CallBack();
    }

    private void OnDisable()
    {
        Continue.onClick.RemoveListener(OnContinueButtonClick);
    }

    public void PlaySequence(Action CallBack)
    {
        this.CallBack = CallBack;
        Continue.gameObject.SetActive(false);
        logo.transform.localScale = Vector3.zero;
        textPresents.text = "";
        logo2.localScale = Vector3.zero;
        description.text = "";

        SoundManager.instance.OnWashing();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(imageOverlay.DOFade(0, 1));

        logo.localScale = Vector3.zero;
        sequence.Append(logo.DOScale(1, logoScaleDuration).SetEase(Ease.OutQuad));
        sequence.AppendCallback(() =>
        {
            PlayTypingEffect(textPresents, "PRESENTS");
        });
        sequence.AppendInterval(textPresents.text.Length * textTypingSpeed + delayBetweenSteps);

        sequence.AppendCallback(() =>
        {
            logo2.localScale = Vector3.zero;

            logo2.DOScale(1.15f, 0.3f)
                 .OnComplete(() =>
                 {
                     logo2.DOScale(1f, 0.11f);
                 });

            PlayTypingEffect(description, "Clean the clothes\r\nin <color=#FF3693>30 Seconds</color>!");
        });
        sequence.AppendInterval(1 + delayBetweenSteps);
        sequence.AppendInterval(1);
        sequence.Append(canvasGroup.DOFade(0, fadeDuration).SetEase(Ease.InQuad));
        sequence.Play()
            .OnComplete(
            () => {
                CallBack();
                //Continue.gameObject.SetActive(true);
            }
        );
    }

    private void PlayTypingEffect(TextMeshProUGUI textComponent, string fullText)
    {
        textComponent.text = ""; 
        textComponent.maxVisibleCharacters = 0;

        textComponent.text = fullText;

        DOTween.To(() => textComponent.maxVisibleCharacters,
                   x => textComponent.maxVisibleCharacters = x,
                   fullText.Length,
                   fullText.Length * textTypingSpeed)
               .SetEase(Ease.Linear);
    }
}
