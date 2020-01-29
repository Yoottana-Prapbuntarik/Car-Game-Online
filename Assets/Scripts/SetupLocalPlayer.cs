using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour
{
    public Text NamePrefab;
    private Text NameLabel;
    string textboxName = "";
    string color = "";
    public Transform namePosition;
    [SyncVar(hook = "OnChangeName")]
     string pName = "Player";
    [SyncVar(hook = "OnChangeColor")]
     string pColor = "ffffff";
        
    void OnChangeName(string n)
    {
        pName = n;
        NameLabel.text = pName;
    }
    void OnChangeColor(string n)
    {
        pColor = n;
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in rends)
        {
            if (r.gameObject.name == "BODY")
            {
                r.material.SetColor("_Color", ColorFromHex(pColor));
            }
        }
    }
    Color ColorFromHex(string hex)
    {
        hex = hex.Replace("0x", "");
        hex = hex.Replace("#", "");
        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            textboxName = GUI.TextField(new Rect(25, 15, 100, 25), textboxName);
            if (GUI.Button(new Rect(130, 15, 35, 35), "Set"))
            {
                CmdChangeName(textboxName);
            }
            color = GUI.TextField(new Rect(200, 50, 100, 25), color);
            if (GUI.Button(new Rect(300, 50, 35, 35), "Set"))
            {
                CmdOnChangeColor(color);
            }
        }
    }
    [Command]
    public void CmdChangeName(string name)
    {
        pName = name;
        NameLabel.text = pName;
    }
    [Command]
    public void CmdOnChangeColor(string newcolor)
    {
        pColor = newcolor;
    }
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
        Vector3 nameLabelPos =
            Camera.main.WorldToScreenPoint(namePosition.position);
        NameLabel.transform.position = nameLabelPos;

    }
}
