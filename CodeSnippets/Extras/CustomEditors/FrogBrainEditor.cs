using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FrogBrain))]
public class FrogBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FrogBrain script = (FrogBrain)target;

        // Draw default inspector
        DrawDefaultInspector();

        // Show/hide variables based on enum value
        if (script.attackType == LogicSO.AttackType.Projectile)
        {
            script.projectilePos = EditorGUILayout.ObjectField("Projectile Position", script.projectilePos, typeof(Transform), true) as Transform;
            script.projectile = EditorGUILayout.ObjectField("Projectile", script.projectile, typeof(GameObject), true) as GameObject;
        }
    }
}