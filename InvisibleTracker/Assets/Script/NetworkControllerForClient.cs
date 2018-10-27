using UnityEngine;
using System.Collections.Generic;
using Photon;

public class NetworkControllerForClient : Photon.MonoBehaviour
{
	[SerializeField]
	private const string ROOM_NAME  = "RoomA";
	private static PhotonView ScenePhotonView;

	private SoundController SoundInfo;

	private string playerName = "";

	private AudioSource TrackerAudio;
	private AudioSource ObjectOnceAudio;
	private List<AudioSource> objectRepeatAudioSources = new List<AudioSource>();

	public AudioClip[] controllerAudioClips;
	public AudioClip[] objectOnceAudioClips;
	public AudioClip[] objectRepeatAudioClips;

	// For Debug
	private string clipName = "";

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings( "v.1.0.0" );

		TrackerAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		ObjectOnceAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

		foreach (AudioClip clip in objectRepeatAudioClips) {
			AudioSource tempAudio = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			tempAudio.clip = clip;
			tempAudio.volume = 0.0f;
			objectRepeatAudioSources.Add(tempAudio);
		}

		foreach (AudioSource source in objectRepeatAudioSources) {
			source.Play();
		}
		
		ScenePhotonView = this.GetComponent<PhotonView>();
		TrackerAudio = GetComponent<AudioSource> ();

		SoundInfo = GameObject.Find("SoundManager").GetComponent<SoundController>();
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		GUILayout.Label(playerName);
		GUILayout.Label(clipName);
		// GUILayout.Label(soundInfo.hensu1.ToString());
		// GUILayout.Label(soundInfo.hensu2.ToString());
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
		SoundInfo.SetPlayerID(playerID);
		Debug.Log(playerName);
	}

	void Update(){
		if(playerName != ""){

			if(playerName == "Player2"){
				foreach (AudioSource source in objectRepeatAudioSources) {
					foreach (KeyValuePair<string, float> pair in SoundInfo.requestedVolumeDictForPlayerA) {
						if(pair.Key == source.clip.name){
            				source.volume = pair.Value;
            				break;
						}
        			}
				}
			}
			
			else if(playerName == "Player3"){
				foreach (AudioSource source in objectRepeatAudioSources) {
					foreach (KeyValuePair<string, float> pair in SoundInfo.requestedVolumeDictForPlayerB) {
						if(pair.Key == source.clip.name){
            				source.volume = pair.Value;
            				break;
						}
        			}
				}
			}
			
		}
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

			foreach (AudioClip clip in objectOnceAudioClips) {
				if (clip.name == ClipName) {
					ObjectOnceAudio.clip = clip;
					ObjectOnceAudio.Play ();
				}
			}
		}
	}

	public string GetPlayerName(){
		return this.playerName;
	}
}