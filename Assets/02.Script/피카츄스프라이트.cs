using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 피카츄스프라이트 : MonoBehaviour {
    private SpriteRenderer sr;
    private CharacterController controller;
    private 피카츄이동 이동;
    //이미지들
    public Sprite Idle;public Sprite Jump;public Sprite Swing;public Sprite DiveRight;public Sprite DiveLeft;
    //포톤뷰 변수
    private PhotonView pv;
    //현재의 스프라이트 상태를 나타내는 변수
    private int SpriteState = 0; //0 = idle, 1 = jump, 2 = swing, 3 = right dive, 4 = left dive
    //상대방의 스프라이트 상태를 받아올 변수
    private int SpriteOthersState = 0;
	void Awake () {
        sr = GetComponent<SpriteRenderer>();
        controller = GetComponent<CharacterController>();
        이동 = GetComponent<피카츄이동>();
        pv = GetComponent<PhotonView>();
        if (pv.isMine == false)
            sr.flipX = true;
        if (PhotonNetwork.isMasterClient == false) {
            sr.flipX = true;
            Sprite temp = DiveRight;
            DiveRight = DiveLeft;
            DiveLeft = temp;
        }
       
	}
	
	void Update () {
        if (게임진행.isGameEnd == false) {
            if (pv.isMine) { //내것이라면
                if (이동.Swinging) {
                    sr.sprite = Swing;
                    SpriteState = 2;
                } else if (이동.Divinging == -1) {
                    sr.sprite = DiveLeft;
                    SpriteState = 4;
                } else if (이동.Divinging == 1) {
                    sr.sprite = DiveRight;
                    SpriteState = 3;
                } else if (controller.isGrounded == false) {
                    sr.sprite = Jump;
                    SpriteState = 1;
                } else {
                    sr.sprite = Idle;
                    SpriteState = 0;
                }
            } else { //남의것이라면
                sr.flipX = false;
                if (PhotonNetwork.isMasterClient == true)
                    if (pv.isMine == false)
                        sr.flipX = true;

                switch (SpriteOthersState) {


                    case 0: //Idle
                        sr.sprite = Idle;
                        break;
                    case 1: //Jump
                        sr.sprite = Jump;
                        break;
                    case 2: //Swing
                        sr.sprite = Swing;
                        break;
                    case 3: //Right Dive
                        sr.sprite = DiveLeft;
                        break;
                    case 4: //LEft Dive
                        sr.sprite = DiveRight;
                        break;
                }

            }
        }
	}

    void OnPhotonSerializeView(PhotonStream stream , PhotonMessageInfo info) {
        if (stream.isWriting) { //송신중일때
            stream.SendNext(SpriteState);
        } else { //수신중일때
            SpriteOthersState = (int)stream.ReceiveNext();
        }
    }
}
