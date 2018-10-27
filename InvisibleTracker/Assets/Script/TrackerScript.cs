using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class TrackerScript : Photon.MonoBehaviour {

	public SteamVR_TrackedObject trackedObject;	//SteamVRでViveトラッカーを扱う宣言

	public bool collisionTrigger;		//いまオブジェクトとぶつかっているか否かを保持　この間にボタン入力するとゲット

	public bool catchButtonTrigger;		//物理ボタン入力のために使用　PogoPin 3番の電圧状況を保持するフラグ
	public bool releaseButtonTrigger;	//物理ボタン入力のために使用　PogoPin 4番の電圧状況を保持するフラグ

	public AudioSource TrackerAudio; 	//音源　一つしか用意していないので重複再生不可　なにか同時に鳴らすならもう一つ増やす
	public AudioClip[] audioClips; 		//ゲット時，リリース時の効果音を格納しておく　鳴らす都度，TrackeAudioに突っ込んで使う

	public GameObject CollisionObject; 	//いまトラッカーにぶつかっているオブジェクトを保持しておく変数

	private NetworkControllerForHost networkInfo;

	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name.IndexOf (withName) > -1) return t.gameObject;
		return null;
	}

	void Start () {
		trackedObject = GetComponent<SteamVR_TrackedObject>();	//トラッカー制御スクリプトの取得
		TrackerAudio = GetComponent<AudioSource> ();			//効果音コンポーネントの取得
		networkInfo = GameObject.Find("GameManagerForHostServer").GetComponent<NetworkControllerForHost> ();
		collisionTrigger = false;								//衝突判定を切っておく
	}

	//オブジェクトと当たっている間は衝突判定フラグをオンにする　このフラグがオンの間にボタン入力したらゲット
	void OnCollisionStay(Collision col){
		if(col.gameObject.tag == "Object"){
			collisionTrigger = true;
		}

		//オブジェクトと衝突したときの処理　効果音再生とオブジェクト情報の取得を行う
		if(col.gameObject.tag == "Object"){
			//1番OutPutから信号出力　LEDを光らせてぶつかっていることを通知
			SteamVR_Controller.Input((int)trackedObject.index).TriggerHapticPulse((ushort)100);
			//当たているオブジェクトを格納しておく　実施にゲットするのはキャッチ処理で行う
			CollisionObject = col.gameObject;
		}
	}

	public void LEDflash(){
		SteamVR_Controller.Input((int)trackedObject.index).TriggerHapticPulse((ushort)10);
	}

	//オブジェクトから離れたら衝突判定フラグをオフにする　
	void OnCollisionExit(Collision col){
		collisionTrigger = false;
	}

	void Update () {
		//ViveコントローラやViveトラッカーを取得，indexをふって格納
		var device = SteamVR_Controller.Input((int)trackedObject.index);

		//ViveトラッカーのPogoPinの電圧（物理ボタンの押下状況）を格納　Gripは3番目，Triggerは4番目のピン
		catchButtonTrigger = device.GetPress(Valve.VR.EVRButtonId.k_EButton_Grip);
		releaseButtonTrigger = device.GetPress(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);

		//空振り時も効果音を鳴らしてあげる
		if(catchButtonTrigger && !collisionTrigger){
			GameObject player = getChildGameObject (this.transform.gameObject, "Player");
			if(player != null)
			{
				Debug.Log ("Call Sound [Missed]");
				networkInfo.PlaySound ("Missed", player.transform.name, "Controller");
			}
			//TrackerAudio.clip = audioClips[0];	//鳴らす効果音を空振り効果音に差し替え
			//TrackerAudio.Play(); 				//ゲット効果音再生
		}
		if(releaseButtonTrigger && !collisionTrigger){	
			GameObject player = getChildGameObject (this.transform.gameObject, "Player");
			if(player != null)
			{
				Debug.Log ("Call Sound [Missed]");
				networkInfo.PlaySound ("Missed", player.transform.name, "Controller");
			}
		}

		//衝突判定がオンになっている間にボタン入力が入ったらオブジェクトをゲットする
		//if (Input.GetKeyDown (KeyCode.A) && collisionTrigger) {		//テスト用のキー操作
		if(catchButtonTrigger && collisionTrigger){				//本番用のトラッカーボタン操作
			GameObject player = getChildGameObject (this.transform.gameObject, "Player");
			if(player != null)
			{
				Debug.Log ("Call Sound [Catch]");
				networkInfo.PlaySound ("Catch", player.transform.name, "Controller");
			}
			CollisionObject.transform.parent = gameObject.transform; //ゲット処理　プレイヤのコントローラに追従するよう親子関係を紐づけ
		}
		//衝突判定がオンになっている間にボタン入力が入ったらオブジェクトをリリースする
		//if (Input.GetKeyDown (KeyCode.B) && collisionTrigger) {		//テスト用のキー操作
		if(releaseButtonTrigger && collisionTrigger){				//本番用のトラッカーボタン操作
			GameObject player = getChildGameObject (this.transform.gameObject, "Player");
			if(player != null)
			{
				Debug.Log ("Call Sound [Release]");
				networkInfo.PlaySound ("Release", player.transform.name, "Controller");
			} 									//リリース効果音再生
			CollisionObject.transform.parent = null; 				//リリース処理　親子関係を解消して追従しないようにする
		}
	}

	void OnCollisionEnter (Collision col ){
		string objectName = col.transform.name;
		GameObject player = getChildGameObject (this.transform.gameObject, "Player");

		if(player != null){
			string playerName = player.transform.name;
			Debug.LogFormat ("Call Sound [{0}] by [{1}]", objectName, playerName);
			networkInfo.PlaySound (objectName, playerName, "Object");
		}
	}
}
