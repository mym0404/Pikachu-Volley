using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonField : MonoBehaviour {
    private GameObject 내피카츄;
    private GameObject 공;
    private int 센드레이트 = 100;
    public Text 카운트다운텍스트;
    public AudioClip fieldBGM;
    public AudioClip countDownEffect;
    public AudioClip startEffect;
	void Awake () {
        PhotonNetwork.isMessageQueueRunning = true;
        PhotonNetwork.sendRate = 센드레이트;
        PhotonNetwork.sendRateOnSerialize = 센드레이트;

        StartCoroutine(StartGame());
        //피카츄 생성
        게임진행.isGameEnd = false;
       
	}
    //게임 시작
    IEnumerator StartGame() {

        if (PhotonNetwork.isMasterClient) {
            내피카츄 = PhotonNetwork.Instantiate("피카츄" , new Vector3(-4 , -2 , 0) , Quaternion.identity , 0);
            int WhoIsFirst = Random.Range(0 , 2); //0하고 1 중에 나옴
            /*
            if (WhoIsFirst == 0)
                공 = PhotonNetwork.Instantiate("공" , new Vector3(4 , 4 , 0) , Quaternion.identity , 0);
            else
            */
                공 = PhotonNetwork.Instantiate("공" , new Vector3(-4 , 4 , 0) , Quaternion.identity , 0);
        
        } else
            내피카츄 = PhotonNetwork.Instantiate("피카츄" , new Vector3(4 , -2 , 0) , Quaternion.identity , 0);


        StartCoroutine(카운트다운());
        //3초 기다림
        yield return new WaitForSeconds(4.8f);

        //피카츄를 움직이게함
        내피카츄.GetComponent<피카츄이동>().enabled = true;
        내피카츄.GetComponent<피카츄충돌>().enabled = true;


        //공을 움직이게함
        if (PhotonNetwork.playerList.Length == 2) {
            GameObject.FindGameObjectWithTag("BALL").GetComponent<Rigidbody>().isKinematic = false;
            //공을 보이게함
            foreach (SpriteRenderer sr in GameObject.FindGameObjectWithTag("BALL").GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = true;
        }
    }

    //카운트다운 코루틴
    IEnumerator 카운트다운() {
        Debug.Log("카운트다운 함수 호출");
        //3초
        GameMgr.PlaySfx(countDownEffect , false);
        while (true) {
            카운트다운텍스트.fontSize += 3;
            yield return new WaitForSeconds(0.01f);
            if (카운트다운텍스트.fontSize >= 300)
                break;
        }
        while (true) {
            카운트다운텍스트.fontSize -= 3;
            yield return new WaitForSeconds(0.01f);
            if (카운트다운텍스트.fontSize <= 200)
                break;
        }


        //yield return new WaitForSeconds(1.0f);
        카운트다운텍스트.text = "2";
        //2초
        카운트다운텍스트.gameObject.GetComponent<Outline>().effectColor = new Color(0 , 1.0f , 0f , 100f / 255f);
        GameMgr.PlaySfx(countDownEffect , false);
   while (true) {
            카운트다운텍스트.fontSize += 3;
            yield return new WaitForSeconds(0.01f);
            if (카운트다운텍스트.fontSize >= 300)
                break;
        }
        while (true) {
            카운트다운텍스트.fontSize -= 3;
            yield return new WaitForSeconds(0.01f);
            if (카운트다운텍스트.fontSize <= 200)
                break;
        }
       // yield return new WaitForSeconds(1.0f);
        카운트다운텍스트.text = "1";
        //1초
        카운트다운텍스트.gameObject.GetComponent<Outline>().effectColor  = new Color(1.0f , 0f , 0f , 100f / 255f);
        GameMgr.PlaySfx(countDownEffect , false);
        while (true) {
            카운트다운텍스트.fontSize += 3;
            yield return new WaitForSeconds(0.01f);
            if (카운트다운텍스트.fontSize >= 300)
                break;
        }
        while (true) {
            카운트다운텍스트.fontSize -= 3;
            yield return new WaitForSeconds(0.01f);
            if (카운트다운텍스트.fontSize <= 200)
                break;
        }
        //yield return new WaitForSeconds(1.0f);
        //시작
        GameMgr.PlaySfx(startEffect , false);
        카운트다운텍스트.text = "START!";
        yield return new WaitForSeconds(1.0f);
        //텍스트 숨김
        카운트다운텍스트.gameObject.SetActive(false);

        //BGM재생
        GameMgr.PlaySfx(fieldBGM,true);
        StartCoroutine(GameMgr.PlayBGMAgain(fieldBGM));
    }

    

	void Update () {
		
	}
   
    
}
