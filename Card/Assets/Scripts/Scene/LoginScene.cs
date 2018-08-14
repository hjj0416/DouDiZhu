using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : MonoBehaviour {
    
	void Start () {
        UIMgr.Instance.OpenWindow("UILogin/PlayPanel",SceneID.Login);
        UIMgr.Instance.OpenWindow("UILogin/StartPanel", SceneID.Login);
        UIMgr.Instance.OpenWindow("UILogin/RegistPanel", SceneID.Login);
        UIMgr.Instance.OpenWindow("PromptPanel", SceneID.DEFAULT);
    }
	
}
