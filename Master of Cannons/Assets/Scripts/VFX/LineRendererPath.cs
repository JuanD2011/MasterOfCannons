using System.Collections.Generic;
using UnityEngine;

public static class LineRendererPath
{
    public static void RenderLines(LineRenderer _lineRenderer, Vector3[] targets)
    {
        try
        {
            _lineRenderer.positionCount = targets.Length;
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, targets[i]);
            }
        }
        catch { Debug.Log("There is no Line Renderer to handle"); }
    }

    public static void RenderBezierCurve(LineRenderer _lineRenderer, int _vertexCount, Vector3[] targetRotations, Vector3 originPoint)
    {
        var pointList = new List<Vector3>();
        Vector3 point1 = originPoint + new Vector3(1 * -(float)System.Math.Sin(targetRotations[0].z * Mathf.Deg2Rad), 1 * (float)System.Math.Cos(targetRotations[0].z * Mathf.Deg2Rad), 0);
        Vector3 point2 = originPoint + new Vector3(1 * -(float)System.Math.Sin((targetRotations[0].z + targetRotations[targetRotations.Length - 1].z) / 2 * Mathf.Deg2Rad), 1 * (float)System.Math.Cos((targetRotations[0].z + targetRotations[targetRotations.Length-1].z) / 2 * Mathf.Deg2Rad), 0);
        Vector3 point3 = originPoint + new Vector3(1 * -(float)System.Math.Sin(targetRotations[targetRotations.Length - 1].z * Mathf.Deg2Rad), 1 * (float)System.Math.Cos(targetRotations[targetRotations.Length - 1].z * Mathf.Deg2Rad), 0);
        pointList.Add(point1);
        pointList.Add(point2);
        pointList.Add(point3);
        _lineRenderer.positionCount = pointList.Count;
        _lineRenderer.SetPositions(pointList.ToArray());
    }
}
