using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LogicSO", menuName = "ScriptableObjects/Frogs/LogicSO", order = 1)]
public class LogicSO : ScriptableObject
{
    //This scriptable object is in charge for everything logic related to the frogSO

    [Header("LOGIC")]

    public string frogName;
    [Min(50)]
    public int cost;
    [Range(0, 5)]
    public int discipline;
    [Min(5)]
    public int damage;
    [Min(1)]
    public float range;
    [Min(0.5f)]
    public float attackSpeed;
    public LayerMask targetLayer;
    public bool isWaterFrog;

    [Header("Upgrade")]

    public Upgrade<int> upgradeDiscipline;
    public Upgrade<int> upgradeDamage;
    public Upgrade<float> upgradeRange;
    public Upgrade<float> upgradeAttackSpeed;

    [Serializable]
    public struct Upgrade<T>
    {
        [Min(0)]
        public T amount;
        [Min(0)]
        public T price;
    }

    [Serializable]
    public enum Target
    {
        First,
        Last,
        Strongest,
        Weakest
    }

    [Serializable]
    public enum AttackType
    {
        AOE,
        Projectile,
        Single,
        Other,
    }
}
