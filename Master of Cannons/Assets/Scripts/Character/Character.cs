﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    protected float specialProgress = 0f;
    protected float specialTime = 4f;
    protected bool canActivateSpecial = false;
    protected bool hasSpecial = false;

    public Rigidbody Rigidbody { get; private set; }
    public Vector3 velocityUpdated { get; private set; }
    public bool HasSpecial { get => hasSpecial; }

    public Collider Collider { get; private set; }

    public static event Delegates.Action<float> OnChargeSpecial;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        hasSpecial = false;
    }

    protected virtual void Start()
    {
        //PlayerInputHandler.OnSpecialFunc += OnSpecial;
        MenuGameManager.OnPause += Freeze;
        Referee.OnGameOver += Freeze;
    }

    private void Update()
    {
        velocityUpdated = Rigidbody.velocity;

        //if (Rigidbody.velocity.y < 0)
        //{
        //    Rigidbody.velocity += Physics.gravity * Time.deltaTime;
        //}
    }

    public void CannonEnterReset(Transform _reference)
    {
        transform.SetParent(_reference.transform);
        //Rigidbody.velocity = Vector3.zero;
        //transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
        SetKinematic(true);
    }

    public void SetKinematic(bool _value) => Rigidbody.isKinematic = _value;

    private void Freeze(LevelStatus _levelStatus)
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void Freeze(bool _Value)
    {
        if (_Value)
        {
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    /// <summary>
    /// Sets the character interactibility with other objects to value
    /// </summary>
    /// <param name="_value"></param>
    public void SetFunctional(bool _value)
    {
        if (_value)
        {
            Collider.enabled = true;
            SetKinematic(false);
        }
        else
        {
            Collider.enabled = false;
            SetKinematic(true);
        }
    }

    #region Special
    protected virtual IEnumerator OnSpecial()
    {
        canActivateSpecial = false;
        hasSpecial = true;
        for (float t = 0; t <= specialTime; t += Time.unscaledDeltaTime)
        {
            if (!hasSpecial) {
                OnDisableSpecial();
                yield break;
            }

            OnChargeSpecial.Invoke(1 - (t / specialTime));
            yield return null;
        }

        OnDisableSpecial();
    }

    public void UpdateSpecialProgress(float timePercentageInCannon)
    {
        if (canActivateSpecial || hasSpecial) return;
        specialProgress += (1 - timePercentageInCannon) * 0.3f;
        OnChargeSpecial.Invoke(specialProgress);

        if (specialProgress > 0.95f)
        {
            //PlayerInputHandler.canActivateSpecialHandler.Invoke();
            canActivateSpecial = true;
        }
    }

    protected virtual void OnDisableSpecial()
    {        
        hasSpecial = false;
        specialProgress = 0;
        OnChargeSpecial(specialProgress);
    }
    #endregion
}
