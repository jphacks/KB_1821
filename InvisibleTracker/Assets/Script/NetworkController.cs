using UnityEngine;

public class NetworkController : MonoBehaviour
{
    [SerializeField]
    public string  m_resourcePath  = "";
    [SerializeField]
    private float   m_randomCircle  = 4.0f;

    private const string ROOM_NAME  = "RoomA";

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings( "v1.0.0" );
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void OnJoinedLobby()
    {
        // ルームのどれかに入室する
        PhotonNetwork.JoinOrCreateRoom( ROOM_NAME, new RoomOptions(), TypedLobby.Default );
    }

    void OnJoinedRoom()
    {
		GameObject[] cotrollers = GameObject.FindGameObjectsWithTag("Controller"); 
		foreach (GameObject controller in cotrollers) {
			var renderModel = controller.GetComponentInChildren<SteamVR_RenderModel> ();
			if (renderModel != null) {
				string renderModelName = renderModel.renderModelName;
				if (renderModelName != null && renderModelName.IndexOf ("{htc}vr_tracker_vive_1_0") > -1) {
					SpawnObject(controller);
					NetworkCharacter networkCharacter = controller.GetComponentInChildren<NetworkCharacter> ();
					networkCharacter.setControllerName (controller.transform.name);
				}	
			}
		}
    }

    void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom( ROOM_NAME, new RoomOptions(), TypedLobby.Default );
    }

	public void SpawnObject(GameObject target)
    {
		GameObject obj  = PhotonNetwork.Instantiate( m_resourcePath, target.transform.position, Quaternion.identity, 0 );
        obj.transform.parent = target.transform;
    }

    private Vector3 GetRandomPosition()
    {
        var rand = Random.insideUnitCircle * m_randomCircle;
        return rand;
    }
}