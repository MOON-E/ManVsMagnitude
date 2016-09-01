using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FakeChat : MonoBehaviour {
    class FakeUser
    {
        public FakeUser(string _name)
        {
            name = _name;
        }
        public string name;
        public float messageCD = 0.0f;
        public void GenerateAndSendMessage()
        {
            // TODO: 
            // determine a course of action based on direction/distance to target base
            // construct a message object
            // call OnServerMessage() in the Scene's TwitchIRCExample
        }
    }

    int numFakeUsers = 10;
    MonsterGridMovement monster;
    List<FakeUser> fakeUsers;
    PlayerBase targetBase; // fake chat aims here, adjusted in update if there's one closer


	// Use this for initialization
	void Start () {
        monster = GameObject.FindObjectOfType<MonsterGridMovement>();
        for (int i = 0; i < numFakeUsers; i++)
        {
            string name = "user" + i;
            FakeUser fake = new FakeUser(name);
            // TODO: add random CD offset to the message so its not all at once?
            fakeUsers.Add(fake);
        }
	}
	
	// Update is called once per frame
	void Update () {
      

        PlayerBase[] targets = GameObject.FindObjectsOfType<PlayerBase>();
        if (targetBase == null && targets.Length > 0){
            targetBase = targets[0];
        }
        foreach (PlayerBase pbase in targets){
            if (Vector3.Distance(pbase.transform.position, monster.transform.position) < Vector3.Distance(targetBase.transform.position, monster.transform.position))
            {
                targetBase = pbase;
            }
        }
        foreach (FakeUser u in fakeUsers)
        {
            if (u.messageCD <= 0.0f)
            {
                u.GenerateAndSendMessage();
            }
            else
            {
                u.messageCD -= Time.deltaTime;
            }
        }

	}
}


