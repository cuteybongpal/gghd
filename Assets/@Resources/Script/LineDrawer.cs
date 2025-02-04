using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    LineRenderer _lineRenderer;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    public void SetPosCount(int count)
    {
        _lineRenderer.positionCount = count;
    }
    public void ResetLine()
    {
        _lineRenderer.positionCount = 0;
    }
    public void SetPosition(int index, Vector3? pos = null)
    {
        Vector3 pos2 = Vector3.zero;
        if (pos == null)
            pos2 = transform.position;
        else
            pos2 = pos.Value;

        _lineRenderer.SetPosition(index, pos2);
    }
}
