using UnityEngine;
using Photon;

public class NetworkControllerForClient : Photon.PunBehaviour
{
	[SerializeField]
	private const string ROOM_NAME  = "RoomA";
	private static PhotonView ScenePhotonView;

	private string PlayerName = "";

	public AudioSource TrackerAudio;
	public AudioClip[] audioClips;

	private string clipname = "";
	private string playername = "";

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings( "v.1.0.0" );
		ScenePhotonView = this.GetComponent<PhotonView>();
		TrackerAudio = GetComponent<AudioSource> ();
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label(PlayerName);
		GUILayout.Label(clipname);
		GUILayout.Label(playername);
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

	[PunRPC]
	void PlaySound(string ClipName, string r_PlayerName)
	{
		if (r_PlayerName == PlayerName) {
			clipname = ClipName;
			playername = r_PlayerName;

			foreach (AudioClip clip in audioClips) {
				if (clip.name == ClipName) {
					TrackerAudio.clip = clip;
					TrackerAudio.Play ();
				}
			}
		}
	}

	public string GetPlayerName(){
		return this.PlayerName;
	}
}