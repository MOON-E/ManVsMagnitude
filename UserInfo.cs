using UnityEngine;
using System.Collections;
using Amazon.DynamoDBv2.DataModel;


[DynamoDBTable("UserData")]
public class userinfo
{
	[DynamoDBHashKey]
	public string username { get; set;}

	[DynamoDBProperty]
	public int NumTimesDragonGreen { get; set;} 

}
