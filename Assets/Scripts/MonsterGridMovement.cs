using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterGridMovement : MonoBehaviour
{
	enum state{NORMAL, PLAYERWON};
	private state currstate = state.NORMAL;

    public GridManager gm;
    int facing = 0;                 //direction the monster is currently facing

    public int x = 5;               //Starting coordinates
    public int y = 5;                       
    
    public float shake_duration = 1f;               //Screen Shake variables
    public float shake_magnitude = 0.5f;

    public float stepSpeed = 1f;        //Seconds for monster movement

    float startTime;                    //For monster movement
    Vector3 startPoint, endPoint;

	public int maxHealth = 100;
	public Slider healthSlider;

    public AudioSource tankDeath;       //tank death sound cause screw you arrays

	public GameObject PlayerWinCanvas;
	public GameObject UICanvas;
	public GameObject playerWinSound;

    void Start()
    {
        transform.position = startPoint = endPoint = gm.Find(x, y);
		healthSlider.maxValue = maxHealth;
		healthSlider.value = maxHealth;
    }

    void Update()
    {
        if(transform.position != endPoint) {
            float moveCompletion = (Time.time - startTime) / stepSpeed;
            if (moveCompletion < 1) {
                transform.position = Vector3.Lerp(startPoint, endPoint, moveCompletion);
            }
        }
		if (Input.GetKeyDown(KeyCode.R) && currstate == state.PLAYERWON)
			Application.LoadLevel (0);
    }

    public void Command(int i)
    {
		//TakeDamage (100);
        switch (i) {                //switch case for different inputs
            case 0: Up();
                    break;

            case 1: Down();
                    break;

            case 2: Left();
                    break;

            case 3: Right();
                    break;

            case 4: Special();
                    break;
        }
    }

    public void Step()              //Shared effects between all steps
    {
        startTime = Time.time;
        startPoint = transform.position;
        StartCoroutine(CameraShake.Shake(shake_duration, shake_magnitude));
    }

    public void Up()
    {
        if(y<9) y += 1;
		if (gm.Smash (x, y)) {
			endPoint = gm.Find (x, y);
		} else
			y -= 1;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        facing = 0;
        Step();
    }

    public void Down()
    {
        if (y > 0) y -= 1;
		if (gm.Smash (x, y)) {
			endPoint = gm.Find (x, y);
		} else
			y += 1;
        transform.rotation = Quaternion.Euler(0, 180, 0);
        facing = 1;
        Step();
    }

    public void Left()
    {
        if (x > 0) x -= 1;
		if (gm.Smash (x, y)) {
			endPoint = gm.Find (x, y);
		} else
			x += 1;
        transform.rotation = Quaternion.Euler(0, -90, 0);
        facing = 2;
        Step();
    }

    public void Right()
    {
        if (x < 9) x += 1;
		if (gm.Smash (x, y)) {
			endPoint = gm.Find (x, y);
		} else
			x -= 1;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        facing = 3;
        Step();
    }

    public void Special()
    {
        GridNode[] fireRange = new GridNode[6];
        int i = 0;
        Vector3 fireBreathPosition = transform.position;

        switch(facing) {                    //Destroys 1 block in direction
            case 0:
                for(int j=1; j<=2; j++) {
                    for(int k=-1; k<=1; k++) {
                        try { fireRange[i] = gm.FindNode(x + k, y + j); }
                        catch { fireRange[i] = null; }
                        fireBreathPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z +5);
                        i++;
                    }
                }
                break;

            case 1:
                for (int j = 1; j <= 2; j++) {
                    for (int k = -1; k <= 1; k++) {
                        try { fireRange[i] = gm.FindNode(x + k, y - j); }
                        catch { fireRange[i] = null; }
                        fireBreathPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z-5);
                        i++;
                    }
                }
                break;

            case 2:
                for (int j = 1; j <= 2; j++) {
                    for (int k = -1; k <= 1; k++) {
                        try { fireRange[i] = gm.FindNode(x - j, y + k); }
                        catch { fireRange[i] = null; }
                        fireBreathPosition = new Vector3(transform.position.x-5, transform.position.y, transform.position.z);
                        i++;
                    }
                }
                break;

            case 3:
                for (int j = 1; j <= 2; j++) {
                    for (int k = -1; k <= 1; k++) {
                        try { fireRange[i] = gm.FindNode(x + j, y + k); }
                        catch { fireRange[i] = null; }
                        fireBreathPosition = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
                        i++;
                    }
                }
                break;
        }

        Instantiate(Resources.Load("Particles/FireBreathPrefab"), fireBreathPosition, transform.rotation);

        foreach (GridNode node in fireRange) {
            if(node!=null) gm.Smash(node.x,node.y);
        }


    }

    public void TakeDamage(int dmg)
    {
		healthSlider.value -= dmg;
		if (healthSlider.value <= 0) {
			currstate = state.PLAYERWON;
			Instantiate(playerWinSound);
			UICanvas.SetActive (false);
			PlayerWinCanvas.SetActive (true);
		}
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Unit>()) {
            col.gameObject.SetActive(false);
            Instantiate(tankDeath);
        }
    }
}
