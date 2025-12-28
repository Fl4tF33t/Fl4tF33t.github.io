using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;

public class Attacking : BaseState
{
    private FrogSM sm; // Reference to the Frog State Machine
    private FrogData fd; // Reference to the Frog Data

    private float timer; // Timer for attacking
    private BaseBug oldBugInfo; // Previous bug info
    private BaseBug bugInfo; // Current bug info

    // Constructor
    public Attacking(FrogSM stateMachine) : base("Attacking", stateMachine)
    {
        sm = (FrogSM)this.stateMachine; // Casting the state machine to FrogSM
    }

    // Called when entering the state
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
            foreach (int number in fd.damageBuff) // Iterate through damage buffs
            {
                if (number > percentageBuff)
                {
                    percentageBuff = number; // Set percentage buff to the new value
                }
            }
            int currentDamage = fd.GetDamage();

            // Calculate damage increase based on percentage buff
            float damageIncrease = ((float)currentDamage * (float)percentageBuff) / 100f;
            int damageIncreaseRoundedToInt = Mathf.CeilToInt(damageIncrease); // Round up to nearest integer

            return damageIncreaseRoundedToInt;
        }
        else
        {
            return 0;
        }
    }

    // Called every frame for logic update
    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }

    // Called every frame for physics update
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        AttackHighestHealth(); // Attack bug with highest health
    }

    // Called when exiting the state
    public override void Exit()
    {
        base.Exit();
    }

    // Attack bug with highest health within range
    private void AttackHighestHealth()
    {
        timer -= Time.deltaTime; // Decrease timer

        // If timer is less than 0
        if (timer < 0)
        {
            float largestHealth; // Initialize variable for largest health

            // If there are overlapping colliders
            if (fd.GetOverlappingColliders2D().Count > 0)
            {
                List<int> largestHealthList = new List<int>();
                foreach (Collider2D collider in fd.GetOverlappingColliders2D()) // Iterate through colliders
                {
                    BaseBug bugInfo = collider.transform.GetComponent<BaseBug>(); // Get bug info
                    largestHealthList.Add(bugInfo.GetHealth()); // Add bug's health to list
                }
                largestHealth = largestHealthList.Max(); // Get maximum health from list

                foreach (Collider2D collider in fd.GetOverlappingColliders2D())
                {
                    BaseBug bugInfo = collider.transform.GetComponent<BaseBug>();
                    if (bugInfo.GetHealth() == largestHealth)
                    {
                        bugInfo.TakeDamage(fd.GetDamage()); // Attack the bug
                        timer = 1 / fd.GetFireRate(); // Reset timer
                        return; // Exit method
                    }
                }
            }
        }
    }
}