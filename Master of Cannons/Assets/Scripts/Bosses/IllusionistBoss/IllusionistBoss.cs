using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionistBoss : MonoBehaviour
{
    CannonsObjects cannons = null;

    [SerializeField] float movementSpeed = 0;
    //[SerializeField] Transform character = null;
    [SerializeField] Transform cannonsContainer = null;
    List<Transform> cannonsList;

    private void Awake()
    {
        cannons = Resources.Load<CannonsObjects>("Scriptable Objects/Cannons Objects");
        for(int i = 0; i < cannonsContainer.childCount; i++)
            cannonsList.Add(cannonsContainer.GetChild(i));

        StartCoroutine(SetIllusionCannons());
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, character.position, Time.deltaTime * movementSpeed);
    }

    IEnumerator SetIllusionCannons()
    {

        yield return new WaitForSeconds(1f);
        for(int i = 0; i < cannonsList.Count; i++)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber >= 0.5f) continue;

            foreach (Component c in cannonsList[i].GetComponents<Component>())
            {
                if (cannonsList[i].Find("Reference").childCount == 0 && !(c is Transform) && !(c is Collider))
                {
                    Destroy(c);
                }
            }

            Renderer cannonRender = cannonsList[i].Find("GFX").GetComponent<Renderer>();
            Color color = cannonRender.material.GetColor("_BaseColor") * 0.7f;            
            cannonRender.material.SetColor("_BaseColor", color);
            cannonsList[i].gameObject.AddComponent<IllusionCannon>();
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
