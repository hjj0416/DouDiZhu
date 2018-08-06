using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LoadSceneMsg
{
    public int SceneBuildIndex;
    public string SceneBuildName;
    public Action OnSenceLoad;

    public LoadSceneMsg()
    {
        this.SceneBuildIndex = -1;
        this.SceneBuildName = null;
        this.OnSenceLoad = null;
    }

    public LoadSceneMsg(string name,Action loaded)
    {
        this.SceneBuildIndex = -1;
        this.SceneBuildName = name;
        this.OnSenceLoad = loaded;
    }

    public LoadSceneMsg(int index, Action loaded)
    {
        this.SceneBuildIndex = index;
        this.SceneBuildName = null;
        this.OnSenceLoad = loaded;
    }
}

