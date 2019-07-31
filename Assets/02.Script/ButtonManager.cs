using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    public static ButtonManager instance = null;
    public float h = 0;
    public bool jump = false;
    public bool down = false;
    public bool up = false;

    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject upButton;
    public GameObject downButton;
    public GameObject jumpButton;


    void Awake() {
        instance = this;
    }
    //좌우버튼 이벤트 트리거
    public void OnDownLeftButton() {
        h = -1;
        leftButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 100f / 255f);
    }
    public void OnUpLeftButton() {
        if(h==-1)
            h = 0;
        leftButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 170f / 255f);
    }
    public void OnDownRightButton() {
        h = 1;
        rightButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 100f / 255f);
    }
    public void OnUpRightButton() {
        if(h==1)
            h = 0;
        rightButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 170f / 255f);
    }
    //위 아래 버튼
    public void OnDownUpButton() {
        upButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 100f / 255f);
       
        up = true;
    }
    public void OnUpUpButton() {
        up = false;

        upButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 170f / 255f);
    }
    public void OnDownDownButton() {
        down = true;
        downButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 100f / 255f);
    }
    public void OnUpDownButton() {
        down = false;
        downButton.GetComponent<Image>().color = new Color(1 , 1 , 1 , 170f / 255f);
    }

    public void OnDownJumpButton() {
        JumpButtonRotate.rotateSpeed = 3.5f;
        
    }
    public void OnUpJumpButton() {
        JumpButtonRotate.rotateSpeed = 1.0f;
    }

    public void OnClickSwingButton() {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("PLAYER")) {
            if (player.GetComponent<피카츄이동>().enabled == true) {
                player.SendMessage("PikachuSwing" , null);
                player.SendMessage("PikachuDive" , null);
            }
        }
        
        
    }

    public void OnClickJumpButton() {
         foreach (GameObject player in GameObject.FindGameObjectsWithTag("PLAYER")) {
            if (player.GetComponent<피카츄이동>().enabled == true) {
                player.SendMessage("PikachuJump" , null);
              
            }
        }
    }
    
}
