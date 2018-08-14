using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIMgr.Instance.OpenWindow("UIMain/InfoPanel", SceneID.Main);
        UIMgr.Instance.OpenWindow("UIMain/CreatePanel", SceneID.Main);
        UIMgr.Instance.OpenWindow("UIMain/SettingPanel", SceneID.Main);
        UIMgr.Instance.OpenWindow("UIMain/MatchPanel", SceneID.Main);
    }

}
