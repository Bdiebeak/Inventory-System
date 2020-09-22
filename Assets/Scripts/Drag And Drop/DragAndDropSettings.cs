using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB Settings", menuName = "Drag And Drop/Create New Settings")]
public class DragAndDropSettings : ScriptableObject
{
    public float yDraggingHeight = 1.5f;
    public float risingSpeed = 0.3f;
}
