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

	void Start () {
		m_SceneName = SceneManager.GetActiveScene ().name;
		objectCollisionAudio = this.GetComponent<AudioSource> ();
		if (m_SceneName == "HostServer") {
			GameManager = GameObject.Find("GameManagerForHost");
			GameManagerScript = this.GetComponent<GameManagerScript> ();
		}
		else if (m_SceneName == "ClientServer") {
			GameManager = GameObject.Find("GameManagerForClient");
			GameManagerScript = this.GetComponent<GameManagerScript> ();
		}

	}
	
	void Update () {
		
	}

	void OnCollisionEnter (Collision col){
		objectCollisionAudio.clip = objectCollisionClip[Random.Range (0,3)];
		AudioSource.PlayClipAtPoint (objectCollisionAudio.clip, this.gameObject.transform.position);

		//ゴールに入った処理
		if(col.gameObject.tag == "Goal"){
			Debug.Log ("Goalに衝突");
			switch (scorePlayer) {
			case 1:
				GameManagerScript.scoreA += 10;
				Destroy (gameObject);
				break;
			case 2:
				GameManagerScript.scoreB += 10;
				Destroy (gameObject);
				break;
			}
		}
	}
}
