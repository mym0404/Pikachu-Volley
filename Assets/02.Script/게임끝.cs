using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class 게임끝 : MonoBehaviour {
    //바닥에 닿는 이펙트
    public GameObject 바닥닿는이펙트;
    
    //public Image 화면전환;
   private GameObject 게임감독;
    private bool 점수낼수있다 = true;

    //포톤뷰
    private PhotonView pv;
	void Start () {
        
        게임감독 = GameObject.Find("게임진행감독");
        pv = GetComponent<PhotonView>();
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (PhotonNetwork.isMasterClient == false)
            return;
        //공과 충돌했을 때
        if (other.gameObject.tag == "BALL" && 점수낼수있다==true) {
            점수낼수있다 = false;
            StartCoroutine(점수내는시간코루틴());
            if (PhotonNetwork.isMasterClient)
                PhotonNetwork.RPC(pv , "땅이펙트내기" , PhotonTargets.AllViaServer , false , null);
            if (other.transform.position.x < 0.0f) { //클라이언트의 득점일 때
                게임감독.SendMessage("GameScoreUpdate" , false , SendMessageOptions.DontRequireReceiver);
            } else { //마스터의 득점일 때
                 게임감독.SendMessage("GameScoreUpdate" , true , SendMessageOptions.DontRequireReceiver);
            }
        }
        
    }
    [PunRPC]
    void 땅이펙트내기() {
        
        GameObject 땅이펙트 = Instantiate(바닥닿는이펙트 , GameObject.FindGameObjectWithTag("BALL").transform.position+Vector3.down*0.5f , Quaternion.identity);
        Destroy(땅이펙트 , 5.0f);
    }


    IEnumerator 점수내는시간코루틴() {
        yield return new WaitForSeconds(2.0f);
        점수낼수있다 = true;
    }


   
}
