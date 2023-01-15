using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kurisu.TimeControl;
using UnityEngine.EventSystems;

public class TBManager : MonoBehaviour
{
    public struct AbilityState
    {
        public bool fastRecall;
        public bool slowRecall;
        public bool flash;
        public AbilityState(bool value1,bool value2, bool value3)
        {
            fastRecall = value1;
            slowRecall = value2;
            flash = value3;
        }
    }

    private Sequence seq, coldTimer;
    private int counter = 0;
    private Queue transformQueue = new Queue();

    public GameObject flashLight;
    public GameObject recordObj;
    public AbilityState abilityState = new AbilityState(true, true, true);

    public static TBManager instance;
    public static TBManager Instance
    {
        get { return instance; }
    }
    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (TBManager)this;
            DontDestroyOnLoad(this);
        }      
    }
    public void Start()
    {
        //初始化计时器
        seq = DOTween.Sequence();
        InitSequence();
        if (seq.IsPlaying())
        {
            seq.Pause<Sequence>();
        }
        coldTimer = DOTween.Sequence();
        InitColdTimer();
        if (coldTimer.IsPlaying())
        {
            coldTimer.Pause<Sequence>();
        }
    }
    public static bool IsInitialized
    {
        get { return instance != null; }
    }
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && (abilityState.fastRecall == true || abilityState.slowRecall == true)
            && EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (TBController.Instance.CurrentState == TBController.TBState.Normal)
            {
                Debug.Log("normal to choose");
                TBController.Instance.NormalToChoose();
                Player.Instance.TimeStopChecks();
                seq.Restart();
            }
            else if (TBController.Instance.CurrentState == TBController.TBState.Choose)
            {
                GameObject obj = TBController.Instance.ChooseOne();
                if(obj == null)
                {
                    return;
                }
                recordObj.transform.position = obj.transform.position;
                recordObj.SetActive(true);
                if (seq.IsPlaying())
                {
                    seq.Pause<Sequence>();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl) && abilityState.slowRecall == true)
        {
            if (TBController.Instance.CurrentState == TBController.TBState.Record)
            {
                //Debug.Log("slow recall");
                recordObj.SetActive(false);
                TBController.Instance.RecallAllSlow();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && abilityState.fastRecall == true)
        {
            if (TBController.Instance.CurrentState == TBController.TBState.Record)
            {
                //Debug.Log("fast recall");
                recordObj.SetActive(false);
                TBController.Instance.RecallAllFast();
            }
        }
        else if(Input.GetMouseButtonDown(1) && abilityState.flash == true)
        {
           if(TBController.Instance.CurrentFlashState == TBController.FlashState.Normal)
           {
                Debug.Log("can flash");
                TBController.Instance.NormalToFlash();
                Player.Instance.transform.position = flashLight.transform.position;
                Player.Instance.transform.rotation = flashLight.transform.rotation;
                RestartFlashUpdate();
                coldTimer.Restart();
                //Player.Instance.Flash(step);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Tab))
        {
            Player.Instance.Rebirth();
            //Player.Instance.transform.position = flashLight.transform.position;
            //Player.Instance.transform.rotation = flashLight.transform.rotation;
        }
    }
    private void FixedUpdate()
    {
        transformQueue.Enqueue(Player.Instance.GetTransfomStep());
        if (counter <= 150)
        {
            counter++;
        }
        else
        {
            if(flashLight.activeSelf == false)
            {
                flashLight.SetActive(true);
            }
            UpdateFlash();
        }
    }

    private void InitSequence()
    {
        Debug.Log("init sequence");
        seq.AppendInterval(2);
        seq.AppendCallback(BackToNormal);
        seq.SetAutoKill(false);
        seq.SetUpdate(true);//设为true时可在Time.timeScale=0的情况下正常执行
    }

    private void InitColdTimer()
    {
        Debug.Log("Init coldTimer");
        coldTimer.AppendInterval(4);
        coldTimer.AppendCallback(BackToNormal_Flash);
        coldTimer.SetAutoKill(false);
        coldTimer.SetUpdate(true);
    }

    private void BackToNormal()
    {
        //Debug.Log("Manager back to normal");
        TBController.Instance.BackToNormal();
    }
    private void BackToNormal_Flash()
    {
        Debug.Log("Manager back to normal - flash");
        TBController.Instance.FlashToNormal();
    }
    
    public void UpdateFlash()
    {
        TransformStep step = (TransformStep)transformQueue.Dequeue();
        flashLight.transform.position = step.position;
        flashLight.transform.rotation = step.rotation;
    }

    public void RestartFlashUpdate()
    {
        Debug.Log("RestartFlashUpdate");
        if (flashLight.activeSelf == true)
        {
            flashLight.SetActive(false);
        }
        transformQueue.Clear();
        counter = 0;
    }

    public void ActiveAbility(int num)
    {
        switch (num)
        {
            case 0:
                abilityState.fastRecall = true;
                break;
            case 1:
                abilityState.slowRecall = true;
                break;
            case 2:
                abilityState.flash = true;
                break;
            default:
                Debug.Log("dont have the ability!");
                break;
        }
    }

    public bool GetActiveAbility(int num)
    {
        switch (num)
        {
            case 0:
                return abilityState.fastRecall;
            case 1:
                return abilityState.slowRecall;
            case 2:
                return abilityState.flash;
            default:
                Debug.Log("dont have the ability!");
                return false;
        }
    }

    public void PlayerRebirth()
    {
        TBController.Instance.BackToNormal();
        TBController.Instance.NormalToFlash();
        RestartFlashUpdate();
        coldTimer.Restart();
    }
}
