using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStone : MonoBehaviour
{
    public int num = 0;
    
    void Start()
    {
        if(!TBManager.Instance.GetActiveAbility(num))
        {
            this.gameObject.SetActive(false);
        }
    }
}
