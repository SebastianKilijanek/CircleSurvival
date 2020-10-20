using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTapButton : TapButton
{
    protected override IEnumerator LifeTimeCounter()
    {
        while (Timer < LifeTime)
        {
            Timer += Time.deltaTime;
            yield return null;
        }

        Deactivate();
    }

    protected override void OnButtonTap()
    {
        GameOver();
    }
}
