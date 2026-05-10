using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOrder", menuName = "VitalRun/Order")]
public class OrderData : ScriptableObject
{
    public string destination;
    public enum BoxColor { Gray, Green, Yellow, Blue }
    public BoxColor boxColor;
    public List<ItemData> requiredItems = new List<ItemData>();
    public int basePoints = 100;
}