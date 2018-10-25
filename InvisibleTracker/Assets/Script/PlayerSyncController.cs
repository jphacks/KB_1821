using UnityEngine;
using System.Collections.Generic;

public class PlayerSyncController : MonoBehaviour
{
    public Dictionary<string, float> objectDistanceDict = new Dictionary<string,float> ();

    private static float volumeMin = 0.0f;
    private static float volumeMax = 1.0f;
    private static float distanceMin = 0.0f;
    private static float distanceMax = 2.5f;

    void Start()
    {
        var childTransform = GameObject.Find("GameMap").GetComponentsInChildren<Transform>();
        foreach (Transform child in childTransform)
        {
            if (child.gameObject.tag == "Object")
            {
                objectDistanceDict.Add(child.name, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (!photonView.isMine)
        // {
            // transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            // transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        // }
        foreach (KeyValuePair<string, float> pair in objectDistanceDict) {
            Debug.Log (pair.Key + " : " + pair.Value);
        }
    }

    void OnCollisionEnter (Collision col ){

    }

    void OnCollisionStay(Collision col){
        if(col.gameObject.tag == "Object"){
            string target_name = col.transform.name;
            float distance = (float) Vector3.Distance(col.transform.position, this.transform.position);
            float lerped_distance = Mathf.InverseLerp (distanceMin, distanceMax, distance);
            float volume = Mathf.Lerp(volumeMin, volumeMax, lerped_distance);

            objectDistanceDict[target_name] = volume;
        }
    }

    void OnCollisionExit(Collision col){
        if(col.gameObject.tag == "Object"){
            string target_name = col.transform.name;
            objectDistanceDict[target_name] = 0;
        }
    }
}