using UnityEngine;
using System.Collections;

public class Missile : Building {

    public float chargeTime;
    public bool ready;

    void Start()
    {
        base.Start();
        ready = true;
    }

    public void launch()
    {
        if (!ready) return;

        Camera.main.GetComponent<CommanderBuildingControl>().MissileLaunch();

        Debug.Log("missile click");
    }
}
