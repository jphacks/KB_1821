using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon;

//オブジェクト自体が自分で管理する処理たち
//衝突押下音再生，ゴール到達判定，ゲームマネージャーへの得点加算を担当

//ぶつかったときの音源はオブジェクト自体が保有　オブジェクトの種類によって音を変えたいから
//そのためトラッカーと衝突時の効果音はオブジェクトから再生する
//あと衝突したトラッカーのプレイヤー判定はオブジェクトが行う　そのプレイヤに対してゴール時に得点加算する
//ゴール判定も当たり判定を使って済ませたいのでオブジェクト側で行う
//トラッカたープレイヤ

public class ObjectScript : Photon.MonoBehaviour {

	public AudioSource objectCollisionAudio;
	public AudioClip[] objectCollisionClip;

	string m_SceneName;
	private GameObject gameManager;
	private GameManagerScript gameManagerScript;
	private NetworkControllerForHost networkInfo;

	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
		foreach (Transform t in ts) if (t.gameObject.name.IndexOf (withName) > -1) return t.gameObject;
		return null;
	}

	void Start () {
		objectCollisionAudio = this.GetComponent<AudioSource> ();

		gameManager = GameObject.Find("GameManagerForHostServer");
		gameManagerScript = gameManager.GetComponent<GameManagerScript> ();
		networkInfo = gameManager.GetComponent<NetworkControllerForHost> ();
	}
	
	void Update () {
	}

	void OnCollisionEnter (Collision col){
		string objectName = this.transform.name;
		GameObject player = getChildGameObject (col.transform.gameObject, "Player");

		if (player != null)
		{
			string playerName = player.transform.name;

			if(col.gameObject.tag == "Goal"){
				Debug.Log ("in Goal!");
				switch (playerName) {
				case "Player2":
					gameManagerScript.scoreA += 10;
					Debug.Log(gameManagerScript.scoreA);
					Debug.Log(gameManagerScript.scoreB);
					break;
				case "Player3":
					gameManagerScript.scoreB += 10;
					Debug.Log(gameManagerScript.scoreA);
					Debug.Log(gameManagerScript.scoreB);
					break;
				}
				Destroy (this.gameObject);
			}
		}
	}
}
