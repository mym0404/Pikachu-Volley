using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class 로비옵션 : MonoBehaviour {
    public GameObject 옵션창;
    public Slider BGMSlider;
    public Slider EffectSlider;
    public AudioClip ButtonSound;
    public GameObject ContactPanel;
	void Start () {
        
	}
	
	void Update () {
        
        
         
	}
    //로비에서 옵션버튼을 눌렀을 때
    public void OnClickOptionButton() {
        옵션창.SetActive(true);
       
        BGMSlider.value = GameMgr.bgmVolume;
        EffectSlider.value = GameMgr.effectVolume;
    }
    //옵션창에서 X 버튼을 눌렀을 때
    public void OnClickExitOptionButton() {
        GameMgr.PlaySfx(ButtonSound , false);
        옵션창.SetActive(false);
    }
    
    //옵션창에서  BGM슬라이더를 조절했을 때
    public void OnChangeBGMSliderValue() {
        GameMgr.bgmVolume = BGMSlider.value;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SFX")) {
                obj.GetComponent<AudioSource>().volume = GameMgr.bgmVolume;    
            }
    }
    //옵션창에서  EFFECT슬라이더를 조절했을 때
    public void OnChangeEffectSliderValue() {
        GameMgr.effectVolume = EffectSlider.value;
    }

    //옵션창에서 Contact버튼 눌렀을 때
    public void OnClickContactButton() {
        ContactPanel.SetActive(true);
    }
    //Contact 패널에서 나가기 버튼 눌렀을 때
    public void OnClickExitContactButton() {
        ContactPanel.SetActive(false);
    }
}
