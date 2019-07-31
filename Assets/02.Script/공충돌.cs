using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 공충돌 : MonoBehaviour {
    private Transform tr;
    private Rigidbody rg;
    private bool isPushed = false;
	void Start () {
        tr = GetComponent<Transform>();
        rg = GetComponent<Rigidbody>();
	}

    void Update() {
        if ((tr.position.x <= -8 || tr.position.x>=8) && isPushed==false) {
            isPushed = true;
            rg.velocity = new Vector3(rg.velocity.x * (-1) , rg.velocity.y , rg.velocity.z);
            StartCoroutine(isPushedOn());
        }
    }
    IEnumerator isPushedOn() {
        yield return new WaitForSeconds(0.8f);
        isPushed = false;
    }

}
