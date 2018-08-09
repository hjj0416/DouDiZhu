using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
   public class Constant
    {
        private static Dictionary<int, string> typeTextDict = new Dictionary<int, string>();

        static Constant()
        {
            typeTextDict = new Dictionary<int, string>();
            typeTextDict.Add(1, "大家好，很高兴见到各位~");
            typeTextDict.Add(2, "和你合作真是太愉快了！");
            typeTextDict.Add(3, "快点儿吧，我等到花儿都谢了");
            typeTextDict.Add(4, "你的牌打得太好了！");
            typeTextDict.Add(5, "不要吵了，有什么好吵的，专心玩游戏吧！");
            typeTextDict.Add(6, "不要走，决战到天亮！");
            typeTextDict.Add(7, "再见了，我会想念大家的！");
        }

        public static string GetChatText(int chatType)
        {
            return typeTextDict[chatType];
        }
    }
}
