using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class GetBBox : ScriptableObject
{
    private static GameObject go;
 
    [MenuItem ("GameObject/Get Bounding Box")]
 
    static void MenuGetBound()
    {
        Bounds bound = new Bounds();
        bool didOne = false;
        bool found = false;
 
        didOne = GetBoundWithChildren(Selection.activeTransform, ref bound, ref found);
        if (didOne)
        {
            EditorUtility.DisplayDialog("Object Bounds", FormatBounds(bound)+"\nCenter: "+bound.center.ToString()+"  Size:"+bound.size.ToString(), "OK", "");
            Debug.Log("Object "+Selection.activeTransform.name+" Bounds "+FormatBounds(bound));
        }
        else
        {
            EditorUtility.DisplayDialog("Nothing renderable found", "OK", "");
            Debug.Log("Object "+Selection.activeTransform.name+" Nothing renderable found");
        }			
    }
 
    static bool GetBoundWithChildren(Transform parent, ref Bounds pBound, ref bool initBound)
    {
        Bounds bound = new Bounds();
        bool didOne = false;
 
        // get 'this' bound
        if (parent.gameObject.GetComponent<Renderer>() != null)
        {
            bound = parent.gameObject.GetComponent<Renderer>().bounds;
            if (initBound)
            {
                pBound.Encapsulate  (bound.min);
                pBound.Encapsulate  (bound.max);
            }
            else
            {
                pBound.min = new Vector3(bound.min.x,bound.min.y,bound.min.z);
                pBound.max = new Vector3(bound.max.x,bound.max.y,bound.max.z);
                initBound = true;
            }
            didOne = true;
        }
        // union with bound(s) of any/all children
        foreach ( Transform child in parent )
        {
            if (GetBoundWithChildren(child,ref pBound, ref initBound))
            {
                didOne = true;
            }
        }
        return didOne;
    }
 
    static string FormatBounds(Bounds b)
    {
        string bs = "Min: ("+b.min.x+", "+b.min.y+", "+b.min.z+")  Max: ("+b.max.x+", "+b.max.y+", "+b.max.z+")";
        return bs;	
    }
}