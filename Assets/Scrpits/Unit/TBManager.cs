using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kurisu.TimeControl;

public class TBManager : MonoBehaviour
{
    [LabelText("��������"), SerializeField]
    private KeyCode key = KeyCode.F;
    public static TBManager instance;
    Sequence seq, coldTimer;
    TransformStep step = new TransformStep();
    int counter = 0;
    public static TBManager Instance
    {
        get { return instance; }
    }
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void Start()
    {
        //��ʼ����ʱ��
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
                seq.Restart();
            }
            else if (TBController.Instance.CurrentState == TBController.TBState.Choose)
            {
                //Э��
                //ʱͣ
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
                seq.Restart();
                TBController.Instance.NormalToFlash();
                //Player.Instance.Flash(step);
           }
        }
    }
    private void FixedUpdate()
    {
        if(counter <= 300)
        {
            counter++;
            //SetActive ������
        }
        else
        {
            //step = Player.Instance.GetTransfomStep();
        }
    }

    private void InitSequence()
    {
        Debug.Log("init sequence");
        seq.AppendInterval(2);
        seq.AppendCallback(BackToNormal);
        seq.SetAutoKill(false);
        seq.SetUpdate(true);//��Ϊtrueʱ����Time.timeScale=0�����������ִ��
    }

    private void InitColdTimer()
    {
        Debug.Log("Init coldTimer");
        coldTimer.AppendInterval(5);
        seq.AppendCallback(BackToNormal);
        seq.SetAutoKill(false);
        seq.SetUpdate(true);
    }

    private void BackToNormal()
    {
        Debug.Log("Manager back to normal");
        TBController.Instance.ChooseToNormal();
    }
    private void BackToNormal_Flash()
    {
        Debug.Log("Manager back to normal - flash");
        TBController.Instance.FlashToNormal();
    }
    
}
