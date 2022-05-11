using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
public class Loading : MonoBehaviour
{
    [SerializeField] Text load;
    [SerializeField] Image Checking;
    [SerializeField] Image CatLoad;
    [SerializeField] Button tryAgain;
    [SerializeField] SoundMananger soundManager;
    private float num;
    private bool checkInternet;
    private SoundMananger sound;
    // Start is called before the first frame update
    void Start()
    {
        sound = Instantiate(soundManager, Vector2.zero, Quaternion.identity);

        CatLoad.fillAmount += 0.1f;

    StartCoroutine ( CheckInternetConnection((check)=>
       {
           if (!check)
           {
               Checking.gameObject.SetActive(true);
           }
           else
           {
               checkInternet= true;
           }
       }));
    }

    // Update is called once per frame
    void Update()
    {

        if (checkInternet)
        {
            num += 0.1f;
          //  load.text = "Loading: " + num + "%";
            CatLoad.fillAmount = num;
            if (num >= 1f)
            {
                sound.audioSourceMusic.Play();
                sound.audioSourceMusic.loop = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }          
        }
    }
    IEnumerator CheckInternetConnection(Action<bool> checkInternet)
    {
      //  yield return new WaitForSeconds(0.5f);
        UnityWebRequest request = new UnityWebRequest("http://google.com");
        yield return request.SendWebRequest();
       // request.timeout = 1;
        if (request.error != null)
        {
            checkInternet(false);
          //  Debug.Log("Not Internet");
        }
        else
        {
            checkInternet(true);

          //  Debug.Log("On Internet");
        }
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
