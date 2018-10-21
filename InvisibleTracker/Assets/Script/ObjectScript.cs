using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//オブジェクト自体が自分で管理する処理たち
//衝突押下音再生，ゴール到達判定，ゲームマネージャーへの得点加算を担当

//ぶつかったときの音源はオブジェクト自体が保有　オブジェクトの種類によって音を変えたいから
//そのためトラッカーと衝突時の効果音はオブジェクトから再生する
//あと衝突したトラッカーのプレイヤー判定はオブジェクトが行う　そのプレイヤに対してゴール時に得点加算する
//ゴール判定も当たり判定を使って済ませたいのでオブジェクト側で行う
//トラッカたープレイヤ

public class ObjectScript : MonoBehaviour {

	public AudioSource objectCollisionAudio;
	public AudioClip[] objectCollisionClip;

	string m_SceneName;
	public GameObject GameManager;
	public GameManagerScript GameManagerScript;

	public int PlayerAscore;
	public int PlayerBscore;

	public int scorePlayer;

	private int PlayerID;

	void Start () {
		m_SceneName = SceneManager.GetActiveScene ().name;
		objectCollisionAudio = this.GetComponent<AudioSource> ();
		if (m_SceneName == "HostServer_Demo") {
			GameManager = GameObject.Find("GameManagerForHost");
			GameManagerScript = this.GetComponent<GameManagerScript> ();
		}
		else if (m_SceneName == "ClientServer_Demo") {
			GameManager = GameObject.Find("GameManagerForClient");
			GameManagerScript = this.GetComponent<GameManagerScript> ();
		}

		PlayerID = PhotonNetwork.player.ID;

	}
	
	void Update () {
		
	}

	void OnCollisionEnter (Collision col){
		objectCollisionAudio.clip = objectCollisionClip[Random.Range (0,3)];
		AudioSource.PlayClipAtPoint (objectCollisionAudio.clip, this.gameObject.transform.position);

		//ゴールに入った処理
		if(col.gameObject.tag == "Goal"){
			Debug.Log ("Goalに衝突");
			switch (PlayerID) {
			case 1:
				GameManagerScript.scoreA += 10;
				break;
			case 2:
				GameManagerScript.scoreA += 10;
				break;
			case 3:
				GameManagerScript.scoreB += 10;
				break;
			}
			Destroy (this.gameObject);
		}
	}
}
