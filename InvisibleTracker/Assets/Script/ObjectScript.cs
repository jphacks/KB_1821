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
	private GameObject gameManager;
	private GameManagerScript gameManagerScript;

	// public int PlayerAscore;
	// public int PlayerBscore;

	// public int scorePlayer;

	private int PlayerID;

	void Start () {
		m_SceneName = SceneManager.GetActiveScene ().name;
		objectCollisionAudio = this.GetComponent<AudioSource> ();

		if (m_SceneName == "HostServer_Demo") {
			gameManager = GameObject.Find("GameManagerForHostServer");
			gameManagerScript = gameManager.GetComponent<GameManagerScript> ();

		}
		else if (m_SceneName == "ClientServer_Demo") {
			gameManager = GameObject.Find("GameManagerForClient");
			gameManagerScript = gameManager.GetComponent<GameManagerScript> ();
		}
		PlayerID = 1;
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
				gameManagerScript.scoreA += 10;
				Debug.Log(gameManagerScript.scoreA);
				Debug.Log(gameManagerScript.scoreB);
				break;
			case 2:
				gameManagerScript.scoreA += 10;
				Debug.Log(gameManagerScript.scoreA);
				Debug.Log(gameManagerScript.scoreB);
				break;
			case 3:
				gameManagerScript.scoreB += 10;
				Debug.Log(gameManagerScript.scoreA);
				Debug.Log(gameManagerScript.scoreB);
				break;
			}
			Destroy (this.gameObject);
		}
	}
}
