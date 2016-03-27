using UnityEngine;
using System.Collections;

public class CommanderUnitControl : MonoBehaviour {

    private ArrayList selectedUnits = new ArrayList();

    public Unit[] units;
    //public ArrayList allUnits = new ArrayList();        //List of all units for drag select purposes

    private GameObject unitAbilitiesPanel;

    private Vector2 startBoxPos = Vector2.zero;           //drag select box
    private Vector2 endBoxPos = Vector2.zero;

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
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 13f));

        if (Input.GetMouseButtonDown(0))
        {
            LeftClick(mousePosWorld);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            RightClick(mousePosWorld);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (startBoxPos == Vector2.zero) {
                startBoxPos = Input.mousePosition;
                Debug.Log("startBoxPos = " + startBoxPos);
            }
        }
        if (Input.GetKey(KeyCode.Mouse0)) {
                endBoxPos = Input.mousePosition;
                Debug.Log("endBoxPos = " + endBoxPos);
            }
        else {
            startBoxPos = Vector2.zero;
            endBoxPos = Vector2.zero;
        }

        if (startBoxPos != Vector2.zero && endBoxPos != Vector2.zero) {
            Rect selectionBox = new Rect(startBoxPos.x, startBoxPos.y, (endBoxPos.x - startBoxPos.x), (endBoxPos.y - startBoxPos.y));
            foreach (Unit u in units) {
                if (selectionBox.Contains(u.ScreenPosition(), true)) {
                    selectedUnits.Add(u);
                    u.Select(true);
                }
            }
        }
    }

    void LeftClick(Vector3 mousePosWorld)
    {
        RaycastHit hit;
        if (Physics.Raycast(mousePosWorld, new Vector3(0, -1, 0), out hit))
        {
            Debug.DrawLine(mousePosWorld, hit.point, Color.green);
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
            else if (Input.GetKey(KeyCode.LeftShift)) {
                //if (startCorner == null) startCorner = Input.mousePosition;
                //else selectionRect = new Rect(Mathf.Min(startCorner.x, Input.mousePosition.x), Mathf.Min(startCorner.y, Input.mousePosition.y),
                //    Mathf.Abs(startCorner.x - Input.mousePosition.x), Mathf.Abs(startCorner.y - Input.mousePosition.y));
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
            //unitAbilitiesPanel.SetActive(true);
        }
        else if(selectedUnits.Count == 0)
        {
            //unitAbilitiesPanel.SetActive(false);
        }
    }

    void RightClick(Vector3 mousePosWorld)
    {
        RaycastHit hit;
        if (Physics.Raycast(mousePosWorld, new Vector3(0, -1, 0), out hit))
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

    void OnGUI()
    {
        if (startBoxPos != Vector2.zero && endBoxPos != Vector2.zero) {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            GUI.skin.box.normal.background = texture;
            GUI.Box(new Rect(startBoxPos.x, -startBoxPos.y+Screen.height, (endBoxPos.x - startBoxPos.x), (- endBoxPos.y + startBoxPos.y)), GUIContent.none);
        }
        
    }
}
