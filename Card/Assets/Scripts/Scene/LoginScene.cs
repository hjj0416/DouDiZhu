using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : MonoBehaviour {
    
	void Start () {
        UIMgr.Instance.OpenWindow("PlayPanel",SceneID.Login);
        UIMgr.Instance.OpenWindow("StartPanel", SceneID.Login);
        UIMgr.Instance.OpenWindow("RegistPanel", SceneID.Login);
    }
	
}
