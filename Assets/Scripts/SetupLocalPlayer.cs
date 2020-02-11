using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour
{
    public Text NamePrefab;
    private Text NameLabel;
    public Text Chat;
    int offsetY = 1;
    string textboxName = "";
    string color = "";
    string currentTextChat = "";

    List<string> chatHistory = new List<string>();
    public Transform namePosition;
    [SyncVar(hook = "OnChangeName")]
    string pName = "Player";
    [SyncVar(hook = "OnChangeColor")]
    string pColor = "ffffff";
    [SyncVar(hook = "OnChangeNextMsg")]
    string HistoryText = "";

    void OnChangeNextMsg(string newMsg)
    {
        HistoryText = newMsg;
        chatHistory.Add(HistoryText);
        foreach (string items in chatHistory)
        {
            Chat.text += items;
        }
    }
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
            color = GUI.TextField(new Rect(350, 15, 100, 25), color);
            if (GUI.Button(new Rect(450, 15, 35, 35), "Set"))
            {
                CmdOnChangeColor(color);
            }
            //input
            currentTextChat = GUI.TextField(new Rect(25, 500, 100, 25), currentTextChat);
            if (GUI.Button(new Rect(200, 500, 35, 35), "send"))
            {
                CmdOnChat(currentTextChat);

            }
        }
    }
    [Command]
    public void CmdOnChat(string text)
    {
        HistoryText = text;
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
        GameObject canvasChat = GameObject.FindWithTag("chatCanvas");
        Chat = Instantiate(Chat, new Vector3(300, 20, 0), Quaternion.identity) as Text;
        //transform follow canvas 
        Chat.transform.SetParent(canvasChat.transform);

    }
    public void OnDestroy()
    {
        if (NameLabel != null)
        {
            Destroy(NameLabel.gameObject);
        }
    }

    //After at start get current Name
    public override void OnStartClient()
    {
        //call old coding original 
        base.OnStartClient();
        Invoke("UpdatesState", 1);
    }
    void UpdatesState()
    {
        OnChangeName(pName);
        OnChangeColor(pColor);

    }
    // Update is called once per frame
    void Update()
    {
        /*  Vector3 nameLabelPos =
            Camera.main.WorldToScreenPoint(namePosition.position);
            NameLabel.transform.position = nameLabelPos;
        */
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
                        screenPoint.x < 1 && screenPoint.y > 0 &&
                        screenPoint.y < 1;
        if (onScreen)
        {
            Vector3 nameLabelPos =
            Camera.main.WorldToScreenPoint(namePosition.position);
            NameLabel.transform.position = nameLabelPos;

        }
        else
        {
            NameLabel.transform.position = new Vector3(-1000, -1000, 0);
        }

    }
}
