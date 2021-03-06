﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class LineRendererPath
{
    public static void RenderLines(LineRenderer _lineRenderer, Vector3[] targets)
    {
        try
        {
            if (targets.Length == 2)
            {
                Vector3[] retarget = new Vector3[3];
                retarget[0] = targets[0];
                retarget[1] = (targets[0] + targets[1]) / 2;
                retarget[2] = targets[1];
                _lineRenderer.positionCount = retarget.Length;
                _lineRenderer.SetPositions(retarget.ToArray());
            }
            else
            {
                _lineRenderer.positionCount = targets.Length;
                _lineRenderer.SetPositions(targets);
            }
        }
        catch { Debug.Log("There is no Line Renderer to handle"); }
    }

    public static void RenderBezierCurve(LineRenderer _lineRenderer, int _vertexCount, Vector3[] targetRotations, Vector3 originPoint)
    {
        var pointList = new List<Vector3>();
        Vector3 point1 = originPoint + new Vector3(1.5f * -(float)System.Math.Sin(targetRotations[0].z * Mathf.Deg2Rad), 1.5f * (float)System.Math.Cos(targetRotations[0].z * Mathf.Deg2Rad), 0);
        Vector3 point2 = originPoint + new Vector3(1.5f * -(float)System.Math.Sin((targetRotations[0].z + targetRotations[targetRotations.Length - 1].z) / 2 * Mathf.Deg2Rad), 1.5f * (float)System.Math.Cos((targetRotations[0].z + targetRotations[targetRotations.Length-1].z) / 2 * Mathf.Deg2Rad), 0);
        Vector3 point3 = originPoint + new Vector3(1.5f * -(float)System.Math.Sin(targetRotations[targetRotations.Length - 1].z * Mathf.Deg2Rad), 1.5f * (float)System.Math.Cos(targetRotations[targetRotations.Length - 1].z * Mathf.Deg2Rad), 0);
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / _vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1, point2, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2, point3, ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierpoint);
        }
        _lineRenderer.positionCount = pointList.Count;
        _lineRenderer.SetPositions(pointList.ToArray());
    }
}
