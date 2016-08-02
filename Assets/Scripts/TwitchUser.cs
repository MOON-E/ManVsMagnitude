using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwitchUser {
    string name;
    public List<string> chatMessages;

    private int upCount = 0;
    private int downCount = 0;
    private int leftCount = 0;
    private int rightCount = 0;
    private int moveCount = 0 ;
    private int fireCount = 0;
    private int KappaCount = 0;
    private int PanicBasketCount = 0;
    private int ValidInputCount = 0;

    public void IncUp()
    {
        upCount++;
        moveCount++;
    }
    public void IncDown()
    {
        downCount++;
        moveCount++;
    }
    public void IncLeft()
    {
        leftCount++;
        moveCount++;
    }
    public void IncRight()
    {
        moveCount++;
        rightCount++;
    }
    public void IncFire()
    {
        fireCount++;
    }
    public void IncKappa()
    {
        KappaCount++;
    }
    
    public void IncPanicBasket()
    {
        PanicBasketCount++;
    }

    //increment count of chat inputs that have ingame functionality
    public void IncValidInputs()
    {
        ValidInputCount++;
    }

    public void DebugPrint()
    {
        Debug.Log("Data for user " + name);
        Debug.Log("Moves Inputted: " + moveCount);
        Debug.Log("Valid Inputs: "+ValidInputCount);
    }

    public TwitchUser(string name)
    {
        this.name = name;
        chatMessages = new List<string>();
    }

    public void RecordMessage(string message)
    {
        chatMessages.Add(message);
    }

    public string GetName()
    {
        return name;
    }


}
