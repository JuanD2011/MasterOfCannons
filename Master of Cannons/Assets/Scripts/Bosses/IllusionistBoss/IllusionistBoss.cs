using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionistBoss : Boss
{

    [SerializeField] float movementSpeed = 0;
    [SerializeField] Transform cannonsContainer = null;
    [SerializeField] int illusionCannonsNumber = 4;

    Transform[] cannons;
    [SerializeField] Transform character = null;
    protected override void Awake()
    {
        base.Awake();
        int arrayLength = cannonsContainer.childCount;
        cannons = new Transform[arrayLength];
        for(int i = 0; i < arrayLength; i++)
            cannons[i] = cannonsContainer.GetChild(i);

        StartCoroutine(SetIllusionCannons());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, character.position, Time.deltaTime * movementSpeed);
    }



    IEnumerator SetIllusionCannons()
    {
        Transform[] randomCannons = Randoms.ChooseRandomElements(cannons, illusionCannonsNumber);

        for(int i = 0; i < cannons.Length; i++)
        {
            bool illusionCannon = cannons[i].GetComponent<IllusionCannon>();
            if (illusionCannon)
                CannonSet(cannons[i], false);
        }

        yield return new WaitForSeconds(2f);

        for(int i = 0; i < randomCannons.Length; i++)
        {
            if (randomCannons[i].Find("Reference").childCount == 0)
                CannonSet(randomCannons[i], true);
        }

        StartCoroutine(SetIllusionCannons());

    }
    
    IEnumerator AttackAnticipation(IEnumerator coroutine)
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(coroutine);
        yield return null;
    }

    private void CannonSet(Transform cannon, bool isIllusion)
    {
        Transform gfx = cannon.Find("GFX");
        Renderer cannonRender = gfx.GetComponent<Renderer>();
        Color color = cannonRender.material.GetColor("_BaseColor");

        if(isIllusion)
        {
            foreach (Component c in cannon.GetComponents<Component>())
                if (!(c is Transform) && !(c is Collider))
                    Destroy(c);

            color *= 0.5f;
            cannon.gameObject.AddComponent<IllusionCannon>();            
            gfx.gameObject.AddComponent<IllusionCannon>();
        }
        else
        {
            if (!cannon.gameObject.activeInHierarchy)
            {
                gfx.gameObject.SetActive(true);
                cannon.gameObject.SetActive(true);
            }
            Destroy(cannon.GetComponent<IllusionCannon>());
            Destroy(gfx.GetComponent<IllusionCannon>());            
            cannon.gameObject.AddComponent<Cannon>();
            cannon.gameObject.AddComponent<AimingBehaviour>();
            color /= 0.5f;
        }

        gfx.GetComponent<Collider>().isTrigger = isIllusion;
        cannonRender.material.SetColor("_BaseColor", color);

    }

    protected override void SetDifficulty()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            BossHit();
        }
    }

    public class IllusionCannon : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
        }

    }

}
