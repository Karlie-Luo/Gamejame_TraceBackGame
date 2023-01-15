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
        StoryDialog_1.Add("̰���С���겻ϲ���ϿΡ���ҵ�ͳ���");
        StoryDialog_1.Add("��һ�죬ѧУ�ź��٣�С������˷ܡ�");
        StoryDialog_1.Add("С����������¶�ʱ��ð�գ����������Ÿ�����ɭ�ֳ����ˡ�");

        StoryDialog_2.Add("С���곯��ɭ�����ȥ������úܾ��ˡ�");
        StoryDialog_2.Add("Ȼ��ֱ������ʶ��Ҫ�ؼҵ�ʱ�����ŷ����Ѿ��Ҳ����ؼҵ�·�ˡ�");
        StoryDialog_2.Add("���û��ݵ�����������С�����뿪�⾲���а���Σ�յ�ɭ�ְɣ�");

        AbilityDialog_1.Add("��ϲ�����������ݵ�������");
        AbilityDialog_1.Add("����������������ʱͣ��״̬���ٴΰ�����������ѡ����Ի��ݵ����塣");
        AbilityDialog_1.Add("ѡ����Ϻ���ǡ����ʱ��������Ctrl�������Ի������塣");
        AbilityDialog_1.Add("���Կ��ɣ�");

        AbilityDialog_2.Add("��ϲ�����˿���ݵ�������");
        AbilityDialog_2.Add("������ܹ���������ٻ��ݣ������ܸ�����һ����ԭ���˶���������ٶȡ�");
        AbilityDialog_2.Add("��������Ψһ��ͬ���ǣ�����ݵĻ��ݴ���ʹ����Shift����");
        AbilityDialog_2.Add("�����书��Ψ�첻�ơ������ÿ���ݻ��ܵ��˰ɣ�");

        AbilityDialog_3.Add("��ϲ������˲����������");
        AbilityDialog_3.Add("ÿ4�����ʹ��һ��˲��������˲���ܹ������ͻص�����֮ǰ��λ�á�");
        AbilityDialog_3.Add("���������������������Σ�գ���������ش�ܵ��ˡ�");
        AbilityDialog_3.Add("���԰�������Ҽ���ʹ��˲����������ӳ��ɣ�");
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
            _ => "ûɶ��˵��",
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
