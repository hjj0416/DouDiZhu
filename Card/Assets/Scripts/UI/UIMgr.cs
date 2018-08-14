using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    [SerializeField] RectTransform layer;
    [SerializeField] Canvas canvas;

    #region 单例
    private static UIMgr mInstance;
    /// <summary>
    /// 获取资源加载实例
    /// </summary>
    /// <returns></returns>
    public static UIMgr Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject go = GameObject.Find("UIRoot");
                if (go == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("UIPrefabs/UIRoot") as GameObject;
                    go = Instantiate(prefab);
                    Transform rootTf = go.transform;
                    rootTf.SetParent(null);
                }
                mInstance = go.GetComponent<UIMgr>();
                mInstance.OnInit();
                DontDestroyOnLoad(go);
            }
            return mInstance;
        }
    }

    #endregion


    public void Setup()
    {
    }

    private void OnInit()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("on scene changed !!!!");
        int curSceneID = scene.buildIndex;
        int count = layer.childCount;
        for (int i = 0; i < count; i++)
        {
            var child = layer.GetChild(i);
            UIBase uIBase = child.GetComponent<UIBase>();
            if (uIBase.sceneID != curSceneID)
            {
                Destroy(child.gameObject);
            }
        }
    }

    #region UI API

    public void OpenWindow(string winName, int sceneID)
    {
        string uiPath = string.Format("UIPrefabs/{0}", winName);
        GameObject prefab = Resources.Load<GameObject>(uiPath);
        if(prefab == null)
        {
            Debug.LogError("Error : Load UI prefab failed : " + uiPath);
            return;
        }
        GameObject uiGo = Instantiate(prefab);
        RectTransform rectTransform = uiGo.transform as RectTransform;
        rectTransform.parent = layer;
        rectTransform.anchoredPosition = Vector2.zero;

        UIBase uiBase = uiGo.GetComponent<UIBase>();
        uiBase.sceneID = sceneID;
    }

    public void CloseWindow(string winName)
    {
        int count = layer.childCount;
        for(int i=0;i<count;i++)
        {
            var child = layer.GetChild(i);
            if(child.name.Equals(winName))
            {
                Destroy(child.gameObject);
                return;
            }
        }
    }

    #endregion
}
