using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kurisu.TimeControl;

public class TBController : MonoBehaviour
{
    public TimeStopSphere timeStopSphere;
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
    [LabelText("�������б�"), SerializeField]
    private List<TBStore> stores = new List<TBStore>();
    private TBStore playerStore;
    [LabelText("��ǰ״̬"), ReadOnly, SerializeField]
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
    /// Ϊ���л�����Ԥ������,����Բ����ڴ�ռ�ú�������ޣ���Ϊ��̬���ݻ�1.5���������������˷�,������Ҫ����Ϸʱ����
    /// </summary>
    [SerializeField, LabelText("��¼����"), DisableInPlayMode]
    private int capacity = 3000;
    public int Capacity
    {
        get { return capacity; }
    }
    [SerializeField, LabelText("��ǰ��¼��"), ProgressBar(0, "capacity"), ReadOnly]
    private int currentCount;
    /// <summary>
    /// ��¼����,��Ϊʹ��Update��¼���и���������ʹ��FixedUpdate��¼���ݣ�FixedDeltaTimeĬ��Ϊ0.02f
    /// </summary>
    [SerializeField, LabelText("��¼����"), Range(0.01f, 0.2f), Tooltip("ÿ�������¼һ��"), DisableIf("state", TBState.Record)]
    private float recordStep = 0.02f;
    [SerializeField, LabelText("���ݲ���"), Range(0.01f, 0.2f), Tooltip("ÿ���������һ��"), DisableIf("state", TBState.RecallFast)]
    private float recallStep = 0.02f;
    /// <summary>
    /// ��ǰ��¼����
    /// </summary>
    /// <value></value>
    public float RecordStep
    {
        get { return recordStep; }
    }
    private float timer;
    public event Action OnRecordStartEvent;
    /// <summary>
    /// ���ݿ�ʼ�¼�
    /// </summary>
    public event Action OnRecallStartEvent;
    /// <summary>
    /// ���ݽ����¼�
    /// </summary>
    public event Action OnRecallEndEvent;
    /// <summary>
    /// ��¼���¼�
    /// </summary>
    public event Action OnRecordEvent;
    /// <summary>
    /// �������¼��������ݣ�
    /// </summary>
    public event Action OnRecallSlowEvent;
    /// <summary>
    /// �������¼�������ݣ�
    /// </summary>
    public event Action OnRecallFastEvent;
    /// <summary>
    /// ��¼������¼���������UI���£�
    /// </summary>
    public event Action<float> OnStepChangeEvent;
    /// <summary>
    /// ������״̬����¼��������ڶ�����Ϸ״̬ͬ����
    /// </summary>
    public event Action<TBState> OnStateChangeEvent;

    [LabelText("�̶�����"), SerializeField]
    private bool useFixedUpdate = true;
    /// <summary>
    /// ʹ���������FixedUpdateMode
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
            timeStopSphere.isChoosen = true;
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
    /// ��������ʼ��¼
    /// </summary>
    [Button("��¼"), HideInEditorMode, EnableIf("state", TBState.Choose)]
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
    /// ��������ʼ����
    /// </summary>
    [Button("�����"), HideInEditorMode, EnableIf("state", TBState.Record)]
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
    [Button("������"), HideInEditorMode, EnableIf("state", TBState.Record)]
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
    /// ǿ�ƹر����л�����
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
                    if (currentCount >= capacity)//��������ֱ�ӻ���
                    {
                        RecallAllSlow();
                        break;
                    }
                    timer += deltaTime;
                    if (timer >= recordStep)
                    {
                        timer = 0;
                        currentCount += 1;
                        OnRecordEvent?.Invoke();//���ü�¼ʱ��
                        OnStepChangeEvent?.Invoke((float)currentCount / Capacity);
                    }
                    break;
                }
            case TBState.RecallSlow:
                {
                    if (currentCount == 0)//���ݽ������û��ݽ����¼�
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
                        OnRecallSlowEvent?.Invoke();//δ��������û���ʱ��
                    }
                    break;
                }
            case TBState.RecallFast:
                {
                    if (currentCount == 0)//���ݽ������û��ݽ����¼�
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
                    OnRecallFastEvent?.Invoke();//δ��������û���ʱ��
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
