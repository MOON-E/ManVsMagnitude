using System;
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

	public GameObject alert;

    void Start()
    {
        //Subscribe for events
        TwitchIrc.Instance.OnChannelMessage += OnChannelMessage;
        TwitchIrc.Instance.OnUserLeft += OnUserLeft;
        TwitchIrc.Instance.OnUserJoined += OnUserJoined;
        TwitchIrc.Instance.OnServerMessage += OnServerMessage;
		TwitchIrc.Instance.OnExceptionThrown += OnExceptionThrown;
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
        ChatText.text += "<b>SERVER:</b> " + message + "\n";
        Debug.Log(message);
    }

    //Receive username that has been left from channel 
    void OnChannelMessage(ChannelMessageEventArgs channelMessageArgs)
    {
        ChatText.text += "<b>" + channelMessageArgs.From + ":</b> " + channelMessageArgs.Message + "\n";
        Debug.Log("MESSAGE: " + channelMessageArgs.From + ": " + channelMessageArgs.Message);
		ChatText.GetComponentInParent<ScrollRect>()
			.GetComponent<ScrollRect>()
				.verticalNormalizedPosition = 0f;
		if (channelMessageArgs.Message == "!up") {
			cBuff.Input(0);
		}
		else if (channelMessageArgs.Message == "!down") {
			cBuff.Input(1);
		}
		else if (channelMessageArgs.Message == "!left") {
			cBuff.Input(2);
		}
		else if (channelMessageArgs.Message == "!right") {
			cBuff.Input(3);
		}
		else if (channelMessageArgs.Message == "!alert") {
			GameObject panel = GameObject.Find("MainPanel");
			if (panel != null) {
				GameObject a = (GameObject)Instantiate(alert);
				a.transform.SetParent(panel.transform, false);
			}
			else {print("fail");}
		}
    }

    //Get the name of the user who joined to channel 
    void OnUserJoined(UserJoinedEventArgs userJoinedArgs)
    {
        ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userJoinedArgs.User + "\n";
        Debug.Log("USER JOINED: " + userJoinedArgs.User);
    }


    //Get the name of the user who left the channel.
    void OnUserLeft(UserLeftEventArgs userLeftArgs)
    {
        ChatText.text += "<b>" + "USER JOINED" + ":</b> " + userLeftArgs.User + "\n";
        Debug.Log("USER JOINED: " + userLeftArgs.User);
    }

    //Receive exeption if something goes wrong
    private void OnExceptionThrown(Exception exeption)
    {
        Debug.Log(exeption);
    }

}
