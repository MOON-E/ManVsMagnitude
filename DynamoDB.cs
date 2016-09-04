using UnityEngine;
using System.Collections;
using Amazon.DynamoDBv2;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon;

public class DynamoDB : MonoBehaviour {
	public string CognitoPoolRegion = RegionEndpoint.USWest1.SystemName;
	public string DynamoRegion = RegionEndpoint.USWest1.SystemName;
	public string IdentityPoolId = "";

	private RegionEndpoint _CognitoPoolRegion
	{
		get { return RegionEndpoint.GetBySystemName(CognitoPoolRegion); }
	}

	private RegionEndpoint _DynamoRegion
	{
		get { return RegionEndpoint.GetBySystemName(DynamoRegion); }
	}

	private static IAmazonDynamoDB _ddbClient;
	private AWSCredentials _credentials;

	private AWSCredentials Credentials
	{
		get
		{
			if (_credentials == null)
				_credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoPoolRegion);
			return _credentials;
		}
	}

	protected IAmazonDynamoDB Client
	{
		get
		{
			if (_ddbClient == null)
			{
				_ddbClient = new AmazonDynamoDBClient(Credentials, _DynamoRegion);
			}

			return _ddbClient;
		}
	}
}
