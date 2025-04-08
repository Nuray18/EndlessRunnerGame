using UnityEngine;

public class FloatMotion : MonoBehaviour
{
    public float floatStrength = 0.5f;
    public float floatSpeed = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
