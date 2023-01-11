using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ʱ��
/// </summary>
public sealed class CounterDownTimer : IDisposable
{
    public bool IsAutoCycle { get; private set; }                   // �Ƿ��Զ�ѭ����С�ڵ���0�����ã�
    public bool IsStoped { get; private set; }                      // �Ƿ��Ƿ���ͣ��
    public double CurrentTime { get { return UpdateCurrentTime(); } }// ��ǰʱ��
    public bool IsTimeUp { get { return CurrentTime <= 0; } }       // �Ƿ�ʱ�䵽
    public double Duration { get; private set; }                     // ��ʱʱ�䳤��

    private float lastTime;                                         // ��һ�θ��µ�ʱ��
    private ulong lastUpdateFrame;                                    // ��һ�θ��µ���ʱ��֡��������һ֡��θ��¼�ʱ��
    private double currentTime;                                      // ��ǰ��ʱ��ʣ��ʱ��
    /// <summary>
    /// ����ʱ�����¼�������Ϊʣ��ʱ��
    /// </summary>
    public List<Action<double>> UpdateAction = new List<Action<double>>();
    /// <summary>
    /// ����ʱ����ʱ�����¼�
    /// </summary>
    public List<Action> EndActions = new List<Action>();
    private bool IsEnd = false;
    /// <summary>
    /// ��ʱ������
    /// </summary>
    public double Speed { get; private set; }

    /// <summary>
    /// ���쵹��ʱ��
    /// </summary>
    /// <param name="duration">��ʼʱ��</param>
    /// <param name="autocycle">�Ƿ��Զ�ѭ��</param>
    public CounterDownTimer(float duration, bool autocycle = false, bool autoStart = true, double speed = 1f)
    {
        IsStoped = true;
        Duration = Mathf.Max(0f, duration);
        IsAutoCycle = autocycle;
        Speed = speed;
        Reset(duration, !autoStart);
        CounterDownTimerMgr.Instance.AddTimer(this);
    }

    ~CounterDownTimer()
    {
        CounterDownTimerMgr.Instance.RemoveTimer(this);
    }


    /// <summary>
    /// ���¼�ʱ��ʱ��
    /// </summary>
    /// <returns>����ʣ��ʱ��</returns>
    public double UpdateCurrentTime()
    {
        if (IsStoped || lastUpdateFrame == CounterDownTimerMgr.Instance.FrameCount)         // ��ͣ�˻��Ѿ���һ֡���¹��ˣ�ֱ�ӷ���
            return currentTime;
        //����
        if (Math.Abs(currentTime - 0) < 1e-07)                                       // С�ڵ���0ֱ�ӷ��أ����ѭ���Ǿ�����ʱ��
        {
            if (IsEnd)
            {
                return currentTime;
            }
            IsEnd = true;
            if (IsAutoCycle)
                Reset(Duration, false);
            foreach (var action in EndActions)
            {
                action.Invoke();
            }
            return currentTime;
        }
        //����ʱ
        double countTime = Time.fixedDeltaTime * Speed;
        currentTime -= countTime;
        currentTime = Math.Max(currentTime, 0);
        if (Math.Abs(currentTime - 0) < 1e-07)
        {
            currentTime = 0;
        }
        foreach (var action in UpdateAction)
        {
            action?.Invoke(countTime);
        }
        UpdateLastTimeInfo();
        return currentTime;
    }

    /// <summary>
    /// ����ʱ������Ϣ
    /// </summary>
    private void UpdateLastTimeInfo()
    {
        lastTime = Time.time;
        lastUpdateFrame = CounterDownTimerMgr.Instance.FrameCount;
    }

    /// <summary>
    /// ��ʼ��ʱ��ȡ����ͣ״̬
    /// </summary>
    public void Start()
    {
        Reset(Duration, false);
    }

    /// <summary>
    /// ���ü�ʱ��
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    /// <param name="isStoped">�Ƿ���ͣ</param>
    public void Reset(double duration, bool isStoped = false)
    {
        IsEnd = false;
        UpdateLastTimeInfo();
        Duration = Math.Max(0f, duration);
        currentTime = Duration;
        IsStoped = isStoped;
    }

    /// <summary>
    /// ��ͣ
    /// </summary>
    public void Pause()
    {
        UpdateCurrentTime();    // ��ͣǰ�ȸ���һ��
        IsStoped = true;
    }

    /// <summary>
    /// ������ȡ����ͣ��
    /// </summary>
    public void Continue()
    {
        UpdateLastTimeInfo();   // ����ǰ�ȸ��µ�ǰʱ����Ϣ
        IsStoped = false;
    }

    /// <summary>
    /// ��ֹ����ͣ�����õ�ǰֵΪ0
    /// </summary>
    public void End()
    {
        //Dispose();
        IsStoped = true;
        currentTime = 0f;
    }

    /// <summary>
    /// ��ȡ����ʱ����ʣ�0Ϊû��ʼ��ʱ��1Ϊ��ʱ������
    /// </summary>
    /// <returns></returns>
    public double GetPercent()
    {
        UpdateCurrentTime();
        if (currentTime <= 0 || Duration <= 0)
            return 1f;
        return 1f - currentTime / Duration;
    }
    /// <summary>
    /// ���õ�ǰ��ʱ��ʣ��ʱ��
    /// </summary>
    /// <param name="time"></param>
    public void SetCurrentTime(double time)
    {
        this.currentTime = time;
    }
    /// <summary>
    /// ���ü�ʱ���������ʣ��˷�
    /// </summary>
    /// <param name="speed"></param>
    public void SetUpdateSpeed(float speed)
    {
        this.Speed *= speed;
    }
    public void ResetUpdateSpeed(float speed)
    {
        this.Speed = speed;
    }
    /// <summary>
    /// countDownTimer.Dispose();
    /// countDownTimer = null;
    /// System.GC.Collect();
    /// </summary>
    public void Dispose()
    {
        if (CounterDownTimerMgr.Instance.counterDownTimers.Contains(this))
        {
            CounterDownTimerMgr.Instance.RemoveTimer(this);
        }
    }

    public void AddToMgr()
    {
        if (!CounterDownTimerMgr.Instance.counterDownTimers.Contains(this))
        {
            CounterDownTimerMgr.Instance.AddTimer(this);
        }
    }

}

