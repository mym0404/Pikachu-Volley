using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class 게임진행 : MonoBehaviour{
    public Image 스코어업데이트이미지;
    private int MasterScore = 0;
    private int ClientScore = 0;
    public Text 마스터점수텍스트;
    public Text 클라이언트점수텍스트;
    private float alphaValue = 0;
    //피카츄와 공
    private GameObject 내피카츄;
    private GameObject 공;
    //바닥
    public GameObject 바닥;
    //포톤뷰
    private PhotonView pv;
    //게임결과창
    public GameObject 게임결과창;
    //사운드
    public AudioClip 점수내는소리;
    public AudioClip 승리소리;
    public AudioClip 패배소리;
    //게임이 끝났는지 여부
    [HideInInspector]
    public static bool isGameEnd = false;
    //승리 패배 이미지
    public Sprite 승리이미지;
    public Sprite 패배이미지;
    //승리,패배 이펙트
    public GameObject 승리이펙트;
    public GameObject 패배이펙트;
    //피카츄 스프라이트
    public Sprite IdleImage;
   //터치카운트
    public Text masterTouchText;
    public Text clientTouchText;

   


    void Awake() {
        
        pv = GetComponent<PhotonView>();
        StartCoroutine(중도퇴장감지());
    }


    IEnumerator 중도퇴장감지() {
        while (true) {
            if (PhotonNetwork.playerList.Length == 1) {//중도퇴장시
                GameEnd(3);
                break;
            }
            //PhotonNetwork.RPC(pv , "GameEnd" , PhotonTargets.AllViaServer , false , 3);
            yield return new WaitForSeconds(1.0f);
        }
    }

    void GameScoreUpdate(bool isMasterScore) {
        if (isGameEnd == true)
            return;
        Debug.Log("GameScoreUpdate 함수 호출");
        object[ ] param = new object[3];

        if (isMasterScore) {//마스터의 득점
            MasterScore++;
            if (MasterScore == GameMgr.endScore) { //게임 끝 로직
                PhotonNetwork.RPC(pv , "GameEnd" , PhotonTargets.AllViaServer , false , 1);

                
                return;
            }
            param[0] = true;
            param[1] = MasterScore;
            param[2] = ClientScore;

        } else {//클라이언트의 득점
            ClientScore++;
            if (ClientScore == GameMgr.endScore) {//게임 끝 로직
                PhotonNetwork.RPC(pv , "GameEnd" , PhotonTargets.AllViaServer , false , 2);
              
                return;
            }
            param[0] = false;
            param[1] = MasterScore;
            param[2] = ClientScore;
        }
        PhotonNetwork.RPC(pv , "ImageChange" , PhotonTargets.AllViaServer , false , param);
    }
     
    [PunRPC]
    void ImageChange(object[ ] param) {
        if(PhotonNetwork.isMasterClient) 
        Debug.Log("ImageChange함수 호출");
        GameMgr.PlaySfx(점수내는소리 , false);
        StartCoroutine(ImageChangeCoroutine(param));
    }


    IEnumerator ImageChangeCoroutine(object[ ] param) {
        bool isMasterScoreUp = (bool)param[0];
        int MasterScore = (int)param[1];
        int ClientScore = (int)param[2];
        // Debug.Log("ImageChangeCoroutine 함수 호출, 마스터의 득점여부 : "+isMasterScoreUp.ToString()+
        //    "마스터의 점수 = "+MasterScore.ToString()+" 클라이언트의 점수 = "+ClientScore.ToString());

        //공을 멈춤
        GameObject.FindGameObjectWithTag("BALL").GetComponent<Rigidbody>().isKinematic = true;
        //피카츄를 못움직이게함
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            obj.GetComponent<피카츄이동>().enabled = false;
        }

        /*
        while (true) { //페이드아웃
            yield return new WaitForSeconds(0.05f);
            alphaValue += 0.1f;
            스코어업데이트이미지.color = new Color(0 , 0 , 0 , alphaValue);
            if (alphaValue >= 1.0f) {
                alphaValue = 1.0f;
                yield return new WaitForSeconds(1.5f);
                break;
            }//페이드아웃 끝
        }
        */

        //이미지 내려오게함
        float ySpeed = -15;
        while (true) {
            yield return new WaitForSeconds(0.01f);

            스코어업데이트이미지.rectTransform.Translate(0 , ySpeed , 0);
            if (스코어업데이트이미지.rectTransform.position.y <= 0) {
                break;
            }
        }
        


        //터치카운트를 초기화시킴
        masterTouchText.text = "15";
        clientTouchText.text = "15";
        피카츄충돌.isMasterLastTouch = 3; 


        //공의 각도를 원래대로 되돌림
        if (GameObject.FindGameObjectWithTag("BALL") != null)
            foreach (Transform tr in GameObject.FindGameObjectWithTag("BALL").GetComponentsInChildren<Transform>())
                tr.rotation = Quaternion.identity;
    
        //필드에 피카츄와 공을 지움
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            foreach (SpriteRenderer sr in obj.GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = false;
        }
        if (GameObject.FindGameObjectWithTag("BALL") != null) {
            foreach (SpriteRenderer sr in GameObject.FindGameObjectWithTag("BALL").GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = false;

            if (isMasterScoreUp == true)
                GameObject.FindGameObjectWithTag("BALL").transform.position = new Vector3(-4 , 4 , 0);
            else
                GameObject.FindGameObjectWithTag("BALL").transform.position = new Vector3(4 , 4 , 0);
        }
        마스터점수텍스트.text = MasterScore.ToString();
        클라이언트점수텍스트.text = ClientScore.ToString();


        /*
        while (true) { //페이드인
            yield return new WaitForSeconds(0.05f);
            alphaValue -= 0.1f;
            스코어업데이트이미지.color = new Color(0 , 0 , 0 , alphaValue);
            if (alphaValue <= 0.0f) {
                alphaValue = 0.0f;
                yield return new WaitForSeconds(1.5f);
                break;
            }
        }
        */

        yield return new WaitForSeconds(2.5f);
         //이미지 올라오게함
        ySpeed = 20;
        while (true) {
            yield return new WaitForSeconds(0.01f);

            스코어업데이트이미지.rectTransform.Translate(0 , ySpeed , 0);
            if (스코어업데이트이미지.rectTransform.position.y >= 800) {
                break;
            }
        }

        //필드에 피카츄와 공을 생성
        yield return new WaitForSeconds(1.0f);
        //피카츄의 위치를 재설정함
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            if (obj.transform.position.x < 0) //마스터클라이언트
                obj.transform.position = new Vector3(-4 , -2 , 0);
            else
                obj.transform.position = new Vector3(4 , -2 , 0);
        }
        //피카츄를 다시 그림
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            foreach (SpriteRenderer sr in obj.GetComponentsInChildren<SpriteRenderer>()) {
                sr.enabled = true;
            }
        }
        
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            obj.GetComponent<피카츄스프라이트>().enabled = false;
            obj.GetComponent<SpriteRenderer>().sprite = IdleImage;
        }
        
       
        yield return new WaitForSeconds(1.0f);
        if (GameObject.FindGameObjectWithTag("BALL") != null) {
            //공을 다시 그림
            foreach (SpriteRenderer sr in GameObject.FindGameObjectWithTag("BALL").GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = true;
            //피카츄가 다시 이동할 수 있게함
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            if (obj.transform.position.x < 0)//마스터클라이언트의 피카츄이고
                if (PhotonNetwork.isMasterClient)//내가마스터클라이언트라면
                    obj.GetComponent<피카츄이동>().enabled = true;
            if (obj.transform.position.x > 0)//클라이언트의 피카츄이고
                if (PhotonNetwork.isMasterClient == false)//내가 마스터가 아니라면
                    obj.GetComponent<피카츄이동>().enabled = true;
                
        }

            //공이 중력의 영향을 받게함
            GameObject.FindGameObjectWithTag("BALL").GetComponent<Rigidbody>().isKinematic = false;

            //피카츄의 스프라이트를 다시 보이게함
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
                obj.GetComponent<피카츄스프라이트>().enabled = true; 
            }
        }
    }




    //게임 끝 함수
    [PunRPC]
    void GameEnd(int HowGameEnd) { // 1 = 마스터 승리, 2 = 클라이언트 승리, 3 = 중도퇴장, 남은사람 승리
        isGameEnd = true;

        Room myRoom = PhotonNetwork.room;
        myRoom.IsOpen = true;

        //공 없앰
        if(GameObject.FindGameObjectWithTag("BALL")!=null)
        GameObject.FindGameObjectWithTag("BALL").SetActive(false);                        
     
        //필드에 피카츄지움
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            foreach (SpriteRenderer sr in obj.GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = false;
        }

        //점수업데이트

        int 현재마스터점수 = int.Parse(마스터점수텍스트.text);
        int 현재클라이언트점수 = int.Parse(클라이언트점수텍스트.text);
        if (HowGameEnd == 1) {//마스터의 승리일시
            마스터점수텍스트.text = (현재마스터점수 + 1).ToString();
        } else if(HowGameEnd==2) { //클라이언트의 승리일시
            클라이언트점수텍스트.text = (현재클라이언트점수 + 1).ToString();
        }
     

        //승자와 패자 피카츄이미지를 바꿈
        이미지바꾸기(HowGameEnd);

        
        
        게임결과창.SetActive(true); //결과창 소환!


        switch (HowGameEnd) {
            case 1://마스터의 승리
                if (PhotonNetwork.isMasterClient) {
                    승리이벤트발생();
                } else {
                    패배이벤트발생();
                }
                break;
            case 2://클라이언트의 승리
                if (PhotonNetwork.isMasterClient) {
                    패배이벤트발생();
                } else {
                    승리이벤트발생();
                }
                break;
            case 3://중도퇴장
                승리이벤트발생();
                break;
        }



        //연승 카운트 올림

        //마스터의 연승 올림
        if (HowGameEnd == 1) {
            int 연승 = (int)PhotonNetwork.room.CustomProperties["WinCount"];
            연승++;
            PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){
                {"WinCount",연승 } });
        } else { //연승 끝
            PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable(){
                {"WinCount" ,0 } });
        }

        StartCoroutine(LoadRoomScene());
    }

  
    void 이미지바꾸기(int 게임결과) {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PLAYER")) {
            /*
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            
            if (게임결과 == 1) {//마스터가 이기고
                if (sr.gameObject.transform.position.x < 0) {//마스터라면
                    sr.sprite = 승리이미지;
                    Debug.Log("마스터의 승리 이미지 나옴");
                } else {
                    sr.sprite = 패배이미지;
                }
            } else if (게임결과 == 2) {//클라이언트가 이기고
                if (sr.gameObject.transform.position.x < 0) {//마스터라면
                    sr.sprite = 패배이미지;
                } else {
                    sr.sprite = 승리이미지;
                }
            } else { //탈주
                sr.sprite = 승리이미지;

            }
            */
            obj.GetComponent<피카츄이동>().enabled = false;
        }
    }

    void 승리이벤트발생() {
        게임결과창.GetComponentInChildren<Text>().text = "【VICTORY】";
        GameMgr.PlaySfx(승리소리 , false);
        StartCoroutine(승리이펙트코루틴());
    }
    IEnumerator 승리이펙트코루틴() {
        while (true) {
            yield return new WaitForSeconds(0.2f);
            Instantiate(승리이펙트 , new Vector2(Random.Range(-8.0f , 8.0f) , Random.Range(-8.0f , 8.0f)) , 
                Quaternion.identity);
        }
    }
    void 패배이벤트발생() {
        게임결과창.GetComponentInChildren<Text>().text = "【LOSE】";
        GameMgr.PlaySfx(패배소리 , false);
        StartCoroutine(패배이펙트코루틴());
    }
     IEnumerator 패배이펙트코루틴() {
        while (true) {
            yield return new WaitForSeconds(0.2f);
            Instantiate(패배이펙트 , new Vector2(Random.Range(-8.0f , 8.0f) , Random.Range(-8.0f , 8.0f)) , 
                Quaternion.identity);
        }
    }

    IEnumerator LoadRoomScene() {

        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.DestroyAll();


        PhotonNetwork.isMessageQueueRunning = false;
        yield return new WaitForSeconds(7.0f);
        //PhotonNetwork.LoadLevel("scRoom");
        yield return SceneManager.LoadSceneAsync("scRoom");
    }

   

}
