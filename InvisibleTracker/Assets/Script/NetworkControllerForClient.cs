using UnityEngine;
using Photon;

public class NetworkControllerForClient : Photon.PunBehaviour
{
	[SerializeField]
	private const string ROOM_NAME  = "RoomA";
	private static PhotonView ScenePhotonView;

	private string PlayerName = "";

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings( "v.1.0.0" );
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label(PlayerName);
	}

	void OnJoinedLobby()
	{
		PhotonNetwork.JoinOrCreateRoom( ROOM_NAME, new RoomOptions(), TypedLobby.Default );
	}

	void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		PhotonNetwork.CreateRoom( ROOM_NAME );
	}

	void OnJoinedRoom()
	{
		int PlayerID = PhotonNetwork.player.ID;
		ScenePhotonView.RPC("SpawnObject", PhotonTargets.MasterClient, PlayerID);
		PlayerName = "Player" + PlayerID;
		Debug.Log(PlayerName);
	}

	public string GetPlayerName(){
		return this.PlayerName;
	}
}