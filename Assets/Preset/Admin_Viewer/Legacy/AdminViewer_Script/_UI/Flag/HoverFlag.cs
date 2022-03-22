using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverFlag : MonoBehaviour
{
    [SerializeField] string name;

    public string FlagName
    {
        get => name;
    }
}
