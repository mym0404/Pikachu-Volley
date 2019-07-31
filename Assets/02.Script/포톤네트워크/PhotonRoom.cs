using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PhotonRoom : MonoBehaviour {
    public Button startButton;
    public Text MasterInfo;
    public Text NonMasterInfo;
    public Text RoomNameInfo;
    private float alphaValue = 0;
    private PhotonView pv;
    public AudioClip roomBGM;
    public Text 연승텍스트;
    //로딩관련 변수
    public GameObject 게임로딩패널;
    public Image loading;
    public Text 로딩퍼센트;

    //스타트버튼 sfx
    public AudioClip buttonDown;
    void Awake() {
        PhotonNetwork.isMessageQueueRunning = true;
        pv = GetComponent<PhotonView>();
        RoomNameInfo.text = PhotonNetwork.room.Name;
        //BGM재생
        GameMgr.PlaySfx(roomBGM,true);
        StartCoroutine(GameMgr.PlayBGMAgain(roomBGM));

    }

    private void Update() {
        //사람이 모두 차고 자신이 방장일 때
        
        if (PhotonNetwork.playerList.Length == 2 && PhotonNetwork.isMasterClient==true) {
            
            StartCoroutine(스타트버튼보이기());
        } else {
            
            StartCoroutine(스타트버튼안보이기());
        }
        foreach (PhotonPlayer player in PhotonNetwork.playerList) {
            if (player.IsMasterClient) {
                MasterInfo.text = player.NickName;
            } else {
                NonMasterInfo.text = player.NickName;
            }
        }//혼자 남았을 때
        if (PhotonNetwork.playerList.Length == 1) {
            //PhotonNetwork.SetMasterClient(PhotonNetwork.player);
            NonMasterInfo.text = "";
        }

        StartCoroutine(연승텍스트업데이트());
    }

    IEnumerator 연승텍스트업데이트() {
        while (true) {
            //연승
            연승텍스트.text = "<color=red>"+PhotonNetwork.room.CustomProperties["WinCount"].ToString()
                +"</color>"+" Win";
            yield return new WaitForSeconds(2.0f);
            
        }
    }

    IEnumerator 스타트버튼보이기() {
        //float alphaValue=0f;
        float upAndDown = 0.01f;
        Text text = startButton.gameObject.GetComponent<Text>();
        while (alphaValue<=1.0f) {
            yield return new WaitForSeconds(0.01f);
            alphaValue += upAndDown;
            text.color = new Color(1.0f , 1.0f , 1.0f , alphaValue);
            
        }
        startButton.GetComponent<Button>().enabled = true;
    }
     IEnumerator 스타트버튼안보이기() {
     //float alphaValue=1.0f;
        float upAndDown = -0.01f;
        Text text = startButton.gameObject.GetComponent<Text>();
        while (alphaValue>=0.0f) {
            yield return new WaitForSeconds(0.01f);
            alphaValue += upAndDown;
            text.color = new Color(1.0f , 1.0f , 1.0f , alphaValue);
            
        }
        startButton.GetComponent<Button>().enabled = false;
    }
    //나가기 버튼
    public void OnClickReturnButton() {
        Debug.Log("나가기 버튼 클릭");
        PhotonNetwork.LeaveRoom();
        StartCoroutine(씬전환());
    }
    IEnumerator 씬전환() {
        PhotonNetwork.isMessageQueueRunning = false;
        yield return SceneManager.LoadSceneAsync("scLobby");
    }
    //
    
    public void OnClickStartButton() {
        //자신의 RoomInfo 의 isOpen 프로퍼티를 false로 바꿈
        Room myRoom = PhotonNetwork.room;
        myRoom.IsOpen = false;

        Debug.Log("스타트버튼호출");
        startButton.GetComponent<Button>().interactable = false;
        startButton.GetComponent<Transform>().Translate(Vector3.down * 3f);
        startButton.GetComponent<Text>().color = Color.gray;
        GameMgr.PlaySfx(buttonDown,false);
        PhotonNetwork.RPC(pv , "로딩화면시작함수" , PhotonTargets.AllViaServer , false , null);
        //PhotonNetwork.RPC(pv , "LoadGameFieldScene" , PhotonTargets.AllViaServer , false , null);

    }
   


    [PunRPC]
    void 로딩화면시작함수() {
        StartCoroutine(로딩화면코루틴());
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
            yield return new WaitForSeconds(0.01f); // 나중에 변경
            loading.fillAmount = fillAmount;

            if (fillAmount >= 1.0f) {
                로딩퍼센트.text = "100%";
                break;
            } else {
                로딩퍼센트.text = Mathf.RoundToInt(loading.fillAmount * 100.0f).ToString()+"%"; 
            }
        }
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.RPC(pv , "LoadGameFieldScene" , PhotonTargets.AllViaServer , false , null);
       // LoadGameFieldScene();
        //PhotonNetwork.RPC(pv , "LoadGameFieldScene" , PhotonTargets.AllViaServer , false , null);
    }

    [PunRPC]
    private void LoadGameFieldScene() {
        Debug.Log("LoadGameFieldScene() 호출");
        StartCoroutine(GoField());
    }
    IEnumerator GoField() {
        PhotonNetwork.isMessageQueueRunning = false;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("scField");
    }


}
