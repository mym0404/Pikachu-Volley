using UnityEngine;

public class Shadow : MonoBehaviour {
    public Transform ParentTr;
    public Transform Tr;
	
	 
	void Update () {
        Tr.rotation = Quaternion.identity;
        /*
        if (gameObject.tag == "공그림자")
        Tr.position = new Vector3(ParentTr.position.x , -4.3f, ParentTr.position.z);
        else
        */
        Tr.position = new Vector3(ParentTr.position.x , -4.0f, ParentTr.position.z);
	}
}
