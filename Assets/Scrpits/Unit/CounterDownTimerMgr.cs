using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CounterDownTimerMgr : MonoBehaviour
{
    public static CounterDownTimerMgr Instance;
    /// <summary>
    /// 正常计时器
    /// </summary>
    public List<CounterDownTimer> counterDownTimers = new List<CounterDownTimer>();
    /// <summary>
    /// 销毁列表
    /// </summary>
    public List<CounterDownTimer> destroyCounterDownTimers = new List<CounterDownTimer>();
    /// <summary>
    /// 更新计数
    /// </summary>
    protected ulong frameCount = 0;
    public ulong FrameCount
    {
        get
        {
            return frameCount;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void FixedUpdate()
    {
        ++frameCount;
        if (frameCount >= ulong.MaxValue)
        {
            frameCount = 0;
        }
        for (int i = counterDownTimers.Count - 1; i >= 0; i--)
        {
            counterDownTimers[i].UpdateCurrentTime();
        }
    }

    private void LateUpdate()
    {
        for (int i = destroyCounterDownTimers.Count - 1; i >= 0; --i)
        {
            CounterDownTimer item = destroyCounterDownTimers[i];
            counterDownTimers.Remove(item);
            item = null;
        }
        destroyCounterDownTimers.Clear();
    }

    public void AddTimer(CounterDownTimer countDownTimer)
    {
        counterDownTimers.Add(countDownTimer);
    }

    public bool RemoveTimer(CounterDownTimer countDownTimer)
    {
        if (counterDownTimers.Contains(countDownTimer))
        {
            destroyCounterDownTimers.Add(countDownTimer);
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
