using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PhotonManager : MonoBehaviour {
	public InputField NickName;
    public InputField RoomName;
    public GameObject scrollContents;
    public GameObject roomItem;

    //사운드
    //public AudioClip lobbyBGM;
    public AudioClip buttonDown;

    //버튼 게임오브젝트
    public GameObject 방생성;
    public GameObject 옵션;

    //private static PhotonManager instance;
    //public static PhotonManager Instance{
    //    get { return Instance; }
    //}

    //private void Awake() {
    //    if(Instance!=null){
    //        DestroyImmediate(this.gameObject);
    //        return;
    //    }
    //    instance = this;
    //    DontDestroyOnLoad(this.gameObject);
    //}

    void Start () {

	}
	
	void Update () {
		
	}
    //로비에 정상적으로 접속
    void OnJoinedLobby(){
        Debug.Log("로비에 접속 완료!");
        //SceneManager.LoadScene("scLobby");
        로비항목들켬();
    }
     void 로비항목들켬() {

        // PhotonNetwork.isMessageQueueRunning = false;
        Debug.Log("로비접속 코루틴 실행");
        //yield return SceneManager.LoadSceneAsync("scLobby");
        GameObject.Find("방목록 패널").GetComponent<Image>().enabled = true;

        방생성.GetComponent<Button>().enabled = true;
        방생성.GetComponent<Text>().enabled = true;
        옵션.GetComponent<Button>().enabled = true;
        옵션.GetComponent<Text>().enabled = true;

        NickName.text= GetUserID();
        RoomName.text = GetRoomID();
        
    }

    //유저 아이디 랜덤 반환
    string GetUserID() {
        return "USER_" + Random.Range(0 , 999).ToString("000");
    }
    //방 아이디 랜덤 반환
    string GetRoomID() {
        return "ROOM_ " + Random.Range(0 , 999).ToString("000");
    }

    //포톤서버로부터 연결이 끊겼을 때
    void OnDisconnectedFromPhoton() {
        Debug.Log("연결 끊김");
    }
     //룸 정보가 업데이트 되었을 때
    
    void OnReceivedRoomListUpdate() {
        foreach (GameObject delObj in GameObject.FindGameObjectsWithTag("ROOM_ITEM")) {
           Destroy(delObj);
        }
        int rowCount = 0;
        //scrollContents.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        
        foreach (RoomInfo _room in PhotonNetwork.GetRoomList()) {
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContents.transform , false);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayers = _room.MaxPlayers;
            roomData.WinCount = (int)_room.CustomProperties["WinCount"];
            //만약 그 방이 게임중이라면
            if (_room.IsOpen == false) {
                roomData.isGaming = true;
            }

            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0 , 50);
            roomData.DispRoomData();

            roomData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate {
                OnClickRoomButton(roomData.roomName);});
        }
        
    }
    
    //룸 목록을 클릭해서 방에 들어갈 때
    public void OnClickRoomButton(string roomName) {
        PhotonNetwork.playerName = NickName.text;
        PhotonNetwork.playerName = NickName.text;
        PhotonNetwork.JoinRoom(roomName);
    }
     //방 생성 버튼을 눌렀을 때
    public void OnClickCreateRoomButton() {
        방생성.GetComponent<Button>().enabled = false;
        방생성.GetComponent<Transform>().Translate(Vector3.down * 3f);
        방생성.GetComponent<Text>().color = Color.gray;
        GameMgr.PlaySfx(buttonDown,false);
        옵션.GetComponent<Button>().enabled = false;
        StartCoroutine(CreateGameRoom());
        
    }
    IEnumerator CreateGameRoom() {
        yield return new WaitForSeconds(1.5f);
        옵션.GetComponent<Button>().enabled = true;
        방생성.GetComponent<Button>().enabled = true;
        방생성.GetComponent<Transform>().Translate(Vector3.up * 3f);
        방생성.GetComponent<Text>().color = Color.white;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        if (string.IsNullOrEmpty(RoomName.text))
            RoomName.text = "ROOM_" + Random.Range(0 , 999).ToString("000");
        //맵, 연승 설정
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable(){{
                "Map",1 },{"WinCount",0 } };
        roomOptions.CustomRoomProperties = roomProperties;
        roomOptions.CustomRoomPropertiesForLobby = new string[ ] { "Map" , "WinCount" };
        

        PhotonNetwork.playerName = NickName.text;
        PhotonNetwork.CreateRoom(RoomName.text, roomOptions, null);
    }
     //방 만들기 실패
    private void OnPhotonCreateRoomFailed()
     {
         Debug.Log("Can't join random room!");
     }

    //방에 들어갔을 때
    void OnJoinedRoom() {
        Debug.Log("방에 성공적 입장");
        StartCoroutine(LoadRoomScene());
    }
    
    IEnumerator LoadRoomScene() {
        PhotonNetwork.isMessageQueueRunning = false;
        yield return SceneManager.LoadSceneAsync("scRoom");
    }
    //방에 들어갈 수 없을 때
    void OnPhotonRandomJoinFailed() {
        Debug.Log("랜덤 조인 패일");
    }
    //룸 생성 실패시
    void OnPhotonCreateRoomFailed(object[ ] error) {
        Debug.Log(error[0].ToString()); //오류 코드
        Debug.Log(error[1].ToString()); //오류 메세지

        GameMgr.PopUp(GameObject.Find("팝업창"),"방 이름이 있습니다. Error Code = "+error[0].ToString());
    }

    //포톤 네트워크 접속 실패시
    void OnFailedToConnectToPhoton(DisconnectCause cause) {
        Debug.Log("접속 실패했습니다 이유 = "+cause.ToString());
        GameMgr.PopUp(GameObject.Find("팝업창"),"네트워크 연결 실패!");
    }

    //마스터클라이언트 변경시
    void OnMasterClientSwitched(PhotonPlayer newMasterClient) {
        PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){
            {"WinCount",0 } });
    }	

}
