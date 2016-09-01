using UnityEngine;
using System.Collections;

public class MissileBlast : MonoBehaviour {

    public float mBlastTime = 10;
    float timeUntilBlast;
    float blastRadius = 15;
    Renderer rend;

	public GameObject missileExplode;
    private Transform blastArea;

    // Use this for initialization
    void Start () {
        timeUntilBlast = mBlastTime;
        rend = GetComponent<Renderer>();
        blastArea = this.transform.GetChild(0);
        Debug.DrawLine(blastArea.position, new Vector3(0, blastArea.GetComponent<SphereCollider>().radius * blastArea.localScale.y, 0) + blastArea.position, Color.blue, 10);
    }
	
	// Update is called once per frame
	void Update () {
        timeUntilBlast -= Time.deltaTime;
        float g = timeUntilBlast / mBlastTime;
        rend.material.color = new Color(1f, 1f * g, 1f * g);

        MonsterGridMovement monster = null;

        if (timeUntilBlast <= float.Epsilon) {
			Instantiate(missileExplode);
            Collider[] hitColliders = Physics.OverlapSphere(blastArea.transform.position, blastArea.GetComponent<SphereCollider>().radius*blastArea.localScale.x);
            foreach (Collider c in hitColliders) {
                if (c.gameObject.GetComponent<MonsterGridMovement>()) monster = c.gameObject.GetComponent<MonsterGridMovement>();
            }
            if (monster != null) monster.TakeDamage(5);

            Destroy(this.gameObject);
        }
    }
}
