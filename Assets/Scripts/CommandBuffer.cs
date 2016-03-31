using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandBuffer : MonoBehaviour {

	public int threshold = 1;
    public int specialThreshold = 10;
	public MonsterGridMovement monster;

	public int [] moveBuffers = new int[5];
    public int specialBuffer;
    public Slider[] uiSliders = new Slider[5];

	public GameObject preFabAlert;
	public GameObject fireBreathSound;

    public InputField scaleInput;

	public void Input(int i, string name) {
        if (i == 4) {
            specialBuffer++;
            if (specialBuffer >= specialThreshold) {
                monster.Command(i);
                specialBuffer = 0;
				alert(i,name);
				Instantiate(fireBreathSound);
            }

            uiSliders[i].value = specialBuffer;
        }
        else if (i == 5) {
            Instantiate(Resources.Load("Audio/Kappa"));
        }
        else {
            moveBuffers[i]++;

            if (moveBuffers[i] >= threshold) {
                monster.Command(i);
                moveBuffers[i] = 0;
				alert(i,name);
            }
            
            uiSliders[i].value = moveBuffers[i];
        }

    }

    public void Color(Color c, string name)
    {
        monster.gameObject.GetComponent<Renderer>().material.color = c;
        monster.head.GetComponent<Renderer>().material.color = c;
		alert(c,name);
    }

    public void Scale()
    {
        int x = int.Parse(scaleInput.text);

        if(x > 0) {
            threshold = (int.Parse(scaleInput.text));
            specialThreshold = 5 * (int.Parse(scaleInput.text));
        }
    }

	void alert(int i, string name) {
		GameObject panel = GameObject.Find ("MainPanel");
		if (panel != null) {
			GameObject a = (GameObject)Instantiate (preFabAlert);
			if (i <= 3) {
				a.GetComponent<Text>().text = name + " moved the Monster!";
			}
			else {
				a.GetComponent<Text>().text = name + " activated the Special Attack!";
			}
			a.transform.SetParent (panel.transform, false);
		}
	}

	void alert(Color c, string name) {
		GameObject panel = GameObject.Find ("MainPanel");
		if (panel != null) {
			GameObject a = (GameObject)Instantiate (preFabAlert);
			a.GetComponent<Text>().text = name + " changed the monster's color!";
			a.transform.SetParent (panel.transform, false);
		}
	}
}

