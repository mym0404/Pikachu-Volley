using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScStartButton : MonoBehaviour {
    public AudioClip buttonSound;
    public Text 시작텍스트;
    public AudioClip BGM;
    public GameObject 패널;
    private Image 패널이미지;
    private float alphaValue=0f;
    private float panelAlphaValue = 0f;

    public void OnClickStartButton() {
        GameMgr.PlaySfx(buttonSound,false);
        StartCoroutine(StartButton());
    }
    IEnumerator StartButton() {
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("scLobby");
    }
	void Start () {
        /* 버전처리
        if (float.Parse(PlayerSettings.bundleVersion) > versionOfThisGame) {
            Debug.Log("버전이 낮습니다 업데이트 필요!");
            Debug.Log(Application.version);
            return;
        } 
        */
       
            Screen.SetResolution(1280 , 800 , true);

        GameMgr.PlaySfx(BGM,true);
        StartCoroutine(GameMgr.PlayBGMAgain(BGM));
        시작텍스트.color = new Color(0 , 0 , 0 , 0.0f);
        패널이미지 = 패널.GetComponent<Image>();
        패널이미지.color = new Color(1.0f , 1.0f , 1.0f , 0.0f);
        StartCoroutine(TextLighting());
	}
    IEnumerator TextLighting() {
        float upAndDown = -0.015f;
        while (true) {
            
            if (alphaValue <=0)
                upAndDown = 0.015f;
            if (alphaValue >= 1f)
                upAndDown = -0.015f;

            yield return new WaitForSeconds(0.01f);
            alphaValue += upAndDown;
            panelAlphaValue += upAndDown / 2;
            시작텍스트.color = new Color(0 , 0 , 0 , alphaValue);
            패널이미지.color = new Color(1.0f , 1.0f , 1.0f , panelAlphaValue);
            //Debug.Log(alphaValue);
            

        }
    }
	void Update () {
        
	}
}
