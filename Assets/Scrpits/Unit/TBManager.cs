using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBManager : MonoBehaviour
{
    [LabelText("触发按键"), SerializeField]
    private KeyCode key = KeyCode.F;
    public static TBManager instance;
    Sequence seq;
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
        seq = DOTween.Sequence();
        InitSequence();
        if (seq.IsPlaying())
        {
            seq.Pause<Sequence>();
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
        if(Input.GetMouseButtonDown(0))
        {
            if(TBController.Instance.CurrentState == TBController.TBState.Normal)
            {
                Debug.Log("normal to choose");
                TBController.Instance.NormalToChoose();
                seq.Restart();
            }
            else if(TBController.Instance.CurrentState == TBController.TBState.Choose)
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
        else if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (TBController.Instance.CurrentState == TBController.TBState.Record)
            {
                Debug.Log("fast recall");
                TBController.Instance.ChangeRecallSpeed(true);
                TBController.Instance.RecallAll();
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (TBController.Instance.CurrentState == TBController.TBState.Record)
            {
                Debug.Log("slow recall");
                TBController.Instance.ChangeRecallSpeed(false);
                TBController.Instance.RecallAll();
            }
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

    private void BackToNormal()
    {
        Debug.Log("Manager back to normal");
        TBController.Instance.ChooseToNormal();
    }

}
