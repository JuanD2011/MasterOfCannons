using System.Collections;
using UnityEngine;

public class SlowMoCharacter : Character
{
    private float slowMoScale = 0.15f;
    bool inSlowMo = false;

    protected override void Start()
    {
        base.Start();
        specialTime = 10f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpecial && other.gameObject.layer == 10)
            StartCoroutine(SlowMotion());
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (!inSlowMo && hasSpecial && other.gameObject.layer == 10)
    //        StartCoroutine(SlowMotion());
    //}

    protected override IEnumerator OnSpecial()
    {
        StartCoroutine(base.OnSpecial());
        StartCoroutine(SlowMotion());
        yield return null;
    }

    System.Collections.IEnumerator SlowMotion()
    {
        //inSlowMo = true;
        Time.timeScale = slowMoScale;
        Time.fixedDeltaTime = 0.02f * slowMoScale;
        yield return new WaitUntil(() => transform.parent == null || hasSpecial == false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        //inSlowMo = false;
    }


}
