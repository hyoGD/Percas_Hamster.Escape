using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UI_Lobby : MonoBehaviour
{
    [SerializeField] RectTransform topPanel;
    [SerializeField] RectTransform lstChain;

    [SerializeField] RectTransform[] listbtuonLeft;  
    // [SerializeField] RectTransform[] listButtonRight;
    [SerializeField] RectTransform[] buttonMain;

    private void Awake()
    {
        
    }
    private void Start()
    {
        for(int i=0;i < buttonMain.Length; i++)
        {
            buttonMain[0].DOAnchorPosY(250f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
        }
        listbtuonLeft[0].DOAnchorPosX(80f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);

        if (AdsMananger.Instance.bannerCheck)
        {
            topPanel.anchoredPosition = new Vector2(topPanel.anchoredPosition.x,topPanel.anchoredPosition.y- AdsMananger.Instance.GetBannerHeight());
        }
    }

    private void OnEnable()
    {
        lstChain.anchoredPosition = new Vector2(938f, 76f);
        lstChain.DOAnchorPos(new Vector2(0f, 76f), 1f).SetEase(Ease.OutQuart).SetDelay(0.5f);
    }

    async void AsyncBaBy()
    {
        foreach (var button in buttonMain)
        {
           await button.DOAnchorPosY(250, 0.1f).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            
        }      
    }

    private void OnDisable()
    {
        lstChain.DOAnchorPos(new Vector2(-725f, 76f), 0.5f).SetEase(Ease.InQuart);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }


}
