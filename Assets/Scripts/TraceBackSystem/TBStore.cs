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
            TBController.Instance.OnRecallEndEvent -= TimeStoreFastOver;
            TBController.Instance.OnRecallEndEvent -= TimeStoreSlowOver;
            TBController.Instance.OnRecallFastEvent -= RecallTickFast;
            TBController.Instance.OnRecallSlowEvent -= RecallTickSlow;
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
    public virtual void RecallFast()
    {
        TBController.Instance.OnRecordEvent -= RecordTick;
        if (locked)
            return;
        TBController.Instance.OnRecallFastEvent += RecallTickFast;
        TBController.Instance.OnRecallEndEvent += TimeStoreFastOver;
    }
    public virtual void RecallSlow()
    {
        TBController.Instance.OnRecordEvent -= RecordTick;
        if (locked)
            return;
        TBController.Instance.OnRecallSlowEvent += RecallTickSlow;
        TBController.Instance.OnRecallEndEvent += TimeStoreSlowOver;
    }

    /// <summary>
    /// ��ֹ���ݺͼ�¼
    /// </summary>
    public virtual void TimeStoreSlowOver()
    {
        steps.Clear();
        TBController.Instance.OnRecallEndEvent -= TimeStoreSlowOver;
        TBController.Instance.OnRecallSlowEvent -= RecallTickSlow;
        TBController.Instance.OnRecordEvent -= RecordTick;
    }
    public virtual void TimeStoreFastOver()
    {
        steps.Clear();
        this.gameObject.GetComponent<Rigidbody2D>().velocity *= -2;
        TBController.Instance.OnRecallEndEvent -= TimeStoreFastOver;
        TBController.Instance.OnRecallFastEvent -= RecallTickFast;
        TBController.Instance.OnRecallSlowEvent -= RecallTickSlow;
        TBController.Instance.OnRecordEvent -= RecordTick;
    }

    /// <summary>
    /// ǿ�ƹرջ�����
    /// </summary>
    /*public virtual void ShutDown()
    {
        TimeStoreSlowOver();
    }*/
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
    public virtual void RecallTickSlow()
    {
        if (steps.Count == 0)
            return;
        var oldStep = steps.Pop();
        transform.position = oldStep.position;
        transform.rotation = oldStep.rotation;

    }
    public virtual void RecallTickFast()
    {
        if (steps.Count == 0)
            return;
        TransformStep oldStep = new TransformStep();
        for (int i = 0; i < 2; i ++)
        {
            if(steps.Count == 0)
            {
                transform.position = oldStep.position;
                transform.rotation = oldStep.rotation;
                return;
            }
            oldStep = steps.Pop();
        }
        transform.position = oldStep.position;
        transform.rotation = oldStep.rotation;
    }

}
