using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceDisplay : MonoBehaviour
{
    public DistanceTracker tracker;
    public TMP_Text distanceText;

    private void Update()
    {
        float dist = tracker.GetDistance();
        distanceText.text = "Distance: " + Mathf.FloorToInt(dist).ToString() + "m";
    }
}
