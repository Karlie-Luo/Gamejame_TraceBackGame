using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherDialog : MonoBehaviour
{
    private List<string> StoryDialog_1 = new List<string>();
    private int storyCount_1 = 0;

    private List<string> StoryDialog_2 = new List<string>();
    private int storyCount_2 = 0;

    private List<string> AbilityDialog_1 = new List<string>();
    private int abilityCount_1 = 0;

    private List<string> AbilityDialog_2 = new List<string>();
    private int abilityCount_2 = 0;

    private List<string> AbilityDialog_3 = new List<string>();
    private int abilityCount_3 = 0;

    private Text text;

    public int type = 0;

    public void Start()
    {
        text = this.gameObject.transform.GetChild(0).GetComponentInChildren<Text>();

        InitDialog();

        StartDialog(type);
    }

    public void InitDialog()
    {
        StoryDialog_1.Add("贪玩的小狐狸不喜欢上课、作业和长大。");
        StoryDialog_1.Add("这一天，学校放寒假，小狐狸很兴奋。");
        StoryDialog_1.Add("小狐狸决定重温儿时的冒险，于是它朝着附近的森林出发了。");

        StoryDialog_2.Add("小狐狸朝着森林深处走去。它玩得很尽兴。");
        StoryDialog_2.Add("然而直到它意识到要回家的时候，它才发现已经找不到回家的路了。");
        StoryDialog_2.Add("利用回溯的能力，帮助小狐狸离开这静谧中暗藏危险的森林吧！");

        AbilityDialog_1.Add("恭喜你获得了慢回溯的能力！");
        AbilityDialog_1.Add("按下鼠标左键，进入时停的状态。再次按下鼠标左键，选择可以回溯的物体。");
        AbilityDialog_1.Add("选择完毕后，在恰当的时机按下左Ctrl键，可以回溯物体。");
        AbilityDialog_1.Add("试试看吧！");

        AbilityDialog_2.Add("恭喜你获得了快回溯的能力！");
        AbilityDialog_2.Add("快回溯能够将物体快速回溯，甚至能给物体一个与原来运动方向反向的速度。");
        AbilityDialog_2.Add("和慢回溯唯一不同的是，快回溯的回溯触发使用左Shift键。");
        AbilityDialog_2.Add("天下武功，唯快不破。试试用快回溯击败敌人吧！");

        AbilityDialog_3.Add("恭喜你获得了瞬闪的能力！");
        AbilityDialog_3.Add("每4秒可以使用一次瞬闪能力。瞬闪能够将你送回到五秒之前的位置。");
        AbilityDialog_3.Add("利用这个能力，可以逃离危险，或者巧妙地打败敌人。");
        AbilityDialog_3.Add("尝试按下鼠标右键，使用瞬闪从深坑中逃出吧！");
    }
    public void ChangeStoryDialog_1()
    {
        if (storyCount_1 >= StoryDialog_1.Count - 1)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        storyCount_1 ++;
        text.text = StoryDialog_1[storyCount_1];
    }

    public void ChangeStoryDialog_2()
    {
        if (storyCount_2 >= StoryDialog_2.Count - 1)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        storyCount_2 ++;
        text.text = StoryDialog_2[storyCount_2];
    }

    public void ChangeAbilityDialog_1()
    {
        if (abilityCount_1 >= AbilityDialog_1.Count - 1)
        {
            TBManager.instance.ActiveAbility(0);
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        abilityCount_1 ++;
        text.text = AbilityDialog_1[abilityCount_1];
    }

    public void ChangeAbilityDialog_2()
    {
        if (abilityCount_2 >= AbilityDialog_2.Count - 1)
        {
            TBManager.instance.ActiveAbility(1);
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        abilityCount_2 ++;
        text.text = AbilityDialog_2[abilityCount_2];
    }

    public void ChangeAbilityDialog_3()
    {
        if (abilityCount_3 >= AbilityDialog_3.Count - 1)
        {
            TBManager.instance.ActiveAbility(2);
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        abilityCount_3 ++;
        text.text = AbilityDialog_3[abilityCount_3];
    }

    public void StartDialog(int type)
    {
        text.text = type switch
        {
            0 => StoryDialog_1[0],
            1 => StoryDialog_2[0],
            2 => AbilityDialog_1[0],
            3 => AbilityDialog_2[0],
            4 => AbilityDialog_3[0],
            _ => "没啥可说的",
        };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

}
