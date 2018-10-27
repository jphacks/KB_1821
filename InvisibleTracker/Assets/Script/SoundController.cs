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

    public bool CompareDict<TKey, TValue>( Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
     {
         if (dict1 == dict2) return true;
         if ((dict1 == null) || (dict2 == null)) return false;
         if (dict1.Count != dict2.Count) return false;
 
         var valueComparer = EqualityComparer<TValue>.Default;
 
         foreach (var kvp in dict1)
         {
             TValue value2;
             if (!dict2.TryGetValue(kvp.Key, out value2)) return false;
             if (!valueComparer.Equals(kvp.Value, value2)) return false;
         }
         return true;
     }

	// Use this for initialization
	void Start () {
		m_photonView    = GetComponent<PhotonView>();

        foreach (string objectName in objectNameList)
        {
            Debug.Log(objectName);
            objectVolumeDictForPlayerA.Add(objectName, 0);
            objectVolumeDictForPlayerB.Add(objectName, 0);
        }

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

        Debug.Log("Volume of PlayerA");
        foreach (KeyValuePair<string, float> pair in objectVolumeDictForPlayerA) {
            Debug.Log (pair.Key + " : " + pair.Value);
        }
        Debug.Log("Volume of PlayerB");
        foreach (KeyValuePair<string, float> pair in objectVolumeDictForPlayerB) {
            Debug.Log (pair.Key + " : " + pair.Value);
        }
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting) {
            //データの送信
            foreach (string objectName in objectNameList)
            {
                stream.SendNext(objectVolumeDictForPlayerA[objectName]);
            }
            foreach (string objectName in objectNameList)
            {
                stream.SendNext(objectVolumeDictForPlayerB[objectName]);
            }
        } else {
            foreach (string objectName in objectNameList)
            {
                objectVolumeDictForPlayerA[objectName] = (float)stream.ReceiveNext();
            }
            foreach (string objectName in objectNameList)
            {
                objectVolumeDictForPlayerB[objectName] = (float)stream.ReceiveNext();
            }
            //データの受信
            // this.hensu1 = (int)stream.ReceiveNext();
            // this.hensu2 = (float)stream.ReceiveNext();
        }
    }

    public void SetPlayerID(int m_Id){
    	this.PlayerID = m_Id;
    }

    public void SetVolumeForPalyerA(Dictionary<string, float> volumeInfo){
        if(!CompareDict(objectVolumeDictForPlayerA, volumeInfo)){
            Debug.Log("called set volume A");

        	foreach (string key in volumeInfo.Keys) {
        		objectVolumeDictForPlayerA[key] = volumeInfo[key];
        	}
        }
    }

    public void SetVolumeForPalyerB(Dictionary<string, float> volumeInfo){
        if(!CompareDict(objectVolumeDictForPlayerB, volumeInfo)){
            Debug.Log("called set volume B");

            foreach (string key in volumeInfo.Keys) {
                objectVolumeDictForPlayerB[key] = volumeInfo[key];
            }
        }
    }
}
