using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;




public class PhotonInit : MonoBehaviour {
    /*
    public InputField NickName;
    public InputField RoomName;
    public GameObject scrollContents;
    public GameObject roomItem;
    
    */

    //버튼 게임오브젝트
    public GameObject 방생성;
    

    public GameObject 옵션;
    
    //사운드
    public AudioClip lobbyBGM;
    public AudioClip buttonDown;
    //게임 버전
    private string PhotonGameVersion = "1.0.0";
	
    public Text 접속자수;
    //포톤매니저
    public GameObject 포톤매니저;

    //로딩관련변수
    public GameObject 게임로딩패널;
    public Image loading;
    public Text 로딩퍼센트;
        //팝업창
    public GameObject 팝업창;
    void Awake() {
        //BGM재생
        GameMgr.PlaySfx(lobbyBGM,true);
        StartCoroutine(GameMgr.PlayBGMAgain(lobbyBGM));

        /*
        //버전체크로직
        bool isServerOn = false;
        string lastSavedVersion = Client.GetGameVersion(out isServerOn);
        if (isServerOn == false) {
            GameMgr.PopUp(팝업창,"Server Offline Sorry.");
            return;
        }
        Debug.Log("lastSavedVersion = " + lastSavedVersion);
       
        TrackedBundleVersionInfo currentRunningVersion = new TrackedBundleVersion().current;
        //버전 로그띄우기
        Debug.Log("최신 버전 = "+lastSavedVersion);
        Debug.Log(lastSavedVersion.Length.ToString());
        Debug.Log("현재 버전 = "+currentRunningVersion.version);
        Debug.Log(currentRunningVersion.version.Length.ToString());

        //버전 비교하고 팝업창 띄우기
        if (currentRunningVersion.version.CompareTo(lastSavedVersion)!=0) {
            GameMgr.PopUp(팝업창,"You have to update game to lastest version for access to Photon server,\n expected lastSavedVersion = "+
                lastSavedVersion);
            return;
        }
        
        */

        //로비에 들어가기
        PhotonNetwork.isMessageQueueRunning = true;
        if (PhotonNetwork.connected == false) {
            Debug.Log(PhotonGameVersion.ToString() + " 포톤네트워크 연결시도");
            PhotonNetwork.ConnectUsingSettings(PhotonGameVersion);

            Debug.Log(PhotonGameVersion + "의 포톤버전으로 포톤네트워크 접속 완료!");
            Debug.Log("포톤네트워크 게임 버전 = " + PhotonNetwork.gameVersion);
            Debug.Log(UnityEngine.Application.version);
        } else {
            포톤매니저.SendMessage("로비항목들켬");

        }
        
	}
    //GUI 상태 나타내기
    /*
    private void OnGUI() {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
    */
    void Update() {
        접속자수.text=PhotonNetwork.countOfPlayers.ToString();
    }
 
     
    
   

   
    IEnumerator 로딩화면코루틴() {
        //BGM끄기
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SFX")) {
            Destroy(obj);
        }


        게임로딩패널.SetActive(true); //우선 로딩화면을 띄운다
        float fillAmount = 0.0f;
        while (true) {
            fillAmount += 0.01f;
            yield return new WaitForSeconds(0.05f);
            loading.fillAmount = fillAmount;

            if (fillAmount >= 1.0f) {
                로딩퍼센트.text = "100%";
                break;
            } else {
                로딩퍼센트.text = Mathf.RoundToInt(loading.fillAmount * 100.0f).ToString()+"%"; 
            }
        }
        SceneManager.LoadScene("scSolo");
    }


    //옵션 버튼을 눌렀을 때
    public void OnClickOptionsButton() {
        옵션.GetComponent<Button>().enabled = false;
        옵션.GetComponent<Transform>().Translate(Vector3.down * 3f);
        옵션.GetComponent<Text>().color = Color.gray;
        방생성.GetComponent<Button>().enabled = false;
        GameMgr.PlaySfx(buttonDown,false);
        StartCoroutine(옵션코루틴());
    }
    IEnumerator 옵션코루틴() {
        yield return new WaitForSeconds(1.5f);
        방생성.GetComponent<Button>().enabled = true;
        옵션.GetComponent<Button>().enabled = true;
        옵션.GetComponent<Transform>().Translate(Vector3.up * 3f);
        옵션.GetComponent<Text>().color = Color.white;
    }
    //룸 정보가 업데이트 되었을 때
    /*
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
        혼자놀기.GetComponent<Button>().enabled = false;
        StartCoroutine(CreateGameRoom());
        
    }
    IEnumerator CreateGameRoom() {
        yield return new WaitForSeconds(0.8f);
        옵션.GetComponent<Button>().enabled = true;
        혼자놀기.GetComponent<Button>().enabled = true;
        방생성.GetComponent<Button>().enabled = true;
        방생성.GetComponent<Transform>().Translate(Vector3.up * 3f);
        방생성.GetComponent<Text>().color = Color.white;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        if (string.IsNullOrEmpty(RoomName.text))
            RoomName.text = "ROOM_" + Random.Range(0 , 999).ToString("000");
        PhotonNetwork.playerName = NickName.text;
        PhotonNetwork.CreateRoom(RoomName.text, roomOptions, null);
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
    }
    */
}
