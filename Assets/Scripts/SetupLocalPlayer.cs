using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour
{
    public Text NamePrefab;
    private Text NameLabel;
    public Transform namePosition;
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<PlayerController>().enabled = true;
            CameraFollow360.player = this.gameObject.transform;
        }
        else
        {
            GetComponent<PlayerController>().enabled = false;
        }
        //finding where is canvas
        GameObject canvas = GameObject.FindWithTag("TextCanvas");
        NameLabel = Instantiate(NamePrefab, Vector3.zero, Quaternion.identity) as Text;
        //transform follow canvas 
        NameLabel.transform.SetParent(canvas.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
