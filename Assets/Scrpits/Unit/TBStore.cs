using Kurisu.TimeControl;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBStore : MonoBehaviour
{
    [SerializeField, LabelText("����������")]
    protected bool locked;
    /// <summary>
    /// ����ջ
    /// </summary>
    protected Stack<TransformStep> steps;

    protected virtual void Start()
    {
        if (TBController.IsInitialized)
        {
            TBController.Instance.Add(this);
        }
        steps = new Stack<TransformStep>(TBController.Instance.Capacity);

    }
    protected virtual void OnDestroy()
    {
        if (TBController.IsInitialized)
        {
            TBController.Instance.Remove(this);
            TBController.Instance.OnRecallEndEvent -= TimeStoreOver;
            TBController.Instance.OnRecallEvent -= RecallTick;
            TBController.Instance.OnRecordEvent -= RecordTick;

        }
        steps.Clear();
    }
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="state"></param>
    public void LockTimeStore(bool state)
    {
        this.locked = state;
    }
    /// <summary>
    /// ��ʼ��¼
    /// </summary>
    public virtual void Record()
    {

        steps.Clear();
        if (locked)
            return;
        TBController.Instance.OnRecordEvent += RecordTick;
    }
    /// <summary>
    /// ��ʼ����
    /// </summary>
    public virtual void Recall()
    {
        TBController.Instance.OnRecordEvent -= RecordTick;
        if (locked)
            return;
        TBController.Instance.OnRecallEvent += RecallTick;
        TBController.Instance.OnRecallEndEvent += TimeStoreOver;
    }
    /// <summary>
    /// ��ֹ���ݺͼ�¼
    /// </summary>
    public virtual void TimeStoreOver()
    {
        steps.Clear();
        TBController.Instance.OnRecallEndEvent -= TimeStoreOver;
        TBController.Instance.OnRecallEvent -= RecallTick;
        TBController.Instance.OnRecordEvent -= RecordTick;
    }
    /// <summary>
    /// ǿ�ƹرջ�����
    /// </summary>
    public virtual void ShutDown()
    {
        TimeStoreOver();
    }
    /// <summary>
    /// ��¼ʱ��
    /// </summary>
    public virtual void RecordTick()
    {

        TransformStep newStep = new TransformStep();
        newStep.position = transform.position;
        newStep.rotation = transform.rotation;
        steps.Push(newStep);

    }
    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual void RecallTick()
    {
        if (steps.Count == 0)
            return;
        var oldStep = steps.Pop();
        transform.position = oldStep.position;
        transform.rotation = oldStep.rotation;

    }
}
