using UnityEngine;

public class SlowMoCharacter : Character
{
    private float slowMoScale = 0.15f;

    protected override void Start()
    {
        base.Start();
        //hasSpecial = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasSpecial && other.gameObject.layer == 10)
            StartCoroutine(SlowMotion());        
    }

    System.Collections.IEnumerator SlowMotion()
    {
        Time.timeScale = slowMoScale;
        Time.fixedDeltaTime = 0.02f * slowMoScale;
        yield return new WaitUntil(() => transform.parent == null);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
