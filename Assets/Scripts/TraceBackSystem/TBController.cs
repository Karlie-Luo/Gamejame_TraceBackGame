using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kurisu.TimeControl;

public class TBController : MonoBehaviour
{
    public enum TBState
    {
        Normal,
        Choose,
        Record,
        RecallFast,
        RecallSlow
    }
    public enum FlashState
    {
        Normal,
        Flash
    }
    [LabelText("回溯器列表"), SerializeField]
    private List<TBStore> stores = new List<TBStore>();
    private TBStore playerStore;
    [LabelText("当前状态"), ReadOnly, SerializeField]
    private TBState state;
    public TBState CurrentState
    {
        get { return state; }
    }
    private FlashState flashState;
    public FlashState CurrentFlashState
    {
        get { return flashState; }
    }
    /// <summary>
    /// 为所有回溯器预设容量,你可以测试内存占用后提高上限，因为动态扩容会1.5倍增加容量带来浪费,尽量不要在游戏时扩容
    /// </summary>
    [SerializeField, LabelText("记录上限"), DisableInPlayMode]
    private int capacity = 3000;
    public int Capacity
    {
        get { return capacity; }
    }
    [SerializeField, LabelText("当前记录数"), ProgressBar(0, "capacity"), ReadOnly]
    private int currentCount;
    /// <summary>
    /// 记录步长,因为使用Update记录会有更大误差，建议使用FixedUpdate记录数据，FixedDeltaTime默认为0.02f
    /// </summary>
    [SerializeField, LabelText("记录步长"), Range(0.01f, 0.2f), Tooltip("每多少秒记录一次"), DisableIf("state", TBState.Record)]
    private float recordStep = 0.02f;
    [SerializeField, LabelText("回溯步长"), Range(0.01f, 0.2f), Tooltip("每多少秒回溯一次"), DisableIf("state", TBState.RecallFast)]
    private float recallStep = 0.02f;
    /// <summary>
    /// 当前记录步长
    /// </summary>
    /// <value></value>
    public float RecordStep
    {
        get { return recordStep; }
    }
    private float timer;
    public event Action OnRecordStartEvent;
    /// <summary>
    /// 回溯开始事件
    /// </summary>
    public event Action OnRecallStartEvent;
    /// <summary>
    /// 回溯结束事件
    /// </summary>
    public event Action OnRecallEndEvent;
    /// <summary>
    /// 记录中事件
    /// </summary>
    public event Action OnRecordEvent;
    /// <summary>
    /// 回溯中事件（慢回溯）
    /// </summary>
    public event Action OnRecallSlowEvent;
    /// <summary>
    /// 回溯中事件（快回溯）
    /// </summary>
    public event Action OnRecallFastEvent;
    /// <summary>
    /// 记录数变更事件（适用于UI更新）
    /// </summary>
    public event Action<float> OnStepChangeEvent;
    /// <summary>
    /// 控制器状态变更事件（适用于多人游戏状态同步）
    /// </summary>
    public event Action<TBState> OnStateChangeEvent;

    [LabelText("固定更新"), SerializeField]
    private bool useFixedUpdate = true;
    /// <summary>
    /// 使用物理更新FixedUpdateMode
    /// </summary>
    /// <value></value>
    public bool UseFixedUpdate
    {
        get { return useFixedUpdate; }
    }
    public static TBController instance;
    public static TBController Instance
    {
        get { return instance; }
    }
    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (TBController)this;
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
    public void Add(TBStore store)
    {
        if (!stores.Contains(store))
        {
            stores.Add(store);
        }
    }
    public void Remove(TBStore store)
    {
        if (stores.Contains(store))
        {
            stores.Remove(store);
        }
    }
    public GameObject ChooseOne()
    {
        GameObject obj = GetTBObject();
        Debug.Log("get obj : " + obj);
        if(obj == null)
        {
            return null;
        }
        TBStore timeStore = obj.GetComponent<TBStore>();
        if (timeStore != null)
        {
            Debug.Log("get timestore");
            timeStore.LockTimeStore(false);
        }
        RecordAll();
        return obj;
    }
    private GameObject GetTBObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider)
        {
            //do something
            Debug.Log(hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }
        else
        {
            Debug.Log("Error!");
            return null;
        }
    }
    public void BackToNormal()
    {
        Debug.Log("Controller Back to normal");
        UpdateState(TBState.Normal);
    }
    public void NormalToChoose()
    {
        Debug.Log("Controller to normal");
        UpdateState(TBState.Choose);
    }
    /// <summary>
    /// 回溯器开始记录
    /// </summary>
    [Button("记录"), HideInEditorMode, EnableIf("state", TBState.Choose)]
    public void RecordAll()
    {
        timer = recordStep;
        UpdateState(TBState.Record);
        OnRecordStartEvent?.Invoke();
        foreach (var store in stores)
        {
            store.Record();
        }

    }
    /// <summary>
    /// 回溯器开始回溯
    /// </summary>
    [Button("快回溯"), HideInEditorMode, EnableIf("state", TBState.Record)]
    public void RecallAllFast()
    {
        timer = 0;
        UpdateState(TBState.RecallFast);
        OnRecallStartEvent?.Invoke();
        foreach (var store in stores)
        {
            store.RecallFast();
        }
        recallCount = currentCount;
    }
    [Button("慢回溯"), HideInEditorMode, EnableIf("state", TBState.Record)]
    public void RecallAllSlow()
    {
        timer = 0;
        UpdateState(TBState.RecallSlow);
        OnRecallStartEvent?.Invoke();
        foreach (var store in stores)
        {
            store.RecallSlow();
        }
        recallCount = currentCount;
    }
    /// <summary>
    /// 强制关闭所有回溯器
    /// </summary>
    /*public void ShutdownAll()
    {
        state = TBState.Normal;
        currentCount = 0;
        foreach (var store in stores)
        {
            store.ShutDown();
        }
    }*/
    private void Update()
    {
        if (!useFixedUpdate)
            UpdateStore(Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (useFixedUpdate)
            UpdateStore(Time.fixedDeltaTime);
    }
    int recallCount;
    void UpdateStore(float deltaTime)
    {
        switch (state)
        {
            case TBState.Normal:
                {
                    break;
                }
            case TBState.Choose:
                {
                    break;
                }
            case TBState.Record:
                {
                    if (currentCount >= capacity)//到达上限直接回溯
                    {
                        RecallAllSlow();
                        break;
                    }
                    timer += deltaTime;
                    if (timer >= recordStep)
                    {
                        timer = 0;
                        currentCount += 1;
                        OnRecordEvent?.Invoke();//调用记录时刻
                        OnStepChangeEvent?.Invoke((float)currentCount / Capacity);
                    }
                    break;
                }
            case TBState.RecallSlow:
                {
                    if (currentCount == 0)//回溯结束调用回溯结束事件
                    {
                        OnRecallEndEvent?.Invoke();
                        UpdateState(TBState.Normal);
                        break;
                    }
                    timer += Time.deltaTime;
                    if (timer >= recallStep)
                    {
                        timer = 0;
                        currentCount -= 1;
                        OnStepChangeEvent?.Invoke((float)currentCount / Capacity);
                        OnRecallSlowEvent?.Invoke();//未结束则调用回溯时刻
                    }
                    break;
                }
            case TBState.RecallFast:
                {
                    if (currentCount == 0)//回溯结束调用回溯结束事件
                    {
                        OnRecallEndEvent?.Invoke();
                        UpdateState(TBState.Normal);
                        break;
                    }
                    currentCount -= 2;
                    if(currentCount < 0)
                    {
                        currentCount = 0;
                    }
                    OnStepChangeEvent?.Invoke((float)currentCount / Capacity);
                    OnRecallFastEvent?.Invoke();//未结束则调用回溯时刻
                    break;
                }
        }
    }
    private void UpdateState(TBState newState)
    {
        state = newState;
        OnStateChangeEvent?.Invoke(state);
    }

    private void UpdateFlashState(FlashState newState)
    {
        flashState = newState;
    }

    public void FlashToNormal()
    {
        Debug.Log("Controller Back to normal");
        UpdateFlashState(FlashState.Normal);
    }
    public void NormalToFlash()
    {
        Debug.Log("Controller to flash");
        UpdateFlashState(FlashState.Flash);
    }

}
