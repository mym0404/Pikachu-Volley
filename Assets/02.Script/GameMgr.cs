using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour {
   // public static bool isSfxMute = false;
    public static bool isBgmMute=false;
    public static bool isEffectMute = false;
    public static float effectVolume = 0.3f;
    public static float bgmVolume = 0.3f;
    readonly public static int endScore = 7;

  //  public static bool isMasterLastTouch = true;

    public static PhotonView pv = new PhotonView();
    void Start () {
        //instance = this;
        
	}
	
    //사운드 공용함수
    [PunRPC]
    public static void PlaySfx(AudioClip sfx, bool isBgm) {
        //만약 음소거 옵션이 설정되있으면 함수를 반환
        //if (isSfxMute == true)
            //return;
        if (isBgm==true && isBgmMute==true)
            return;
        if (isEffectMute == true && isBgm == false)
            return;
        //게임 오브젝트를 동적으로 생성 
        GameObject soundObj = new GameObject("Sfx");
        soundObj.tag = "SFX";
        //사운드 발생 위치 지정
        soundObj.GetComponent<Transform>().position = Vector2.zero;
        //생성한 게임오브젝트에 AudioSource 컴포넌트 추가
        AudioSource audio = soundObj.AddComponent<AudioSource>();
        //컴포넌트 속성 설정    
        audio.clip = sfx;
        //전체적인 볼륨 설정
        if (isBgm == true)
            audio.volume = bgmVolume;
        else
            audio.volume = effectVolume;
 
        //오디오클립 재생
        audio.Play();
        //사운드의 플레이가 종료되면 동적으로 생성한 게임오브젝트를 
        Destroy(soundObj , sfx.length);
    }
    //BGM을 다시 돌려주는 함수
    public static IEnumerator PlayBGMAgain(AudioClip clip) {
        yield return new WaitForSeconds(clip.length+3.0f);
        PlaySfx(clip,true);
    }

    //안내창 뜨는 함수
    public static void PopUp(GameObject 팝업창,string txt) {
 
        if (팝업창 == null) {
            Debug.Log("null");
            return;

        }
        Debug.Log("팝업창 찾음");
        팝업창.SetActive(true);
        Text 텍스트 = 팝업창.GetComponentInChildren<Image>().gameObject.GetComponentInChildren<Text>();
        텍스트.text = txt;
        
    }
    public void OnClickPopUpOkButton() {
        GameObject 팝업창=GameObject.FindGameObjectWithTag("팝업창");
        팝업창.SetActive(false);
    }
}
