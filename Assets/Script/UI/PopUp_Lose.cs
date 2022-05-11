using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class PopUp_Lose : MonoBehaviour
{
    [SerializeField] Image header;
    [SerializeField] Image Faild;
    [SerializeField] Button close;
    private float num= 1;
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, -2000f);
        Invoke("MathTime", 1f);
    }

    private void OnEnable()
    {
        this.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-20, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
      //  Invoke("MathTime", 1f);
    }
    void MathTime()
    {
        //if (num > 0)
        //{
        //    num -= 0.005f;
        // //   header.fillAmount = num;
        //    if (num <= 0.6f)
        //    {
        //        close.gameObject.SetActive(true);
        //    }
        //    if (num <= 0.01f)
        //    {
        //        gameObject.SetActive(false);
                Close();
          //  }
        //}
        //else
        //{
        //    return;
        //}
    }

    void GameLose()
    {
        GameController.instance.Finish();
    }

    public void Close()
    {
      //  num = -1;
       // SoundMananger.instance.PlaySoundAction(1);
     //   Faild.gameObject.SetActive(true);
        Invoke("GameLose", 1.5f);
    }

    private void OnDisable()
    {
        this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(20f, -1460f);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

}
