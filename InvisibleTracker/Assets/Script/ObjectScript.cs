using System.Collections;
using System.Collections.Generic;
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

	public GameObject GameManager;

	public int PlayerAscore;
	public int PlayerBscore;

	public int scorePlayer;

	public GameManagerScript gameManagerScript;

	void Start () {
		GameManager = GameObject.Find ("GameManager");
		objectCollisionAudio = this.GetComponent<AudioSource> ();
		gameManagerScript = GameManager.GetComponent<GameManagerScript> ();
	}
	
	void Update () {
		
	}

	void OnCollisionEnter (Collision col){
		objectCollisionAudio.clip = objectCollisionClip[Random.Range (0,3)];
		AudioSource.PlayClipAtPoint (objectCollisionAudio.clip, this.gameObject.transform.position);
		if(col.gameObject.tag == "TrackerA"){
			scorePlayer = 1;
		}
		if(col.gameObject.tag == "TrackerB"){
			scorePlayer = 2;
		}
		//ゴールに入った処理
		if(col.gameObject.tag == "Goal"){
			Debug.Log ("Goalに衝突");
			switch (scorePlayer) {
			case 1:
				gameManagerScript.scoreA += 10;
				Destroy (gameObject);
				break;
			case 2:
				gameManagerScript.scoreB += 10;
				Destroy (gameObject);
				break;
			}
		}
	}
}
