using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class 피카츄이동 : MonoBehaviour {
  
    
    //사운드
    public AudioClip diveSound;
    public AudioClip jumpSound;
    public AudioClip swingSound;

    private Transform tr;
    private CharacterController controller;
   
    //중력, 점프속도, 이동속도, 다이빙 속도
    public float gravity = 1.0f;
    public float jumpSpeed = 10.0f;
    public float walkSpeed = 5.0f;
    public float diveSpeed = 5.0f;
    public float diveJumpSpeed = 1.0f;
    //이동하는 방향 Vector3
    private Vector3 movDir = Vector3.zero;
    //수직 속도 - 점프 메카니즘에 쓰임
    private float vSpeed;
    //포톤 뷰 컴포넌트
    PhotonView pv = null;
    //지금 스윙 상태인지 나타내는 변수
    [HideInInspector]
    public bool Swinging = false;
    //지금 다이브 상태인지 나타내는 변수
    [HideInInspector]
    public int Divinging = 0;
    //버튼 상태 가져오기
    private ButtonManager bm;
    

    void Awake() {
        pv = GetComponent<PhotonView>();
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        bm = ButtonManager.instance;
    }

    void Update() {
        if (Divinging == 0) {
            PikachuMove();
          //  PikachuSwing();
          //  PikachuDive();
         //   PikachuJump();
        } else {
            Diving(Divinging);
        }

        //피카츄 중력, vSpeed 로직
        movDir.y = vSpeed;
        vSpeed -= gravity * Time.deltaTime;
        //피카츄 이동
        controller.Move(movDir * Time.deltaTime);
    }




    //피카츄 점프 함수
    void PikachuJump() {
        //점프 로직
        if (controller.isGrounded) {
            
                vSpeed = jumpSpeed;

                GameMgr.PlaySfx(jumpSound,false);
            
        }
    }
    //피카츄 좌우조작 함수
    void PikachuMove() {
      
        //h = Input.GetAxisRaw("Horizontal");
        movDir = Vector3.right * bm.h;
        movDir *= walkSpeed;
    }
    //피카츄 스윙 함수
    void PikachuSwing() {
        if ( Swinging==false && controller.isGrounded==false && Divinging==0) {
            StartCoroutine(PikachuSwingCoroutine());
            GameMgr.PlaySfx(swingSound , false);
        }
    }
    IEnumerator PikachuSwingCoroutine() {
        Swinging = true;
        yield return new WaitForSeconds(0.2f);
        Swinging = false;
    }
    //피카츄 다이브 함수
    void PikachuDive() {

        if (controller.isGrounded == true ) {
            
            if (bm.h != 0) {
                vSpeed = diveJumpSpeed;
                Divinging = (int)bm.h;
                StartCoroutine(PikachuDiveCoroutine((int)bm.h));

                //사운드 재생
                GameMgr.PlaySfx(diveSound,false);
                controller.center = new Vector3(0.35f , 0 , 0);
                controller.radius = 0.9f;
                controller.height = 0.0f;
            }
        }
    }
    IEnumerator PikachuDiveCoroutine(int direction) {
        
        yield return new WaitForSeconds(0.5f);
        controller.center = new Vector3(0 , 0 , 0);
        controller.radius = 0.8f;
        controller.height = 2.13f;
        Divinging = 0;
    }
    void Diving(int direction) {
       
        movDir = Vector3.right * direction;
        movDir *= diveSpeed;
       
    }
    //공과 충돌 함수
    
   
    


}
