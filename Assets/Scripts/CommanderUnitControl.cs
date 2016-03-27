using UnityEngine;
using System.Collections;

public class CommanderUnitControl : MonoBehaviour {

    private ArrayList selectedUnits = new ArrayList();
    private GameObject unitAbilitiesPanel;

	// Use this for initialization
	void Start () {
	    unitAbilitiesPanel = GameObject.Find("Canvas/UnitAbilitiesPanel");
        if(unitAbilitiesPanel != null)
            unitAbilitiesPanel.SetActive(false);
	}
	
    void OnDrawGizmos()
    {
        foreach (Unit unit in selectedUnits)
        {
            Gizmos.DrawWireSphere(unit.transform.position, 1f);
        }
    }

	// Update is called once per frame
	void Update () {
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

        if (Input.GetMouseButtonDown(0))
        {
            LeftClick(mousePosWorld);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightClick(mousePosWorld);
        }

	}

    void LeftClick(Vector3 mousePosWorld)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, mousePosWorld - transform.position, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.green);
            Unit clickedUnit = hit.transform.GetComponent<Unit>();
            if (clickedUnit != null)
            {
                if (selectedUnits.Contains(clickedUnit))//This unit is already selected
                {
                    if (!Input.GetKey(KeyCode.LeftShift))//If multiselect key is not being held, make this the only selected unit
                    {
                        Clear();
                        selectedUnits.Add(clickedUnit);
                        clickedUnit.Select(true);       //change unit color for selection
                    }
                    else
                    {//Otherwise, deselect this unit
                        selectedUnits.Remove(clickedUnit);
                        clickedUnit.Select(false);      //change unit color for selection
                    }
                }
                else//Otherwise add it as a selected unit
                {
                    if (!Input.GetKey(KeyCode.LeftShift)) //If multiselect key is not being held, deselect all other units
                    {
                        Clear();
                    }
                    selectedUnits.Add(clickedUnit);
                    clickedUnit.Select(true);       //change unit color for selection
                }
            }
            else
            {
                Debug.DrawLine(transform.position, mousePosWorld, Color.red);
                if (!Input.GetKey(KeyCode.LeftShift)) //If multiselect key is not being held, deselect all units
                {
                    Clear();
                }
            }
        }
        else
        {
            Debug.DrawLine(transform.position, mousePosWorld, Color.grey);
            if (!Input.GetKey(KeyCode.LeftShift)) //If multiselect key is not being held, deselect all units
            {
                Clear();
            }
        }

        // Display UI Menu for unit abilities. Currently just displays menu if unit is selected. Hides if no units are selected
        if (selectedUnits.Count > 0) 
        {
            unitAbilitiesPanel.SetActive(true);
        }
        else if(selectedUnits.Count == 0)
        {
            unitAbilitiesPanel.SetActive(false);
        }
    }

    void RightClick(Vector3 mousePosWorld)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, mousePosWorld - transform.position, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.magenta);
            Unit clickedUnit = hit.transform.GetComponent<Unit>();
            if (clickedUnit != null)    //We clicked on another unit, so we should contextually decide what action to do
            {
            }
            else //We clicked on the ground, so tell the units to move there
            {
                foreach (Unit unit in selectedUnits)
                {
                    unit.MoveTo(hit.point);
                }
            }
        }
    }

    void Clear()
    {
        foreach(Unit unit in selectedUnits) {
            unit.Select(false);
        }
        selectedUnits.Clear();
    }
}
