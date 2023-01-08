using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 倒计时器
/// </summary>
public sealed class CounterDownTimer : IDisposable
{
    public bool IsAutoCycle { get; private set; }                   // 是否自动循环（小于等于0后重置）
    public bool IsStoped { get; private set; }                      // 是否是否暂停了
    public double CurrentTime { get { return UpdateCurrentTime(); } }// 当前时间
    public bool IsTimeUp { get { return CurrentTime <= 0; } }       // 是否时间到
    public double Duration { get; private set; }                     // 计时时间长度

    private float lastTime;                                         // 上一次更新的时间
    private ulong lastUpdateFrame;                                    // 上一次更新倒计时的帧数（避免一帧多次更新计时）
    private double currentTime;                                      // 当前计时器剩余时间
    /// <summary>
    /// 倒计时运行事件，参数为剩余时间
    /// </summary>
    public List<Action<double>> UpdateAction = new List<Action<double>>();
    /// <summary>
    /// 倒计时结束时运行事件
    /// </summary>
    public List<Action> EndActions = new List<Action>();
    private bool IsEnd = false;
    /// <summary>
    /// 计时器速率
    /// </summary>
    public double Speed { get; private set; }

    /// <summary>
    /// 构造倒计时器
    /// </summary>
    /// <param name="duration">起始时间</param>
    /// <param name="autocycle">是否自动循环</param>
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
    /// 更新计时器时间
    /// </summary>
    /// <returns>返回剩余时间</returns>
    public double UpdateCurrentTime()
    {
        if (IsStoped || lastUpdateFrame == CounterDownTimerMgr.Instance.FrameCount)         // 暂停了或已经这一帧更新过了，直接返回
            return currentTime;
        //结束
        if (Math.Abs(currentTime - 0) < 1e-07)                                       // 小于等于0直接返回，如果循环那就重置时间
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
        //倒计时
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
    /// 更新时间标记信息
    /// </summary>
    private void UpdateLastTimeInfo()
    {
        lastTime = Time.time;
        lastUpdateFrame = CounterDownTimerMgr.Instance.FrameCount;
    }

    /// <summary>
    /// 开始计时，取消暂停状态
    /// </summary>
    public void Start()
    {
        Reset(Duration, false);
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="isStoped">是否暂停</param>
    public void Reset(double duration, bool isStoped = false)
    {
        IsEnd = false;
        UpdateLastTimeInfo();
        Duration = Math.Max(0f, duration);
        currentTime = Duration;
        IsStoped = isStoped;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        UpdateCurrentTime();    // 暂停前先更新一遍
        IsStoped = true;
    }

    /// <summary>
    /// 继续（取消暂停）
    /// </summary>
    public void Continue()
    {
        UpdateLastTimeInfo();   // 继续前先更新当前时间信息
        IsStoped = false;
    }

    /// <summary>
    /// 终止，暂停且设置当前值为0
    /// </summary>
    public void End()
    {
        //Dispose();
        IsStoped = true;
        currentTime = 0f;
    }

    /// <summary>
    /// 获取倒计时完成率（0为没开始计时，1为计时结束）
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
    /// 设置当前计时器剩余时间
    /// </summary>
    /// <param name="time"></param>
    public void SetCurrentTime(double time)
    {
        this.currentTime = time;
    }
    /// <summary>
    /// 设置计时器更新速率，乘法
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

