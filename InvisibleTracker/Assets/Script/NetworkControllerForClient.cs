using UnityEngine;
using Photon;

public class NetworkControllerForClient : Photon.MonoBehaviour
{
	[SerializeField]
	private const string ROOM_NAME  = "RoomA";
	private static PhotonView ScenePhotonView;

	private SoundController soundInfo;

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
		soundInfo = GameObject.Find("SoundManager").GetComponent<SoundController> ();
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label(playerName);
		GUILayout.Label(clipName);
		GUILayout.Label(soundInfo.hensu1.ToString());
		GUILayout.Label(soundInfo.hensu2.ToString());
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
}