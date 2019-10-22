using UnityEngine;

public class UIGameStars : MonoBehaviour
{
    Animator[] starsAnimator = new Animator[3];

    private readonly string starInState = "Star In";

    private void Awake()
    {
        starsAnimator = GetComponentsInChildren<Animator>();
    }

    private void Start()
    {
        CollectibleManager.OnCollectibleAdded += ActiveStar;
    }

    private void ActiveStar(CollectibleType _CollectibleType)
    {
        if (_CollectibleType == CollectibleType.Star)
        {
            starsAnimator[CollectibleManager.CollectedStars - 1].Play(starInState);
        }
    }
}
