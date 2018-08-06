using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class SceneMgr:ManagerBase
{
    public static SceneMgr Instance = null;

    private void Awake()
    {
        Instance = this;

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        Add(SceneEvent.LOAD_SCENE,this);
    }


    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case SceneEvent.LOAD_SCENE:
                LoadSceneMsg senceIndex = message as LoadSceneMsg;
                LoadSence(senceIndex);
                break;
            default:
                break;
        }
    }

    private Action OnSceneLoaded = null;

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="msg"></param>
    private void LoadSence(LoadSceneMsg msg)
    {
        if(msg.SceneBuildIndex!=-1)
            SceneManager.LoadScene(msg.SceneBuildIndex);
        if (msg.SceneBuildName != null)
            SceneManager.LoadScene(msg.SceneBuildName);
        if (msg.OnSenceLoad != null)
            OnSceneLoaded = msg.OnSenceLoad;
    }

    /// <summary>
    /// 当场景加载完成时调用
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (OnSceneLoaded != null)
            OnSceneLoaded();
    }
}

