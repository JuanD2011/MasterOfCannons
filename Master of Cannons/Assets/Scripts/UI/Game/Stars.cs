using System.Collections;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] private float timeFirstStar = 2f;
    [SerializeField] private float timeBetweenStars = 0.5f;

    private Animator[] starsAnimators = new Animator[3];

    private readonly string inState = "In";

    private WaitForSeconds waitBetweenStars = null;
    private WaitForSeconds waitFirstStar = null;

    private void Awake()
    {
        waitBetweenStars = new WaitForSeconds(timeBetweenStars);
        waitFirstStar = new WaitForSeconds(timeFirstStar);

        starsAnimators = GetComponentsInChildren<Animator>();

        //StartCoroutine(AnimateStars());
    }

    private void Start()
    {
        StartCoroutine(AnimateStars());
    }

    private IEnumerator AnimateStars()
    {
        byte starCount = 0;

        yield return waitFirstStar;

        while (starCount < ScoreManager.Instance.Stars)
        {
            starsAnimators[starCount].Play(inState);
            starCount += 1;
            yield return waitBetweenStars;
        }
    }
}
