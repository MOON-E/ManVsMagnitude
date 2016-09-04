using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceBuilding : Building {
    public uint resourceAmount;
    public float initialDelay;
    public float repeatRate;

    private bool isGeneratingResources = false;

    public CommanderBuildingControl commanderRef;
    Text resourceLabel;

    new void Start()
    {
        base.Start();
        resourceLabel = GameObject.Find("ResourceLabel").GetComponent<Text>();   
    }

	// Update is called once per frame
	new void Update () {
        base.Update();
        if(mBuildState == BuildState.COMPLETED)
        {
            if (!isGeneratingResources)
            {
                InvokeRepeating("GenerateResources", initialDelay, repeatRate);
                isGeneratingResources = true;
            }
        }
	}

    void GenerateResources()
    {
        commanderRef.resourceCount += resourceAmount;
        resourceLabel.text = "Resource count: " + commanderRef.resourceCount;
        Debug.Log("Resources now: " + commanderRef.resourceCount);
    }
}
