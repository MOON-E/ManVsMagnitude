using UnityEngine;
using System.Collections;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using UnityEngine.UI;

public class Table : DynamoDB {

	private IAmazonDynamoDB _client;
	private DynamoDBContext _context;
	private Text txt;

	private DynamoDBContext Context
	{
		get
		{
			if(_context == null)
				_context = new DynamoDBContext(_client);

			return _context;
		}
	}

	void Awake() 
	{
		_client = Client;
	}

	void CreateUser(string Username)
	{
		user newestuser = new user {
			username = Username,
			NumTimesDragonGreen = 0
		};

		Context.SaveAsync (newestuser);
	}


	[DynamoDBTable("UserData")]
	public class user
	{
		[DynamoDBHashKey]
		public string username { get; set; }
		[DynamoDBProperty]
		public int NumTimesDragonGreen { get; set; }

	}
}
