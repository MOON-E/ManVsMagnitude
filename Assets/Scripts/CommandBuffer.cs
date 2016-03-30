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

	public void Input(int i) {
        if (i == 4) {
            specialBuffer++;
            if (specialBuffer >= specialThreshold) {
                monster.Command(i);
                specialBuffer = 0;
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
            }
            
            uiSliders[i].value = moveBuffers[i];
        }

    }
}