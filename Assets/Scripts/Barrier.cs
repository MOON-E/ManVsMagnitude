using UnityEngine;
using System.Collections;

public class Barrier : Building {

    void Update()
    {
        base.Update();
        if (mBuildState == BuildState.COMPLETED) {
            isABarrier = true;
        }
    }
}
