using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;

public class Attacking : BaseState
{
    private FrogSM sm;
    private FrogData fd;

    private float timer;
    BaseBug oldBugInfo;
    BaseBug bugInfo;
    public Attacking(FrogSM stateMachine) : base("Attacking", stateMachine)
    {
        sm = (FrogSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        fd = sm.frogData;
        sm.AttackInRadius(fd.baseBugInfo, FrogBuff());
    }

    int FrogBuff()
    {
        if (fd.sunglassesBuff)
        {
            int percentageBuff = 0; 
            foreach (int number in fd.damageBuff)
            {
                if (number > percentageBuff)
                {
                    percentageBuff = number;
                }
            }
            int currentDamage = fd.GetDamage();
            //float currentFireRate = fd.GetFireRate();

            float damageIncrease = ((float)currentDamage * (float)percentageBuff) / 100f;
            int damageIncreaseRoundedToInt = Mathf.CeilToInt(damageIncrease);
            //float fireRateIncrease = (currentFireRate * frogData.GetFireRate()) / 100;

            //frog.frogData.SetDamage(currentDamage + damageIncreaseRoundedToInt);
            //frog.frogData.SetFireRate(currentFireRate + fireRateIncrease);

            return damageIncreaseRoundedToInt;

        }else 
        return 0;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
    }

    public override void Exit()
    {
        base.Exit();
    }


    private void AttackHighestHealth()
    {
        timer -= Time.deltaTime;
        //sm.Look(bugInfo.transform);
        if (timer <  0)
        {
            float largestHealth;
            if (fd.GetOverlappingColliders2D().Count > 0)
            {
                List<int> largestHealthList = new List<int>();
                foreach (Collider2D collider in fd.GetOverlappingColliders2D())
                {
                    BaseBug bugInfo = collider.transform.GetComponent<BaseBug>();
                    largestHealthList.Add(bugInfo.GetHealth());
                }
                largestHealth = largestHealthList.Max();

                foreach (Collider2D collider in fd.GetOverlappingColliders2D())
                {
                    BaseBug bugInfo = collider.transform.GetComponent<BaseBug>();
                    if (bugInfo.GetHealth() == largestHealth)
                    {
                        bugInfo.TakeDamage(fd.GetDamage());
                        timer = 1/fd.GetFireRate();
                        return;
                    }
                }
            } 
        }
    }

}