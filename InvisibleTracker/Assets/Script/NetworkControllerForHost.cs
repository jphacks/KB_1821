using UnityEngine;

public class NetworkControllerForHost : MonoBehaviour
{
	[SerializeField]
	private string  m_resourcePath_A  = "";
	[SerializeField]
	private string  m_resourcePath_B  = "";

	private const string ROOM_NAME  = "RoomA";

	private static PhotonView ScenePhotonView;
	public static int playerID;

	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name.IndexOf (withName) > -1) return t.gameObject;
		return null;
	}

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings( "v.1.0.0" );
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	void Update()
	{
		Debug.Log(PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	void OnJoinedLobby()
	{
		PhotonNetwork.JoinOrCreateRoom( ROOM_NAME, new RoomOptions(), TypedLobby.Default );
	}

	void OnJoinedRoom()
	{
		if (PhotonNetwork.playerList.Length == 1)
		{
			playerID = PhotonNetwork.player.ID;
		}

		Debug.Log("playerID: " + playerID);
	}

	void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		PhotonNetwork.CreateRoom( ROOM_NAME );
	}

	void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerConnected: " + player);
	}

	public void PlaySound(string ClipName, string PlayerName, string mode){
		if(mode == "Controller")
		{
			ScenePhotonView.RPC("PlayControllerSound", PhotonTargets.Others, ClipName, PlayerName);
		}
		else if(mode == "Object")
		{
			ScenePhotonView.RPC("PlayObjectSound", PhotonTargets.Others, ClipName, PlayerName);
		}
		
	}

	[PunRPC]
	void SpawnObject(int PlayerID)
	{
		GameObject[] cotrollers = GameObject.FindGameObjectsWithTag("Controller"); 
		foreach (GameObject controller in cotrollers) {
			// var renderModel = controller.GetComponentInChildren<SteamVR_RenderModel> ();
			// if (renderModel != null) {
				if(getChildGameObject(controller, "Player") == null)
				{
					// string renderModelName = renderModel.renderModelName;
					// if (renderModelName != null && renderModelName.IndexOf ("{htc}vr_tracker_vive_1_0") > -1) {
						if (PlayerID == 2)
						{
							GameObject obj  = PhotonNetwork.Instantiate( m_resourcePath_A, controller.transform.position, Quaternion.identity, 0 );
							obj.transform.name = "Player" + PlayerID;
							obj.transform.parent = controller.transform;
							break;
						}
						else if (PlayerID == 3)
						{
							GameObject obj  = PhotonNetwork.Instantiate( m_resourcePath_B, controller.transform.position, Quaternion.identity, 0 );
							obj.transform.name = "Player" + PlayerID;
							obj.transform.parent = controller.transform;
							break;   
						}
					// }
				}
					
			// }
		}
	}

}