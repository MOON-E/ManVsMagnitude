using UnityEngine;
using System.Collections;

public class MissileBlast : MonoBehaviour {

    public float mBlastTime = 10;
    float timeUntilBlast;
    float blastRadius = 15;
    Renderer rend;

	public GameObject missileExplode;

    // Use this for initialization
    void Start () {
        timeUntilBlast = mBlastTime;
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        timeUntilBlast -= Time.deltaTime;
        float g = timeUntilBlast / mBlastTime;
        rend.material.color = new Color(1f, 1f * g, 1f * g);

        MonsterGridMovement monster = null;

        if (timeUntilBlast <= float.Epsilon) {
			Instantiate(missileExplode);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);
            foreach (Collider c in hitColliders) {
                if (c.gameObject.GetComponent<MonsterGridMovement>()) monster = c.gameObject.GetComponent<MonsterGridMovement>();
            }
            if (monster != null) monster.TakeDamage(5);

            Destroy(this.gameObject);
        }
    }

    void OnGUI()
    {
            GUI.DrawTexture(new Rect(Camera.main.WorldToScreenPoint(transform.position).x - 50,
                                    -Camera.main.WorldToScreenPoint(transform.position).y - 50 + Screen.height,
                                    100, 100),
                Resources.Load("Images/MissileCircle") as Texture);
    }
}
