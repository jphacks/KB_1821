using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControllerForClient : MonoBehaviour {

	private NetworkControllerForClient client;
	private int TargetNum;

	// Use this for initialization
	void Start () {
		client = this.GetComponent<NetworkControllerForClient>();
		TargetNum = 0;
	}

	// Update is called once per frame
	void Update () {
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

		if(TargetNum != targets.Length){
			TargetNum = targets.Length;
			string PlayerName = client.GetPlayerName();

			foreach(GameObject target in targets)
			{
				if(!(target.transform.name.IndexOf(PlayerName) > -1))
				{
					target.SetActive(false);
					TargetNum -= 1;
				}
			}
		}
	}
}