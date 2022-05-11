using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum Colorr
{
    red,
    blue,
}

public class MovingPlayer : MonoBehaviour
{
    public static MovingPlayer instance;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] public bool isMoving, pause, doubt, die, win, run;
    [SerializeField] private Transform check;
    [SerializeField] public float speed;
    [SerializeField] public int waypoinIndex;
  

    private void Awake()
    {
        instance = this;
      
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        Application.targetFrameRate = 60;
        
    }
    private void FixedUpdate()
    {
        Moving(GameController.instance, Hamster.instance, Cat.instance, SoundMananger.instance);
        if (run)
        {
            run = false;
        }
      

        if (die)
        {
            Invoke("Die", 0.1f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SoundMananger.instance.PlaySoundAction(4);
        }
        
    }
    private void OnDestroy()
    {
        DOTween.KillAll();
    }
    public void Moving(GameController gamecontroller, Hamster hamster, Cat cat, SoundMananger sound)
    {
        Vector3 target = gamecontroller.targets[waypoinIndex + 1].transform.position;
        float distToPlayer = Vector2.Distance(transform.position, target);

        #region xử lý khi đi đến 1 vật thể check điều kiện với ký tự bấm trước đó
        if (isMoving && !pause && !die)
        {
            if (waypoinIndex <= gamecontroller.targets.Count - 1)
            {              
                if (transform.position == gamecontroller.targets[0].transform.position)
                {                    
                    cat.PlayAnimCat(3);
                    sound.PlaySoundAction(7);
                    // run = true;
                    Invoke("Run", 0.5f);

                }

                Hamster.instance.PlayAnimHamster(1);
                transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target && waypoinIndex < gamecontroller.data.question[gamecontroller.index].soluongchuoi.Length)
                {
                    doubt = false;
                    CheckGamePlay(gamecontroller, hamster, cat, sound);
                    //  SoundMananger.instance.PlaySoundAction(0);                  
                }               
                if (transform.position == gamecontroller.targets[gamecontroller.targets.Count - 2].transform.position)
                {
                    audioSource.Stop();
                    win = true;
                    pause = true;
                    FirebaseMananger.LogEventLevelComplete(GameController.instance.index);
                    StartCoroutine(Finish());
                    gamecontroller.ClaimCoin();
                }              
            }
            else
            {
                return;

            }
        }     
        #endregion
    }

    void CheckGamePlay(GameController gamecontroller, Hamster hamster, Cat cat, SoundMananger sound)
    {
        if (gamecontroller.addItem[waypoinIndex].colorr == Colorr.blue)
        {
            if (checkList(gamecontroller))
            {                        
                cat.PlayAnimCat(4);         
                hamster.PlayAnimHamster(0);
                sound.PlaySoundAction(6);
                pause = true;
                audioSource.Stop();
                StartCoroutine(ActionBlue(1f, 2.5f, 1.5f));               
            }
            else
            {
                cat.PlayAnimCat(4);            
                waypoinIndex += 1;       
                StartCoroutine(ActionFaild());
          
                for (int i = 0; i < gamecontroller.buttonSwitch.Length; i++)
                {
                    int ind = i;
                    gamecontroller.buttonSwitch[i].interactable = false;
                }

                foreach (Icon j in gamecontroller.chainIconList)
                {
                    j.normal = false;
                    j.isTrue = false;
                }
            }
        }

        else if (gamecontroller.addItem[waypoinIndex].colorr == Colorr.red)
        {
            if (checkList(gamecontroller))
            {
                doubt = true;     
                cat.PlayAnimCat(4);
                Invoke("IsDoubt", 0.5f);            
                waypoinIndex += 1;               
                StartCoroutine(InitTrueRed(2f,0.1f));
            }
            else
            {              
                pause = true;
                cat.PlayAnimCat(4);
                hamster.PlayAnimHamster(0);              
                sound.PlaySoundAction(6);
                audioSource.Stop();
                StartCoroutine(ActionFaild());

                for (int i = 0; i < gamecontroller.buttonSwitch.Length; i++)
                {
                    int ind = i;
                    gamecontroller.buttonSwitch[i].interactable = false;
                }

                foreach (Icon j in gamecontroller.chainIconList)
                {
                    j.normal = false;
                    j.isTrue = false;
                }
            }
        }
    }

    public bool checkList(GameController gamecontroller)
    {
        if (gamecontroller.topIndChain ==gamecontroller.data.question[gamecontroller.index].soluongchuoi[gamecontroller.indexIcon - 1])
        {
            foreach (bool i in gamecontroller.check_Chain)
            {
                if (i == false)
                {               
                    return false;               
                }
            }
            return true;
        }
        else
        {       
            return false;
        }
    }

    IEnumerator ActionBlue(float doubt, float detected, float see)
    {
        yield return new WaitForSeconds(doubt);       
        IsDoubt();
        yield return new WaitForSeconds(0.8f);
        SoundMananger.instance.PlaySoundAction(5);
        yield return new WaitForSeconds(detected);
        ShibaHide();      
        yield return new WaitForSeconds(see);
        See();
        yield return new WaitForSeconds(0.5f);
     //   GameController.instance.pause = true;
        StartCoroutine(GameController.instance.InitListChair());
    }

    IEnumerator ActionFaild()
    {
        yield return new WaitForSeconds(0.5f);
        IsDoubt();
        yield return new WaitForSeconds(0.8f);
        SoundMananger.instance.PlaySoundAction(5);
        yield return new WaitForSeconds(1f);    
        CheckDie();  
        yield return new WaitForSeconds(0.1f); 
        ShibaHide();  
    }

    IEnumerator InitTrueRed(float detected, float run)
    {
        yield return new WaitForSeconds(1f);
        //GameController.instance.pause = true;
        StartCoroutine(GameController.instance.InitListChair());
        yield return new WaitForSeconds(detected);
        ShibaHide();     
        yield return new WaitForSeconds(run);
        Cat.instance.PlayAnimCat(3);

        //if (waypoinIndex < GameController.instance.indexIcon)
        //{
        //    SoundMananger.instance.PlaySoundAction(6);
        //}
       
        yield return new WaitForSeconds(1f);
        doubt = false;
    }

    public void See()
    {
        Cat.instance.PlayAnimCat(3);
        
        if (pause && !die)
        {
           // Run();
            waypoinIndex += 1;
            pause = false;
            Run();
        }
    }

    public void IsDoubt()
    {
        
        Cat.instance.PlayAnimCat(2);
    }

    public void ShibaHide()
    {
        if (!die)
        {
            Cat.instance.PlayAnimCat(5);
         
            SoundMananger.instance.PlaySoundAction(2);
        }
        else
        {
            Cat.instance.PlayAnimCat(6);
            
        }
    }

    public void CheckDie()
    {
        die = true;
        FirebaseMananger.LogEventLevelLose(GameController.instance.index);
        SoundMananger.instance.PlaySoundAction(3);
        audioSource.Stop();
    }

    void Die()
    {
        pause = true;     
        Hamster.instance.PlayAnimHamster(2);  
        GameController.instance.Lose.SetActive(true);
        GameController.instance.Ui[4].gameObject.SetActive(false);
        
    }

    IEnumerator Finish()
    {
        int ran = Random.Range(3, 6);
        Hamster.instance.PlayAnimHamster(ran);
        yield return new WaitForSeconds(1.5f);
        Popcorn.instance.PlayAnimationPopcorn(1);
        SoundMananger.instance.PlaySoundAction(8);
        GameController.instance.Win.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Popcorn.instance.PlayAnimationPopcorn(2);
        yield return new WaitForSeconds(1f);
        Hamster.instance.transform.position = GameController.instance.targets[GameController.instance.targets.Count - 1].position;
        Hamster.instance.PlayAnimHamster(6);
        Popcorn.instance.PlayAnimationPopcorn(3);
        yield return new WaitForSeconds(0.5f);
        Hamster.instance.PlayAnimHamster(7);
        

    }

    void Run()
    {
        run = true;
       // Debug.Log(run);
        if (run)
        {
            audioSource.Play();
           // audioSource.loop = true;
        }
      
    }
}

