using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB Settings", menuName = "Draggable Behaviour Settings")]
public class DraggableBehaviourSettings : ScriptableObject
{
    public float yDraggingValue = 1.5f;
    public float risingSpeed = 0.3f;
}
