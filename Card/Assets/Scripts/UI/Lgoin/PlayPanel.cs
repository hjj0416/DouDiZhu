using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPanel : UIBase{

    private Button btnStart;
    private Button btnRegist;

	// Use this for initialization
	void Start () {
        btnStart = transform.Find("BtnStart").GetComponent<Button>();
        btnRegist = transform.Find("BtnRegist").GetComponent<Button>();

        btnStart.onClick.AddListener(StartClick);
        btnRegist.onClick.AddListener(RegistClick);
	}

    public override void OnDestroy()
    {
        btnStart.onClick.RemoveAllListeners();
        btnRegist.onClick.RemoveAllListeners();
    }

    void StartClick()
    {
        Dispatch(AreaCode.UI,UIEvent.START_PANEL_ACTIVE,true);
        //UIMgr.Instance.OpenWindow("StartPanel");
    }

    void RegistClick()
    {
        Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE,true);
    }
}
