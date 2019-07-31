using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour {
	[HideInInspector]
    public string roomName = "";
    [HideInInspector]
    public int connectPlayer = 0;
    [HideInInspector]
    public int maxPlayers = 0;
    [HideInInspector]
    public bool isGaming = false;
    [HideInInspector]
    public int WinCount=0;


    public Text textRoomName;
    public Text textConnectInfo;
    public Text textWinCount;
    //121,178,169
    public void DispRoomData() {
        if(isGaming==true)
            GetComponent<Image>().color = new Color(55f/255f, 76f/255f , 88f/255f);
        textRoomName.text = roomName;
        textConnectInfo.text = "(" + connectPlayer.ToString() + "/" + maxPlayers.ToString() + ")";
        textWinCount.text = WinCount.ToString();
    }
	
}
