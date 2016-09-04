using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GridNode : MonoBehaviour
{

    public int x, y;
    GridManager gm;
    public bool startBase;
    //GameObject pBase;

    public Building building = null;   //Building holder

    public int buildStatus = 0; // 1+ = can build here

    Renderer rend;

    bool hovering; //for hover select

    public bool onFire;     //for panic
    GameObject fireParticle;

    //AudioSource variables for sound
    public AudioSource buildingBuild;
    public AudioSource buildingDestroyed;

    //floating screen to interact with the gridnode/building
    public Canvas interactScreen;
    public float fadeSpeed;
    CanvasGroup allUI;
    public Button[] buttons;
    private Button destroyButton;
    private Button actionButton;

    void Start()
    {
        allUI = interactScreen.GetComponent<CanvasGroup>();
        allUI.alpha = 0f;

        destroyButton = buttons[2].GetComponent<Button>();
        actionButton = buttons[1].GetComponent<Button>();

        gm = GetComponentInParent<GridManager>();
        rend = GetComponent<Renderer>();

        if (startBase)
        {
            building = Instantiate(Resources.Load("Prefabs/PlayerBase", typeof(Pylon)), transform.position, transform.rotation) as Pylon;
            building.StartBuild(this);
        }
    }

    void Update()
    {
        //if (hovering) rend.material.color = Color.blue;
        if (buildStatus <= 0) rend.material.color = Color.white;
        else rend.material.color = new Color(.2f, .2f, 1f);
    }

    public void Build(Building new_building)
    {
        Instantiate(buildingBuild);
        buildingBuild.Play();
        print("build");
        building = new_building;
    }

    public void Activate()
    {
        if (BuildingExistsAndComplete())
        {
            Pylon p = building as Pylon;
            if (p != null)
            {
                int range = p.pylonRange;
                for (int xi = -1 * range; xi <= range; xi++)
                {
                    for (int yi = -1 * range; yi <= range; yi++)
                    {
                        try
                        {
                            gm.FindNode(x + xi, y + yi).buildStatus += 1;
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }

    public bool Destroy()
    {
        Instantiate(buildingDestroyed);
        bool r = DestroyBuilding();
        Instantiate(Resources.Load("Particles/Demolish Particles"), transform.position, Quaternion.Euler(-90, 0, 0));
        gameObject.SetActive(false);
        return r;
    }

    //need a void method to be called by commander's destroy button
    //no concern about return value of destroybuilding, because it must always be false (destroy button is disabled is building is playerbase)
    public void CommanderDestroy()
    {
        DestroyBuilding(false);
    }

    public bool DestroyBuilding(bool smashedByMonster = true)
    { //returns true if the destroyed building was a base
        if (BuildingExistsAndComplete())
        {
            Pylon p = building as Pylon;
            if (p != null)
            {
                int range = p.pylonRange;
                for (int xi = -1 * range; xi <= range; xi++)
                {
                    for (int yi = -1 * range; yi <= range; yi++)
                    {
                        try { gm.FindNode(x + xi, y + yi).buildStatus -= 1; }
                        catch { }
                    }
                }
            }
            Mine m = building as Mine;
            if (m != null && smashedByMonster) // if the monster stepped on the mine, don't destroy it. Just trigger it.
            {
                // TODO: maybe don't do anything, see if it works.
                return false;
            }
            Destroy(building.gameObject);
            if (startBase) return true;
        }
        return false;
    }

    public bool HasBarrier()
    {
        if (BuildingExistsAndComplete() && building is Barrier)
            return true;
        return false;
    }

    void OnMouseEnter()
    {
        hovering = true;
        UpdateOptions();
        StartCoroutine(FadeInUI(fadeSpeed));
    }

    void OnMouseExit()
    {
        hovering = false;
        StartCoroutine(FadeOutUI(fadeSpeed));

    }

    public bool CanBuildHere()
    {
        if (onFire) return false;
        if ((buildStatus > 0) && (building == null))
        {
            return true;
        }

        return false;
    }

    public bool BuildingExistsAndComplete()
    {
        if (building != null && building.Completed())
            return true;
        return false;
    }

    public void Ignite()
    {
        if (onFire) return;
        onFire = true;
        fireParticle = Instantiate(Resources.Load("Particles/FirePrefab"), transform.position, transform.rotation) as GameObject;
        Instantiate(Resources.Load("Audio/PanicingCivilianSound"), transform.position, Quaternion.identity);
        StartCoroutine(FireSpread());
    }

    public void Extinguish()
    {
        Destroy(fireParticle);
        onFire = false;
    }

    void OnMouseDown()
    {
        Extinguish();

        if (building is ManualFireBuilding)
        {
            if (building is Missile) {
                ((Missile)building).Launch();
            }
            if (building is Railgun)
            {
                ((Railgun)building).Launch();
            }
        }
        if (building is Factory)
        {
            ((Factory)building).StartBuilding();
        }
    }

    IEnumerator FireSpread()
    {
        yield return new WaitForSeconds(10f);
        if (onFire)
            gm.Panic();
    }


    IEnumerator FadeInUI(float speed)
    {
        
        while (allUI.alpha < 1f && hovering)
        {
            allUI.alpha += speed * Time.deltaTime;
            //Debug.Log(allUI.alpha + ", " + speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator FadeOutUI(float speed)
    {
        while (allUI.alpha > 0f && !hovering)
        {
            allUI.alpha -= speed * Time.deltaTime;
            yield return null;
        }
    }

    private void UpdateOptions()
    {
        if (!BuildingExistsAndComplete())
        {
            destroyButton.interactable = false;
        }
        else if (building is PlayerBase)
        {
            destroyButton.interactable = false;
            Debug.Log("found base");
        }
        else
        {
            destroyButton.interactable = true;
        }
    }
}
