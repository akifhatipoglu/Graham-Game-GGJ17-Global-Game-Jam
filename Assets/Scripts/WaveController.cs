using UnityEngine;
using System.Collections;

public class WaveController : MonoBehaviour
{
    public int SpreadDegree { get; set; }
    public int Frequency { get; set; }

    // Use this for initialization
    void Start()
    {
        DrawDefaultRay();
    }

    // Update is called once per frame
    void Update()
    {
        DrawDefaultRay();
    }

    private void DrawDefaultRay()
    {
        Debug.DrawRay(this.transform.position, new Vector3(5f, -5f, 0f), Color.red, 1000.0f);
    }
}
