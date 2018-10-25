using UnityEngine;

public class NetworkControllerForHost : MonoBehaviour
{
	[SerializeField]
	private string  m_resourcePath_A  = "";
	[SerializeField]
	private string  m_resourcePath_B  = "";
	[SerializeField]
	private GameObject[] objectList = null; // We lerp towards this

	private const string ROOM_NAME  = "RoomA";

	private static PhotonView ScenePhotonView;
	public static int playerID;

	private GameObject PlayerA = null;
	private GameObject PlayerB = null;

	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name.IndexOf (withName) > -1) return t.gameObject;
		return null;
	}

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings( "v.1.0.0" );
		ScenePhotonView = this.GetComponent<PhotonView>();
		
		PlayerA = (GameObject) Resources.Load("Prefabs/Player2");
		PlayerB = (GameObject) Resources.Load("Prefabs/Player3");
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
							GameObject obj  = Instantiate( PlayerA, controller.transform.position, Quaternion.identity);
							obj.transform.name = "Player" + PlayerID;
							obj.transform.parent = controller.transform;
							break;
						}
						else if (PlayerID == 3)
						{
							GameObject obj  = Instantiate( PlayerB, controller.transform.position, Quaternion.identity);
							obj.transform.name = "Player" + PlayerID;
							obj.transform.parent = controller.transform;
							break;   
						}
					// }
				}
					
			// }
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            // stream.SendNext(transform.position);
            // stream.SendNext(transform.rotation);
        }
        else
        {
            // Network player, receive data
            // this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            // this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }
}