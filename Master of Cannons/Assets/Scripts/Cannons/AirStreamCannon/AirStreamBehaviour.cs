﻿using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class AirStreamBehaviour : MonoBehaviour
{
    [SerializeField]
    private AirStreamType type = AirStreamType.None;

    [SerializeField]
    private float streamMaxForce = 30f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem blowParticles = null;
    [SerializeField] private ParticleSystem suckParticles = null;

    private float range = 0f, distance = 0f, force = 0f;
    private bool playerInStreamZone = false;
    private Rigidbody playerRigidbody = null;

    private Cannon cannon = null;

    private AirStreamSwitchBehaviour switcher = null;

    public AirStreamType Type { get => type; set => type = value; }

    private void Awake()
    {
        range = GetComponent<BoxCollider>().size.y;
        cannon = GetComponentInChildren<Cannon>();
        switcher = GetComponent<AirStreamSwitchBehaviour>();
    }

    private void Start()
    {
        cannon.OnCharacterInCannon += OnCharacterInCannon;
        if (switcher != null) switcher.OnSwich += OnSwich;
        if (type == AirStreamType.None) Debug.LogError("No airstream type defined, this cannon may not work properly", gameObject);
        SetParticlesActive(true);
    }


    private void OnCharacterInCannon(bool _value)
    {
        if(_value)
        {
            playerInStreamZone = false;
            playerRigidbody = null;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInStreamZone = true;
        playerRigidbody = other.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other)
    {
        playerInStreamZone = false;
        playerRigidbody = null;
    }

    private void FixedUpdate()
    {
        if (playerInStreamZone && playerRigidbody != null)
        {
            AirStreamForce(); 
        }
    }

    private void AirStreamForce()
    {
        Vector3 direction = Vector3.zero;

        switch (Type)
        {
            case AirStreamType.Suck:
                direction = -transform.up;
                break;
            case AirStreamType.Blow:
                direction = transform.up;
                break;
            case AirStreamType.None:
                break;
            default:
                break;
        }

        distance = (transform.position - playerRigidbody.transform.position).sqrMagnitude;
        force = distance / range * streamMaxForce;

        playerRigidbody.AddForce(direction * force, ForceMode.Force);
    }

    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void SetParticlesActive(bool _value)
    {
        if (_value)
        {
            if (type == AirStreamType.Blow)
            {
                suckParticles.Stop();
                blowParticles.Play();
            }
            else if (type == AirStreamType.Suck)
            {
                blowParticles.Stop();
                suckParticles.Play();
            } 
        }
        else
        {
            blowParticles.Stop();
            suckParticles.Stop();
        }
    }
    private void OnSwich()
    {
        if (type == AirStreamType.Blow) type = AirStreamType.Suck;
        else if (type == AirStreamType.Suck) type = AirStreamType.Blow;
        SetParticlesActive(true);
    }
}

public enum AirStreamType
{
    Suck,
    Blow,
    None
}
