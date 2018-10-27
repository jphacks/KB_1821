using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class SoundController : Photon.MonoBehaviour {

	[SerializeField]
	private int PlayerID;

	public Dictionary<string, float> objectVolumeDictForPlayerA = new Dictionary<string,float> ();
	public Dictionary<string, float> objectVolumeDictForPlayerB = new Dictionary<string,float> ();

	public int hensu1 = 0;
	public float hensu2 = 0f;

	private PhotonView  m_photonView    = null;
	private string[] objectNameList = {"Piano", "Cat", "Box"};

	// Use this for initialization
	void Start () {
		m_photonView    = GetComponent<PhotonView>();

        foreach (string objectName in objectNameList)
        {
            objectVolumeDictForPlayerA.Add(objectName, 0);
            objectVolumeDictForPlayerB.Add(objectName, 0);
        }

        if( !m_photonView.isMine )
        {
            return;
        }

        Debug.Log("I'm owner");
	}
	
	// Update is called once per frame
	void Update () {
		if( !m_photonView.isMine )
        {
            return;
        }
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

    public void SetPlayerID(int m_Id){
    	this.PlayerID = m_Id;
    }

    public void SetVolumeForPalyerA(Dictionary<string, float> volumeInfo){
    	foreach (string key in volumeInfo.Keys) {
    		objectVolumeDictForPlayerA[key] = volumeInfo[key];
    	}
    }

    public void SetVolumeForPalyerB(Dictionary<string, float> volumeInfo){
    	foreach (string key in volumeInfo.Keys) {
    		objectVolumeDictForPlayerB[key] = volumeInfo[key];
    	}
    }
}
