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
        SpawnObject();
    }

    void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom( ROOM_NAME, new RoomOptions(), TypedLobby.Default );
    }

    public void SpawnObject()
    {
        GameObject obj  = PhotonNetwork.Instantiate( m_resourcePath, GetRandomPosition(), Quaternion.identity, 0 );
        obj.transform.parent = transform;
    }

    private Vector3 GetRandomPosition()
    {
        var rand = Random.insideUnitCircle * m_randomCircle;
        return rand;
    }
} // class DemoNetwork