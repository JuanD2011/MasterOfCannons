﻿using UnityEngine;
using System.Collections.Generic;

public class MovingBehaviour : MonoBehaviour
{
    [SerializeField]
    protected bool startMoving = false;

    [SerializeField]
    protected float speed = 5f;

    [SerializeField]
    protected LeanTweenType tweenType = LeanTweenType.easeInOutSine;

    private List<Vector3> targets = new List<Vector3>();

    private int targetCounter = 0;

    protected System.Action repeatMethod = null;

    private void Start()
    {
        repeatMethod += MoveToTarget;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Target")) targets.Add(transform.GetChild(i).position);
        }

        if (startMoving) Move();

        MenuManager.OnPause += SetMovement;
    }

    private void SetMovement(bool _Value)
    {
        if (_Value)
        {
            LeanTween.pauseAll();
        }
        else
        {
            LeanTween.resumeAll();
        }
    }

    protected virtual void Move()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        LeanTween.move(gameObject, targets[targetCounter], 1f).setEase(tweenType).setOnComplete(repeatMethod).setSpeed(speed);
        targetCounter = (targetCounter + 1) % targets.Count;
    }
}