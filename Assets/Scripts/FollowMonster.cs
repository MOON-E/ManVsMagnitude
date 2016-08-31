using UnityEngine;
using System.Collections;

public class FollowMonster : MonoBehaviour {

    public Vector3 CamDisplacement;
    public GameObject monster;
    private Camera componentCamera;

    // Update is called once per frame
    void Update () {
        componentCamera = this.gameObject.GetComponent<Camera>();
        componentCamera.enabled = !MonsterInView();
        if (componentCamera.enabled)
            this.gameObject.transform.position = monster.transform.position + CamDisplacement;
    }

    bool MonsterInView() {
        Vector3 MainCamMonsterLocation = Camera.main.WorldToViewportPoint(monster.transform.position);
        Debug.Log(MainCamMonsterLocation);
        return (MainCamMonsterLocation.x >= 0 && MainCamMonsterLocation.x <= 1 &&
            MainCamMonsterLocation.y >= 0 && MainCamMonsterLocation.y <= 1 &&
            MainCamMonsterLocation.z >= 0);
    }
}
