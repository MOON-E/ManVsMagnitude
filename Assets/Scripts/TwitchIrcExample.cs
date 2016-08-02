using System;
using System.Collections;
using System.Collections.Generic;
using Irc;
using UnityEngine;
using UnityEngine.UI;

public class TwitchIrcExample : MonoBehaviour
{
    public InputField UsernameText;
    public InputField TokenText;
    public InputField ChannelText;

    public Text ChatText;
    public InputField MessageText;
	public CommandBuffer cBuff;

    bool kappaCD = true;
    bool panicCD = true;

    /// <summary>
    ///  List of users present in the chat room at any point during this session of gameplay. 
    ///  Temporarily public for ease of debugging.
    /// </summary>
    public List<TwitchUser> Users;
    public List<string> Usernames;
    //public List<String> Users;
    //public Dictionary<String, List<String>> UsersToInputMap;

    ///TODO:
    ///create a map of username to list of user's inputs
    //List <String> GetChatInputsByUser(string user)
    //{
        //return UsersToInputMap.TryGetValue(user,null);
    //}

    void Start()
    {
        //Subscribe for events
        TwitchIrc.Instance.OnChannelMessage += OnChannelMessage;
        TwitchIrc.Instance.OnUserLeft += OnUserLeft;
        TwitchIrc.Instance.OnUserJoined += OnUserJoined;
        TwitchIrc.Instance.OnServerMessage += OnServerMessage;
		TwitchIrc.Instance.OnExceptionThrown += OnExceptionThrown;

        //instantiate list of users:
        Users = new List<TwitchUser>();
    }

    public void Connect()
    {
        TwitchIrc.Instance.Username = UsernameText.text;
        TwitchIrc.Instance.OauthToken = TokenText.text;
        TwitchIrc.Instance.Channel = ChannelText.text;

        TwitchIrc.Instance.Connect();
    }

    //Send message
    public void MessageSend()
    {
        if (String.IsNullOrEmpty(MessageText.text))
            return;

        TwitchIrc.Instance.Message(MessageText.text);
        ChatText.text += "<b>" + TwitchIrc.Instance.Username + "</b>: " + MessageText.text +"\n";
        MessageText.text = "";
    }

    //Open URL
    public void GoUrl(string url)
    {
        Application.OpenURL(url);
    }

    //Receive message from server
    void OnServerMessage(string message)
    {
        if (message == ":>") {
			ChatText.text += "Successfully Connected to Chat" + "\n";
		}
		//ChatText.text += "<b>SERVER:</b> " + message + "\n";
        Debug.Log(message);
    }

    /// <summary>
    /// Finds or creates a record of the user
    /// </summary>
    /// <param name="name"></param>
    /// <param name="userFound"></param>
    /// <returns></returns>
    TwitchUser findUser(string name)
    {
        foreach (TwitchUser user in Users)
        {
            if (user.GetName() == name)
            {
                user.RecordMessage(name);
                return user;
            }
        }

        //if user is not found:
        TwitchUser newUser = new TwitchUser(name);
        Users.Add(newUser);
        Usernames.Add(name);
        return newUser;
    }

    //Receive username that has been left from channel 
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs)
    {
        ChatText.text += "<b>" + channelMessageArgs.From + ":</b> " + channelMessageArgs.Message + "\n";

        // Record User input data
        TwitchUser currentUser = findUser(channelMessageArgs.From);
        currentUser.RecordMessage(channelMessageArgs.Message);

        bool validInput = true;
        Debug.Log("MESSAGE: " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
        ChatText.GetComponentInParent<ScrollRect>()
            .GetComponent<ScrollRect>()
                .verticalNormalizedPosition = 0f;
        if (channelMessageArgs.Message.ToLower() == "up") {
            cBuff.Input(0, channelMessageArgs.From);
            currentUser.IncUp();
        }
        else if (channelMessageArgs.Message.ToLower() == "down") {
            cBuff.Input(1, channelMessageArgs.From);
            currentUser.IncDown();
        }
        else if (channelMessageArgs.Message.ToLower() == "left") {
            cBuff.Input(2, channelMessageArgs.From);
            currentUser.IncLeft();
        }
        else if (channelMessageArgs.Message.ToLower() == "right") {
            cBuff.Input(3, channelMessageArgs.From);
            currentUser.IncRight();
        }
        else if (channelMessageArgs.Message.ToLower() == "fire") {
            cBuff.Input(4, channelMessageArgs.From);
            currentUser.IncFire();
        }
        else if (channelMessageArgs.Message == "Kappa") {
            if (kappaCD) {
                cBuff.Input(5, channelMessageArgs.From);
                kappaCD = false;
                StartCoroutine(KappaWait());
            }
            currentUser.IncKappa();
        }
        else if (channelMessageArgs.Message == "panicBasket") {
            if (panicCD) {
                cBuff.monster.gm.Panic();
                cBuff.monster.gm.Panic();
                panicCD = false;
                StartCoroutine(PanicWait());
				cBuff.alert(6, channelMessageArgs.From);
            }
            currentUser.IncPanicBasket();
        }
        else if (channelMessageArgs.Message.ToLower() == "black") {
            cBuff.Color(Color.black, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "blue") {
            cBuff.Color(Color.blue, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "clear") {
            cBuff.Color(Color.clear, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "cyan") {
            cBuff.Color(Color.cyan, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "gray") {
            cBuff.Color(Color.gray, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "green") {
            cBuff.Color(Color.green, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "grey") {
            cBuff.Color(Color.grey, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "magenta") {
            cBuff.Color(Color.magenta, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "red") {
            cBuff.Color(Color.red, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "white") {
            cBuff.Color(Color.white, channelMessageArgs.From);
        }
        else if (channelMessageArgs.Message.ToLower() == "yellow") {
            cBuff.Color(Color.yellow, channelMessageArgs.From);
        }
        else
        {
            validInput = false;
        }

        if (validInput)
        {
            currentUser.IncValidInputs();
        }
    }

        //Get the name of the user who joined to channel 
        void OnUserJoined(UserJoinedEventArgs userJoinedArgs)
    {
        //ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userJoinedArgs.User + "\n";
        Debug.Log("USER JOINED: " + userJoinedArgs.User);
        //Users.Add(userJoinedArgs.User);
    }


    //Get the name of the user who left the channel.
    void OnUserLeft(UserLeftEventArgs userLeftArgs)
    {
        //ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userLeftArgs.User + "\n";
        Debug.Log("USER LEFT: " + userLeftArgs.User);
    }

    //Receive exeption if something goes wrong
    private void OnExceptionThrown(Exception exeption)
    {
        Debug.Log(exeption);
    }

    public void ChangeChannel()
    {
        TwitchIrc.Instance.Disconnect();
        TwitchIrc.Instance.Channel = ChannelText.text;
        TwitchIrc.Instance.Connect();
    }
    
    IEnumerator KappaWait()
    {
        yield return new WaitForSeconds(5);
        kappaCD = true;
    }

    IEnumerator PanicWait()
    {
        yield return new WaitForSeconds(5);
        panicCD = true;
    }

    // debug code, temporary
    //public void Update()
    //{
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        foreach (TwitchUser u in Users){
    //            u.DebugPrint();
    //        }
    //    }
    //}
}
