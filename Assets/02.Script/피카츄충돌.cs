using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class 피카츄충돌 : MonoBehaviour {


    private bool isSwinged = false;

    private PhotonView pv;

    private Transform tr;
     //볼에 부딪혔을 때 줄 파워
    public float touchPower = 100.0f;
    //스윙파워
    public float swingPower = 20.0f;
    //피카츄이동
    피카츄이동 이동;
    //터치카운트
    private Text masterTouchText;
    private Text clientTouchText;
    //게임진행감독
    private GameObject 게임진행감독;
    //누가 공을 마지막으로 쳤는지
    public static int isMasterLastTouch=3;


	void Awake () {
        tr = GetComponent<Transform>();
        이동 = GetComponent<피카츄이동>();
        pv = GetComponent<PhotonView>();

        masterTouchText = GameObject.FindGameObjectWithTag("마스터터치텍스트").GetComponent<Text>();
        clientTouchText=GameObject.FindGameObjectWithTag("클라이언트터치텍스트").GetComponent<Text>();
        게임진행감독 = GameObject.Find("게임진행감독");
	}
	
	
    //트리거
    private void OnTriggerEnter(Collider other) {
        //만약 공과 트리거 발생을 한다면

        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("BALL"))) {
            int swingMode = 1; //1 = 일반스윙, 2 = 전진스윙, 3 = 위스윙, 4 = 아래스윙

            //공의 Rigidbody 컴포넌트를 가져온다.
            Rigidbody ballRbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 beforeVelocity = ballRbody.velocity;
            Vector3 powerDirection = other.gameObject.transform.position - tr.position;
            ballRbody.velocity = Vector3.zero;
            if (이동.Swinging == true) { //스윙

                //스윙모드 결정
                if (ButtonManager.instance.h != 0) {
                    swingMode = 2;
                } else if (ButtonManager.instance.down == true) {
                    swingMode = 4;
                } else if (ButtonManager.instance.up == true) {
                    swingMode = 3;
                } else {
                    swingMode = 1;
                }
            

                //스윙 이펙트
                other.gameObject.SendMessage("Swing" , null , SendMessageOptions.DontRequireReceiver);
                //스윙 관련 로직들
                isSwinged = true;
                StartCoroutine(SwingTimeEnd());


                //스윙 로직
                if (PhotonNetwork.isMasterClient == false) { //클라이언트일때

                    PhotonNetwork.RPC(pv , "ClientSwing" , PhotonTargets.MasterClient , false , swingMode);
                } else {
                    switch (swingMode) {
                        case 1: //일반스윙
                            ballRbody.velocity = Vector3.right * swingPower * (0.8f);
                            break;
                        case 2: //전진스윙
                            ballRbody.velocity = Vector3.right * swingPower * 1.2f;
                            break;
                        case 3://업스윙
                            ballRbody.velocity = Vector3.right * swingPower * (0.8f) + Vector3.up * (swingPower / 2);
                            break;
                        case 4: //다운스윙
                            ballRbody.velocity = Vector3.right * swingPower * (0.7f) + Vector3.down * (swingPower * (0.9f));
                            break;
                    }
                }
            } else { //일반 터치
                ballRbody.velocity = new Vector3(powerDirection.x * 5.0f , touchPower , 0);
            }


            
            //터치 횟수 1회 차감
            if (pv.isMine) {
                if (PhotonNetwork.isMasterClient) {
                    PhotonNetwork.RPC(pv , "ChangeTouchCount" , PhotonTargets.AllViaServer , false , true);
                } else {
                    PhotonNetwork.RPC(pv , "ChangeTouchCount" , PhotonTargets.AllViaServer , false , false);
                }
            }
        }
    }
    IEnumerator SwingTimeEnd() {
        yield return new WaitForSeconds(0.3f);
        isSwinged = false;
    }

    
    [PunRPC]
    void ClientSwing(int swingMode) {

        Rigidbody ballRbody = GameObject.FindGameObjectWithTag("BALL").GetComponent<Rigidbody>();

        switch (swingMode) {
           case 1: //일반스윙
                            ballRbody.velocity = Vector3.left * swingPower *(0.7f);
                            break;
                        case 2: //전진스윙
                            ballRbody.velocity = Vector3.left * swingPower;
                            break;
                        case 3://업스윙
                            ballRbody.velocity = Vector3.left * swingPower*(0.7f) + Vector3.up*(swingPower/2);
                            break;
                        case 4: //다운스윙
                            ballRbody.velocity = Vector3.left * swingPower*(0.5f) + Vector3.down *(swingPower*(0.7f));
                            break;
        }
    }
    
    //피카츄와 닿았을때 공 띄우게하기
    private void OnTriggerStay(Collider other) {
        //만약 공과 트리거 발생을 한다면
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("BALL"))) {
            //공의 Rigidbody 컴포넌트를 가져온다.
            if (isSwinged == false) {
                Rigidbody ballRbody = other.gameObject.GetComponent<Rigidbody>();
                ballRbody.velocity += Vector3.up * 0.2f;
            }
        }
    }

    //터치횟수 RPC함수
    [PunRPC]
    void ChangeTouchCount(bool isMasterClient) {
        

       
        if ((isMasterClient && isMasterLastTouch == 2) || (!isMasterClient && isMasterLastTouch == 1)) {
            masterTouchText.text = "15";
            clientTouchText.text = "15";
        }


        int touchCount;
        if (isMasterClient) { //마스터 클라이언트 횟수 차감
            touchCount = int.Parse(masterTouchText.text);
            masterTouchText.text = (--touchCount).ToString();
        } else {
            touchCount = int.Parse(clientTouchText.text);
            clientTouchText.text = (--touchCount).ToString();
        }
        if (touchCount == 0) {//터치카운트를 모두 소진하면

                게임진행감독.SendMessage("GameScoreUpdate" , !isMasterClient);
        }

         if (isMasterClient) 
            isMasterLastTouch = 1;
         else
            isMasterLastTouch = 2;

    }
    
}
