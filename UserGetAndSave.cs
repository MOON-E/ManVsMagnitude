﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using UnityEngine.UI;


public class UserGetAndSave : MonoBehaviour {

	public string cognitoIdentityPoolString;

	private CognitoAWSCredentials credentials;
	private IAmazonDynamoDB _client;
	private DynamoDBContext _context;

	private List<userinfo> users = new List<userinfo>();

	private DynamoDBContext Context
	{
		//this just helps AWS do its thing
		get
		{ 
			if (_context == null)
				_context = new DynamoDBContext (_client);

			return _context;
		}
	}

	public void CreateUserInTable(string newusername)
	{

		//initialize new user account
		var newUser = new userinfo {
			username = newusername,
			NumTimesDragonGreen = 0

		};


		//send it to the cloud
		Context.SaveAsync(newUser, ((result) => 
			{
				if (result.Exception == null)
					Debug.Log(newusername + " has been saved to db");

				//else nothing, you're screwed cause it didn't save.
				else {
					Debug.Log(result.Exception.Message);
				}

			}));
	}

	private void ModifyUserInTable(string username, int newNumTimesDragonGreen)
	{
		userinfo user = null;

		Context.LoadAsync<userinfo>(username, (result) => 
		{
				if(result.Exception == null)
				{
					user = result.Result;
					user.NumTimesDragonGreen = newNumTimesDragonGreen;
					Context.SaveAsync<userinfo>(user, (res) =>
						{
							if(res.Exception == null)
							{
								Debug.Log("updated " + username);
							}
							else 
							{
								Debug.Log(res.Exception.Message);
							}
						});
				}
				else
				{
					Debug.Log(result.Exception.Message);
				}
			
		});

	}

	public void AfterMatchUpdate()
	{
		foreach (userinfo user in users)
		{
			ModifyUserInTable (user.username, user.NumTimesDragonGreen);
		}
	}

	public void updateUser(string username, int newNumTimesDragonGreen)
	{
		//find user
	}


	private void FetchAllUsersFromAWS()
	{
		Debug.Log ("***********Load Table**********");
		Table.LoadTableAsync (_client, "UserData", (loadTableResult) => {
			if (loadTableResult.Exception != null) {
				Debug.Log ("failed to load user table");
			} else {
				try {
					var context = Context;

					var search = context.ScanAsync<userinfo> (new ScanCondition ("NumTimesDragonGreen", ScanOperator.GreaterThanOrEqual, 0));
					search.GetRemainingAsync (result => {
						if (result.Exception == null) {
							users = result.Result;

						} else {
							Debug.LogError ("Failed to load user Table");	
						}
					}, null);
				} catch (AmazonDynamoDBException exception) {
					Debug.Log (string.Concat ("Exception fetching characters from tables", exception.Message));
				}
			}
		});
	}
	void Start()
	{
		//set up the credentials for aws to do its thing

		UnityInitializer.AttachToGameObject (this.gameObject);
		credentials = new CognitoAWSCredentials (cognitoIdentityPoolString, RegionEndpoint.USWest2);
		credentials.GetIdentityIdAsync (delegate(AmazonCognitoIdentityResult<string> result) {
			if (result.Exception != null) {
				Debug.LogError ("exception hit: " + result.Exception.Message);
			}

			var ddbClient = new AmazonDynamoDBClient (credentials, RegionEndpoint.USWest2);

			Debug.Log ("Retreiving table");

			var request = new DescribeTableRequest {
				TableName = @"UserData"
		};

			ddbClient.DescribeTableAsync (request, (ddbresult) => {
				if (result.Exception != null) {
					Debug.Log (result.Exception);
					return;
				}
				var response = ddbresult.Response;

				TableDescription description = response.Table;

			}, null);

			_client = ddbClient;

		//	FetchAllUsersFromAWS ();
		});
	}

}
