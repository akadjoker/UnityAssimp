using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ViewSkeleton))]
public class ViewSkeletonEditor : Editor 
{

    public override void OnInspectorGUI()
    {
        ViewSkeleton myTarget = (ViewSkeleton)target;

        EditorGUILayout.BeginHorizontal("Box");
        myTarget.rootNode = (Transform)EditorGUILayout.ObjectField(myTarget.rootNode, typeof(Transform));

        if(myTarget != null)
        {
            if (GUILayout.Button("PopulateChildren"))
            {
                myTarget.PopulateChildren();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}


