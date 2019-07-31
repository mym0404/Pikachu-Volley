using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 공상태 : MonoBehaviour {
    private Transform tr;
    private Rigidbody rg;
    private PhotonView pv;
    public AudioClip swingSound;
    public AudioClip ElectricSound;
    //전기 이펙트
    public GameObject electricEffect;
     //스윙 이펙트
    public GameObject SwingEffect;
    //벽닿는 이펙트
    public GameObject 벽Effect;



    private void Awake() {
        pv = GetComponent<PhotonView>();
        tr = GetComponent<Transform>();
        rg = GetComponentInParent<Rigidbody>();
    
      

	}
    

	void Update () {
        RotateBall();
        
	}
    //스윙 이펙트
    void Swing() {
        PhotonNetwork.RPC(pv , "SwingRPC" , PhotonTargets.AllViaServer , false , null);
    }
    [PunRPC]
    void 벽닿는이펙트() {
        //벽 충돌 이펙트
            GameObject 네트충돌 = Instantiate(벽Effect,tr.position,Quaternion.identity);
                Destroy(네트충돌 , 3.0f);
    }
    [PunRPC]
    void SwingRPC() {
        GameObject Seffect = Instantiate(SwingEffect,tr.position,Quaternion.identity);
        Destroy(Seffect , 2.0f);

        StartCoroutine(이펙트공따라가기(Instantiate(electricEffect,tr.position,Quaternion.identity)));
        //이펙트 사운드
        GameMgr.PlaySfx(ElectricSound , false);
        GameMgr.PlaySfx(swingSound,false);
    }
    IEnumerator 이펙트공따라가기(GameObject effect) {
        int count = 0;
        while (count<=20) {
            effect.transform.position = tr.position;
            yield return new WaitForSeconds(0.1f);
            count++;
            
        }
        Destroy(effect);
    }
 
    //공 회전 로직
    private void RotateBall() {
       if(PhotonNetwork.isMasterClient)
            tr.Rotate(new Vector3(0 , 0 , -GetBallSpeed()));
    }
    private float GetBallSpeed() {
        return rg.velocity.x ;
    }
    
}
