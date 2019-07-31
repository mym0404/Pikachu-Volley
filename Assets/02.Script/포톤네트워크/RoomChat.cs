using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomChat : MonoBehaviour {
    private PhotonView pv;
    public Text ChatText;
    public InputField ChatInput;
    public Button SendButton;

	void Awake () {
        pv = GetComponent<PhotonView>();
	}
	
    void OnGUI() {
     if(ChatInput.isFocused  && Input.GetKey(KeyCode.Return)) {
            OnClickSendButton();
     }
 }
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        string sendMessage = "[<color=yellow>" + newPlayer.NickName + "</color>] 님이 입장하셨습니다.";
        PhotonNetwork.RPC(pv , "UpdateChatText" , PhotonTargets.AllViaServer , false , sendMessage);
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        string sendMessage = "[<color=yellow>" + otherPlayer.NickName + "</color>] 님이 퇴장하셨습니다.";
        PhotonNetwork.RPC(pv , "UpdateChatText" , PhotonTargets.AllViaServer , false , sendMessage);
    }

    public void OnClickSendButton() {
        if (string.IsNullOrEmpty(ChatInput.text))
            return;

        string sendMessage = "[<color=purple>"+PhotonNetwork.playerName+"</color>] : "+string.Copy(ChatInput.text);
        PhotonNetwork.RPC(pv , "UpdateChatText" , PhotonTargets.AllViaServer , false , sendMessage);
        ChatInput.text = "";
    }

    [PunRPC]
    void UpdateChatText(string inputText) {
        ChatText.text += "\n";
        ChatText.text += inputText;
    }
}
