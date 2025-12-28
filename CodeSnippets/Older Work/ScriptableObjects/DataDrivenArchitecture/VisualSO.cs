using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualSO", menuName = "ScriptableObjects/Frogs/VisualSO", order = 2)]
public class VisualSO : ScriptableObject
{
    //This scriptable object is in charge for everything visual related to the frogSO

    [Header("Visual")]
    public UI userInterface;
    //public AnimatorController animatorController;

    [Serializable]
    public struct UI
    {
        public Sprite UIShopSprite;
        [TextArea(3, 4)]
        public string UIShopTextInfo;

        public Sprite[] UIUpgradeButtons; 
    }

    public float opacityChange;

    //color changes during interactions
    public Color damageColor;
    public Color selectionColor;
    public Color healColor;
}
