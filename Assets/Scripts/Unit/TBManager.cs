using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kurisu.TimeControl;

public class TBManager : MonoBehaviour
{
    [LabelText("触发按键"), SerializeField]
    private KeyCode key = KeyCode.F;
    public static TBManager instance;
    private Sequence seq, coldTimer;
    private int counter = 0;
    public GameObject flashLight;
    public Queue transformQueue = new Queue();
    public static TBManager Instance
    {
        get { return instance; }
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
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
        if (Input.GetMouseButtonDown(0))
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
                //协程
                //时停
                if (seq.IsPlaying())
                {
                    seq.Pause<Sequence>();
                }
                TBController.Instance.ChooseOne();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (TBController.Instance.CurrentState == TBController.TBState.Record)
            {
                //Debug.Log("slow recall");
                TBController.Instance.RecallAllSlow();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (TBController.Instance.CurrentState == TBController.TBState.Record)
            {
                //Debug.Log("fast recall");
                TBController.Instance.RecallAllFast();
            }
        }
        else if(Input.GetMouseButtonDown(1))
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
        coldTimer.AppendInterval(5);
        coldTimer.AppendCallback(BackToNormal_Flash);
        coldTimer.SetAutoKill(false);
        coldTimer.SetUpdate(true);
    }

    private void BackToNormal()
    {
        //Debug.Log("Manager back to normal");
        TBController.Instance.ChooseToNormal();
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
}
