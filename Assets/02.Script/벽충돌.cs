using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 벽충돌 : MonoBehaviour {
    
    private PhotonView pv;
	void Start () {
        pv = GetComponent<PhotonView>();
	}
	
	void Update () {
		 
	}
    private void OnTriggerEnter(Collider other) {
        //공과 충돌했을 때
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("BALL"))) {
            Rigidbody ballRbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 beforeVelocity = ballRbody.velocity;
            ballRbody.velocity = Vector3.zero;
            if (gameObject.tag == "천장바닥") {
                ballRbody.velocity = new Vector3(beforeVelocity.x , beforeVelocity.y * (-0.35f) , beforeVelocity.z);
            } else if (gameObject.tag == "벽") {
                ballRbody.velocity = new Vector3(beforeVelocity.x * (-0.7f) , beforeVelocity.y , beforeVelocity.z);
            } else if (gameObject.tag == "네트") {
                if (other.transform.position.y > -2.2 && ballRbody.velocity.y<0) { // 공이 아래로 떨어지고 있을 때만 위로 튀기기
                    ballRbody.velocity = new Vector3(beforeVelocity.x , beforeVelocity.y * (-1.0f) , beforeVelocity.z);

                } else {
                    ballRbody.velocity = new Vector3(beforeVelocity.x * (-0.7f) , beforeVelocity.y , beforeVelocity.z);
                }
                
            }
            //벽충돌 이펙트
            other.gameObject.SendMessage("벽닿는이펙트" , null , SendMessageOptions.DontRequireReceiver);
            
        }
        
    }
    

}
