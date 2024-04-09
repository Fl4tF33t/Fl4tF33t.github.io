using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "FrogSO", menuName = "ScriptableObjects/Frogs/FrogSO", order = 0)]
public class FrogSO : ScriptableObject
{
    [Header("Static Values")]

    public GameObject prefab;

    [Header("LOGIC & UPGRADE")]
    public LogicSO logicSO;

    [Header("VISUAL & AUDIO")]
    public VisualSO visualSO;
    public AudioSO audioSO;

    public void InitGameObject(FrogBrain.Frog frog)
    {
        //set all of the relevant data for each of the frogs to have its current damage, range, etc.
        //Call this in the beginning of the main controller that uses the frog SO to initialise any of the needed data
        //additionally this can also be done in the class controller itself.
        frog.frogName = logicSO.frogName;
        frog.discipline = logicSO.discipline;
        frog.damage = logicSO.damage;
        frog.range = logicSO.range;
        frog.attackSpeed = logicSO.attackSpeed;

    }

}
