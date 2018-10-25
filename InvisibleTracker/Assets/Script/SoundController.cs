using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class SoundController : Photon.MonoBehaviour {

	public Dictionary<string, float> objectDistanceDict_forPlayerA = new Dictionary<string,float> ();
	public Dictionary<string, float> objectDistanceDict_forPlayerB = new Dictionary<string,float> ();

	public int hensu1 = 0;
	public float hensu2 = 0f;

	private PhotonView  m_photonView    = null;

	// Use this for initialization
	void Start () {
		m_photonView    = GetComponent<PhotonView>();

		if( !m_photonView.isMine )
        {
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if( !m_photonView.isMine )
        {
            return;
        }

        hensu1 += 1;
        hensu2 += 0.1f;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
            //データの送信
            stream.SendNext(hensu1);
            stream.SendNext(hensu2);
        } else {
            //データの受信
            this.hensu1 = (int)stream.ReceiveNext();
            this.hensu2 = (float)stream.ReceiveNext();
        }
    }
}
