using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DestroyGameObject : MonoBehaviour
{
    public bool start;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void OnEnable()
    {
        if (start)
        {
            transform.localScale = Vector2.zero;
            transform.DOScale(new Vector2(1, 1), 0.5f).SetEase(Ease.OutBounce).SetDelay(0.1f).OnComplete(() =>
            {
                transform.DOScale(new Vector2(0, 0), 0.5f).SetEase(Ease.InBounce).SetDelay(1f);
        });
        }
        else
        {
            transform.localScale = Vector2.zero;
            transform.DOScale(new Vector2(1, 1), 0.5f).SetEase(Ease.OutBounce).SetDelay(0.5f).OnComplete(()=>
            {
                transform.DOScale(new Vector2(0.8f, 0.8f), 0.5f).SetEase(Ease.OutBounce).SetLoops(-1,LoopType.Yoyo).SetDelay(5f);
            });
        }
    } 
    private void OnDestroy()
    {
        DOTween.KillAll();
    }

}
