using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenBoss : Boss
{
    [Range(0,1)]
    [SerializeField] float ravensPercentage = 0.5f;

    [Range(1, 2)]
    [SerializeField] float xRandomness = 1;
    [SerializeField] float timeToSpawnRavens = 0;
    [SerializeField] float timeToSpawnMagic = 0;
    float xOffset = 10;

    [SerializeField] GameObject ravensContainer = null;
    [SerializeField] GameObject magicRavenObj = null;
    MagicRaven magicRaven = null;
    List<Raven> ravens = new List<Raven>();

    private bool isAttacking = false;

    Character character = null;
    Cannon[] cannons = null;

    [Range(1, 10)]
    [SerializeField] float bossSpeed = 3f;

    public static event Delegates.Func<IEnumerator> OnBossAttack;
    float ySmoothMovement = 1.5f;

    Collider mCollider;

    private void Start()
    {
        isAttacking = false;
        magicRaven = magicRavenObj.AddComponent<MagicRaven>();
        cannons = FindObjectsOfType<Cannon>();
        character = FindObjectOfType<Character>();
        OnBossAttack += BossAttack;
        Raven[] ravenAux = ravensContainer.GetComponentsInChildren<Raven>();
        for (int i = 1; i < ravenAux.Length; i++)
            ravens.Add(ravenAux[i]);

        mCollider = GetComponent<Collider>();
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer()
    {
        float t = 100;
        float t1 = 0;
        while(!isAttacking)
        {
            t += Time.deltaTime;
            t1 += Time.deltaTime;

            if(t >= timeToSpawnRavens)
            {
                RandomizeRavens();
                t = 0;
            }

            if(t1 >= timeToSpawnMagic)
            {
                SpawnMagicRaven();
                t1 = 0;
            }

            yield return null;
        }
    }


    IEnumerator BossAttack()
    {        
        float t = 0;
        isAttacking = true;
        Transform lastCannon = character.transform;

        while (transform.position.z >= lastCannon.position.z + 0.2f)
        {
            t += Time.deltaTime;
            bool characterHasParent = character.transform.parent;

            if (characterHasParent && t <= 1.5f)
                lastCannon = character.transform.parent;

            transform.position = Vector3.MoveTowards(transform.position, lastCannon.position, Time.deltaTime * bossSpeed);
            yield return null;
        }
        
        isAttacking = false;
        StartCoroutine(SpawnTimer());
    }

    private void Update()
    {
        if (!isAttacking)
        {
            Vector3 pos = new Vector3(transform.position.x, character.transform.position.y, 10);
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * ySmoothMovement);
        }
    }

    private void RandomizeRavens()
    {
        for (int i = 0; i < cannons.Length; i++)
        {            
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber >= ravensPercentage) continue;
            if (!ravens[i].isVisible)
                SpawnRaven(ravens[i], cannons[i].transform.position);
        }
    }
  
    private void SpawnRaven(Raven raven, Vector3 pos)
    {
        float randomNumber = Random.Range(0, 2);
        float direction = randomNumber == 0 ? 1 : -1;        
        float randomPos = Random.Range(xOffset, xOffset * xRandomness);

        randomNumber = Random.Range(2f, 3.5f);
        float randomYPos = Random.Range(1.5f, 3f);
        raven.rigidbody.velocity = Vector3.zero;
        raven.transform.position = new Vector3(pos.x - (randomPos * direction), pos.y + randomYPos, pos.z);
        raven.rigidbody.AddForce(Vector3.right * randomNumber * direction, ForceMode.Impulse);
    }

    private void SpawnMagicRaven()
    {
        if (!magicRaven.isVisible)
        {
            int cannonLayerMask = 1 << 10;
            float sphereRadius = 6;
            Vector3 screenCenter = new Vector3(Screen.width / 2, (Screen.height / 2) + 5, 0);
            Vector3 screenCenter2World = Camera.main.ScreenToWorldPoint(screenCenter);
            screenCenter2World.z = character.transform.position.z;
            Collider[] cannonsColliders = Physics.OverlapSphere(screenCenter2World, sphereRadius, cannonLayerMask);
            if (cannonsColliders.Length > 0)
            {
                int randomNumber = Random.Range(0, cannonsColliders.Length);
                Vector3 cannonPos = cannonsColliders[randomNumber].transform.position;
                SpawnRaven(magicRaven, cannonPos);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Character character = other.GetComponent<Character>();
            FindObjectOfType<Cinemachine.CinemachineVirtualCamera>().Follow = null;
            character.SetKinematic(false);            
            other.GetComponent<Character>().Rigidbody.AddForce(Vector3.back * 10f, ForceMode.Impulse);
            StartCoroutine(WaitForDepth());
        }
        else if (other.gameObject.layer == 10)
        {
            StartCoroutine(WaitForDepth());
            BossHit();
        }
    }

    IEnumerator WaitForDepth()
    {
        mCollider.enabled = false;
        yield return new WaitWhile(() => transform.position.z <= 3);
        mCollider.enabled = true;      
    }


    protected override void SetDifficulty() { }

    public class MagicRaven : Raven
    {
        RavenBoss ravenBoss = null;

        protected override void Awake()
        {
            base.Awake();
            ravenBoss = FindObjectOfType<RavenBoss>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Character"))
            {
                ravenBoss.StartCoroutine(OnBossAttack?.Invoke());
                transform.position = Vector3.right * 200;
            }
        }
    }
}
