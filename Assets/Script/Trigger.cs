using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Coin"))
        {
            SoundMananger.instance.PlaySoundAction(9);
            GameController.instance.Coin++;
            collision.transform.DOMoveY(2.8f, 0.5f).SetEase(Ease.OutQuart);
            collision.transform.DOScale(new Vector2(0, 0), 0.5f).SetEase(Ease.InBounce);
            Destroy(collision.gameObject, 0.5f);
        }
    }
}
