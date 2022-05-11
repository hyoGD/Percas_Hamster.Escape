using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum type
{
    vuong,
    tron,
    tamgiac,
}

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [Header("Button")]
    [SerializeField] public Button[] buttonSwitch;
    [SerializeField] private Slider s_distance;
    [SerializeField] private TextMeshProUGUI txtCoin;

    [Header("UI")]
    [SerializeField] public RectTransform[] Ui;
    [SerializeField] public GameObject Win;
    [SerializeField] public GameObject Lose;
    [SerializeField] private GameObject startImage;
    [SerializeField] private GameObject ReddyImage;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private TextMeshProUGUI txtLevelCurrent;
    [SerializeField] private TextMeshProUGUI txtLevelNext;
    [SerializeField] private TextMeshProUGUI claimTxt;
    [SerializeField] private TextMeshProUGUI claimx2Txt;

    [Header("Scripts, Data")]
    [SerializeField] private Tutorial tutor;
    [SerializeField] public Data data;

    [Header("Object va bien dem")]
    [SerializeField] private MovingPlayer character;  
    [SerializeField] private GameObject cat;
    [SerializeField] public int index;
    [SerializeField] public type[] loaihinh;
    public List<Icon> chainIconList = new List<Icon>();
    public List<type> addChain = new List<type>();
    [SerializeField] GameObject CoinPrefab;
    [SerializeField] List<Transform> coinList;
    [SerializeField] private List<bool> addOpaque = new List<bool>();
    [SerializeField] public List<ObjType> addItem = new List<ObjType>();
    [SerializeField] public List<bool> check_Chain;
    [SerializeField] Icon chainPrefab;
    private Icon s_Chain; //obj chua obj duoc khoi tao tu prefab
    [SerializeField] Transform hidd_tranform;
    [SerializeField] public RectTransform c_tranform;// endRectranform;
    [SerializeField] public List<Transform> targets;
    [SerializeField] public int topIndChain;
    private float distance;
    public bool pause, pick;
    private bool normalSpeed = true;
    public int indexIcon;
    public int Coin, saveCoin;
    public int a_Start, a_Current;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        #region search scripts    
        if (tutor == null)
        {
            tutor = GameObject.Find("Tutorial").GetComponent<Tutorial>();
        }

        #endregion

        index = PlayerPrefs.GetInt("Index", 0);
        saveCoin = PlayerPrefs.GetInt("Coin", 0);
        foreach (type t in data.chainArr)      //add tat ca phan tu type t trong mang type chainArr trong Data
        {
            addChain.Add(t);
        }
        MaxSdk.ShowBanner(AdsMananger.Instance.banner_AdUnitId);  //show banner;
        AdsMananger.Instance.bannerCheck = true;
        if (AdsMananger.Instance.bannerCheck == true)
        {
            FirebaseMananger.LogEventShowBanner("Banner is Show");
        }
    }
    void Start()
    {
      
        SetUpGame();
        for(int i=0; i< CoinPrefab.transform.childCount; i++)
        {
            coinList.Add(CoinPrefab.transform.GetChild(i));
        }
       foreach(Transform i in coinList)
        {
            i.DOMoveY(1.5f, 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        if(index == 0 || index == 10)
        {
            tutor.gameObject.SetActive(true);
        }
        #region tinh toan khoang cach tu diem start den diem cuoi(diem dich)
        distance = ((targets[targets.Count - 2].transform.position /*+ Vector3.right*/) - MovingPlayer.instance.transform.position).magnitude; //koang cach tu vi tri dau den vi tri ve dich(vi tri cuoi cug)
        s_distance.maxValue = distance;
        #endregion

      //  Debug.Log(AdsMananger.Instance.GetBannerHeight());
    }

    private void OnEnable()
    {      
        {
            for (int i = 0; i < buttonSwitch.Length; i++)
            {
                int ind = i;
                buttonSwitch[i].onClick.AddListener(() => Click(loaihinh[ind]));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 60;
        if (topIndChain == data.question[index].soluongchuoi[indexIcon - 1])
        {
           // pause = false;
            MovingPlayer.instance.isMoving = true;
            Invoke("SetUi", 0.5f);
     
        }
     
        txtCoin.text =(saveCoin+ Coin).ToString();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }      
       
    }
    private void FixedUpdate()
    {
        distance = (targets[targets.Count - 2].transform.position  - MovingPlayer.instance.transform.position).magnitude;
        s_distance.value = distance;
        if (MovingPlayer.instance.waypoinIndex >= data.question[index].soluongchuoi.Length)
        {          
            Ui[4].gameObject.SetActive(false);
        }
      
        if (distance <= 0.1f)
        {
            int lv = index;
            lv++;
            PlayerPrefs.SetInt("Index", lv);
            Time.timeScale = 1;          
        }
        
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public void SetUpGame()
    {
        if (ReddyImage != null)
        {
            ReddyImage.SetActive(true);
        }
        #region ShowButton theo lv
        if (index == 0)
        {          
                buttonSwitch[1].gameObject.SetActive(false);
                buttonSwitch[2].gameObject.SetActive(false);            
        }
        else
        {
            if(index < 5)
            {
                buttonSwitch[1].gameObject.SetActive(false);
            }           
        }
        #endregion

        #region tinh toan thong so trong Game bang voi du lieu trong data 

        if (index >= data.question[data.question.Count - 1].id - 1)
        {
            index = data.question[data.question.Count - 1].id - 1;
            PlayerPrefs.SetInt("Index", index);

            txtLevelCurrent.text = data.question[data.question.Count - 1].id.ToString();
            txtLevelNext.text = data.question[data.question.Count - 1].id.ToString();

            // Debug.Log("index da max");
        }
        else if (index < data.question[data.question.Count - 1].id - 1)
        {
            if (data.question[index].id < 9)
            {
                txtLevelCurrent.text = "0" + data.question[index].id.ToString();
                txtLevelNext.text = "0" + data.question[index + 1].id.ToString();
            }
            else if (data.question[index].id >= 9)
            {
                txtLevelCurrent.text = data.question[index].id.ToString();
                txtLevelNext.text = data.question[index + 1].id.ToString();
            }
            // Debug.Log("index chua max");
        }
        txtLevel.text = "Level " + txtLevelCurrent.text;
        #endregion
        // StartCoroutine(InitListChair());
        #region InitListChain
        for (int i = 0; i < data.question[index].soluongchuoi[indexIcon]; i++)
        {
            s_Chain = Instantiate(chainPrefab, c_tranform); //khoi tao chuoi bang prefab Chain                                                          
            chainIconList.Add(s_Chain);   //add chuoi vua khoi tao vao list chainIconList de sau de su dung        
            if (index == 0)
            {
                s_Chain.t_icon = addChain[1];
                s_Chain.opaque = data.question[i].colorr;
            }
            else
            {             
                if (index < 10)
                {
                    s_Chain.opaque = data.question[i].colorr = true;
                    if (index < 5)
                    {
                        s_Chain.t_icon = addChain[Random.Range(0, addChain.Count - 1)];

                    }
                    else
                    {
                        s_Chain.t_icon = addChain[Random.Range(0, addChain.Count)];

                    }

                }
                else if (index == 10)
                {
                    s_Chain.t_icon = addChain[1];
                    s_Chain.opaque = !data.question[i].colorr;
                }
                else
                {
                    s_Chain.t_icon = addChain[Random.Range(0, addChain.Count)];
                    int a = Random.Range(a_Start, 2);
                    s_Chain.opaque = data.question[i].colorr =a == 1 ? true : false;
                    if (a == 0)
                    {
                        a_Start++;
                    }
                }

            }
            addOpaque.Add(s_Chain.opaque);
        }
        indexIcon++;
        #endregion

        #region Init Obj
        for (int i = 0; i < data.question[index].Obj; i++)
        {
            if (i == 0)
            {
                GameObject test = Instantiate(/*addItem[i]*/data.start, new Vector2(i + 0.4f, 2.5f), Quaternion.identity);
                test.transform.SetParent(hidd_tranform.transform);
                targets.Add(test.transform.GetChild(0).transform);
            }
            else
            {
                if (i < data.question[index].Obj - 1)
                {
                    // int n = 0;
                    if (index <= 5)
                    {
                        int ran = Random.Range(0, 3);
                        GameObject test = Instantiate(/*addItem[i]*/data.Obj[ran], new Vector2(i * 15f, 1.5f), Quaternion.identity);
                        test.transform.SetParent(hidd_tranform.transform);
                        targets.Add(test.transform.GetChild(0).transform);
                        addItem.Add(test.GetComponent<ObjType>());
                    }
                    else
                    {
                        int ran = Random.Range(0, 6);
                        GameObject test = Instantiate(/*addItem[i]*/data.Obj[ran], new Vector2(i * 15f, 1.5f), Quaternion.identity);
                        test.transform.SetParent(hidd_tranform.transform);
                        targets.Add(test.transform.GetChild(0).transform);
                        addItem.Add(test.GetComponent<ObjType>());
                    }
                    // n++;

                }
                else if (i == data.question[index].Obj - 1)
                {
                    GameObject test = Instantiate(/*addItem[i]*/ data.Finish, new Vector2(i * 15f, 1.5f), Quaternion.identity);
                    test.transform.SetParent(hidd_tranform.transform);
                    targets.Add(test.transform.GetChild(0).transform);
                    targets.Add(test.transform.GetChild(1).transform);
                }
            }
        }
        #endregion

        #region Init DecoItem
        for (int i = 0; i <= data.itemDeco.Count - 1; i++)
        {
            GameObject itemDeco;
            float x = Random.Range(5f, 50f);
            //  if(data.itemDeco[i].transform.position ) check dk de iteam deco ko nam len nhau
            itemDeco = Instantiate(data.itemDeco[i], new Vector2(i + x, 3f), Quaternion.identity);
            itemDeco.transform.SetParent(hidd_tranform.transform);
        }
        #endregion

        MovingPlayer player = Instantiate(character, new Vector2(0f,0.5f), Quaternion.identity);
        GameObject Cat = Instantiate(cat, new Vector2(0, 3.47f), Quaternion.identity);
        FirebaseMananger.LogEventPlayLevel(index);
    }
    public void InitCoin()
    {

    }
    public IEnumerator  InitListChair()
    {
        if (indexIcon < data.question[index].soluongchuoi.Length)
        {

            if (chainIconList != null)
            {
                foreach (Icon i in chainIconList)
                {
                    Destroy(i.gameObject);
                }
                chainIconList.Clear();
            }

            // pause = true;
            topIndChain = 0;
            a_Current = 0;
            //addOpaque.Clear();
            //check_Chain.Clear();
            yield return new WaitForSeconds(1f);
            Ui[2].gameObject.SetActive(true);
            for (int i = 0; i < data.question[index].soluongchuoi[indexIcon]; i++)
            {
                s_Chain = Instantiate(chainPrefab, c_tranform); //khoi tao chuoi bang prefab Chain                                                             
                chainIconList.Add(s_Chain);   //add chuoi vua khoi tao vao list chainIconList de sau de su dung
                                              // s_Chain.t_icon = addChain[i]; //dat kieu type hien thi 
                                              //  s_Chain.opaque = addOpaque[i]; // dat color hien thi
               
                if (index <= 10)
                {
                    s_Chain.opaque = data.question[i].colorr = true;
                    if (index < 5)
                    {
                        s_Chain.t_icon = addChain[Random.Range(0, addChain.Count - 1)];

                    }
                    else
                    {
                        s_Chain.t_icon = addChain[Random.Range(0, addChain.Count)];

                    }

                }
                if (index > 10)
                {
                    s_Chain.t_icon = addChain[Random.Range(0, addChain.Count)];
                   int a = Random.Range(a_Current, 2);
                    s_Chain.opaque = data.question[i].colorr = a == 1 ? true : false;
                    if (a == 0)
                    {
                        a_Current++;
                    }

                }
                //   addOpaque.Add(s_Chain.opaque);
            }
            indexIcon++;


        }
       
    }


    IEnumerable InitObjDeco()
    {
        //GameObject itemDeco;   
        //itemDeco = Instantiate(data.itemDeco[Random.Range(0,data.itemDeco.Count)], new Vector2(i + x, 3f), Quaternion.identity);
        //itemDeco.transform.SetParent(hidd_tranform.transform);
        yield return new WaitForSeconds(3f);
        ////  if(data.itemDeco[i].transform.position ) check dk de iteam deco ko nam len nhau
        //itemDeco = Instantiate(data.itemDeco[i], new Vector2(i + x, 3f), Quaternion.identity);
        
    }

    public void Click(type c_loaihinh)
    {
        if (chainIconList.Count != 0 && topIndChain < chainIconList.Count)
        {
            #region test type button
            //if (c_loaihinh == type.tron)
            //{
            //    Debug.Log("Day la hinh tron");
            //}
            //else if (c_loaihinh == type.tamgiac)
            //{
            //    Debug.Log("Day la hinh tam giac");
            //}
            //else if (c_loaihinh == type.vuong)
            //{
            //    Debug.Log("Day la hinh vuong");
            //}
            #endregion

            #region kiem tra xem type button co bang voi type cua chuoi ky tu hay ko */
            if (chainIconList[topIndChain].opaque)
            {

                if (c_loaihinh == /*addChain[topIndChain]*/chainIconList[topIndChain].t_icon)
                {
                    chainIconList[topIndChain].isTrue = true;
                    //   Debug.Log("Ban da chon dung");
                    check_Chain.Add(chainIconList[topIndChain].isTrue);
                }
                else
                {
                    chainIconList[topIndChain].isTrue = false;
                    //  Debug.Log("Ban da chon sai");
                    check_Chain.Add(chainIconList[topIndChain].isTrue);
                }
            }
            else
            {
                /* kiem tra xem type button co bang voi type cua chuoi ky tu hay ko */
                if (c_loaihinh ==/* addChain[topIndChain]*/chainIconList[topIndChain].t_icon)
                {
                    chainIconList[topIndChain].isTrue = false;
                    check_Chain.Add(chainIconList[topIndChain].isTrue);
                    //  Debug.Log("Ban da chon sai");
                }
                else
                {
                    chainIconList[topIndChain].isTrue = true;
                    //   Debug.Log("Ban da chon dung");
                    check_Chain.Add(chainIconList[topIndChain].isTrue);

                    switch (c_loaihinh)
                    {
                        case type.tron:
                            chainIconList[topIndChain].geometry.sprite = data.chain_Normal[0];
                            chainIconList[topIndChain].rect.sizeDelta = new Vector2(68, 68);
                            chainIconList[topIndChain].rect.DOLocalMoveY(0, 0.1f);
                            break;
                        case type.tamgiac:
                            chainIconList[topIndChain].geometry.sprite = data.chain_Normal[1];
                            chainIconList[topIndChain].rect.sizeDelta = new Vector2(66, 59);
                            //rect.LeanMoveLocalY(5, 0.1f);
                            chainIconList[topIndChain].rect.DOLocalMoveY(5, 0.1f);
                            break;
                        case type.vuong:
                            chainIconList[topIndChain].geometry.sprite = data.chain_Normal[2];
                            chainIconList[topIndChain].rect.sizeDelta = new Vector2(59, 58);
                            chainIconList[topIndChain].rect.DOLocalMoveY(0, 0.1f);
                            break;
                    }

                }
            }
            chainIconList[topIndChain].normal = false;

            topIndChain++;
            #endregion     
        }

    }

    public void SetUi()
    {
        if(this != null) { 
        if (startImage != null)
        {
            startImage.SetActive(true);
            ReddyImage.SetActive(false);
        }
        if (index == 0 || index == 10)
        {
            tutor.SetTutorial();

              
            Ui[5].gameObject.SetActive(false);
        }
     
        #region AnimationUI      
        
            Ui[0].DOAnchorPosX(-150f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f).OnComplete(() =>
        {
            Ui[3].DOAnchorPosX(95f, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Ui[0].gameObject.SetActive(false);
            });
        });
            if (!AdsMananger.Instance.bannerCheck)
            {
                s_distance.GetComponent<RectTransform>().DOAnchorPosY(-130f, 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
            }
            else
            {
                s_distance.GetComponent<RectTransform>().DOAnchorPosY(-130f - AdsMananger.Instance.GetBannerHeight(), 0.5f).SetEase(Ease.OutBack).SetDelay(0.5f);
            }
            Ui[1].gameObject.SetActive(false);

            Ui[2].gameObject.SetActive(false);
       
        }
        else
        {
            return;
        }


        #endregion
    }

    public void Finish()
    {
        AdsMananger.Instance.ShowInterstitial(()=>
        {
            FirebaseMananger.LogEventShowInter("Interstitial is show");
        });
        if (MovingPlayer.instance.win)
        {
            int claimCoin = saveCoin + Coin;
            PlayerPrefs.SetInt("Coin", claimCoin);
        }          
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
          Time.timeScale = 1;
    }

    public void TimeSpeed()
    {
        if (normalSpeed)
        {
            Time.timeScale = 2f;
            Ui[3].GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            Ui[3].GetChild(0).gameObject.SetActive(false);
        }
        normalSpeed = !normalSpeed;
    }

    public void ClaimCoin()
    {
        claimTxt.text ="+"+ Coin.ToString();
       // claimx2Txt.text ="+"+ ((Coin - 17569)*2).ToString();
    }

    private void OnDisable()
    {
        for (int i = 0; i < buttonSwitch.Length; i++)
        {
            int ind = i;
            buttonSwitch[i].onClick.RemoveListener(() => Click(loaihinh[ind]));
        }
    }

 
}
