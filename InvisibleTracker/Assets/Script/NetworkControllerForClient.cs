using UnityEngine;
using Photon;

public class NetworkControllerForClient : Photon.PunBehaviour
{
	[SerializeField]
	private const string ROOM_NAME  = "RoomA";
	private static PhotonView ScenePhotonView;

	private string playerName = "";

	public AudioSource TrackerAudio;
	public AudioClip[] controllerAudioClips;
	public AudioClip[] objectAudioClips;

	// For Debug
	private string clipName = "";

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings( "v.1.0.0" );
		ScenePhotonView = this.GetComponent<PhotonView>();
		TrackerAudio = GetComponent<AudioSource> ();
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label(playerName);
		GUILayout.Label(clipName);
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
		int playerID = PhotonNetwork.player.ID;
		ScenePhotonView.RPC("SpawnObject", PhotonTargets.MasterClient, playerID);
		playerName = "Player" + playerID;
		Debug.Log(playerName);
	}

	[PunRPC]
	void PlayControllerSound(string ClipName, string r_PlayerName)
	{
		if (r_PlayerName == this.playerName) {
			clipName = ClipName;

			foreach (AudioClip clip in controllerAudioClips) {
				if (clip.name == ClipName) {
					TrackerAudio.clip = clip;
					TrackerAudio.Play ();
				}
			}
		}
	}

	[PunRPC]
	void PlayObjectSound(string ClipName, string r_PlayerName)
	{
		if (r_PlayerName == this.playerName) {
			clipName = ClipName;

			foreach (AudioClip clip in objectAudioClips) {
				if (clip.name == ClipName) {
					TrackerAudio.clip = clip;
					TrackerAudio.Play ();
				}
			}
		}
	}

	public string GetPlayerName(){
		return this.playerName;
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