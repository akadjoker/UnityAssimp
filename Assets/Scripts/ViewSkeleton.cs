using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewSkeleton : MonoBehaviour 
{

    public Transform rootNode;
    public Transform[] childNodes;
    public float size = 1f;


    void OnDrawGizmosSelected()
    {
        if (rootNode != null)
        {
             //Gizmos.DrawCube(rootNode.position, new Vector3(.1f, .1f, .1f));
            foreach (Transform child in childNodes)
            {
                if (child == rootNode)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(child.position, new Vector3(1.5f, 1.5f, 1.5f));
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(child.position, child.parent.position);
                    Gizmos.DrawCube(child.position, new Vector3(size, size, size));
                }
            }

        }
    }

    public void PopulateChildren()
    {
        childNodes = rootNode.GetComponentsInChildren<Transform>();
    }
}
