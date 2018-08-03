using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AhpilyServer.Timers
{
    public class TimerMgr
    {
        private static TimerMgr instance = null;
        public static TimerMgr Instance
        {
            get
            {
                lock(instance)
                {
                    if (instance == null)
                        instance = new TimerMgr();
                    return instance;
                }
            }
        }

        Timer timer;

        //存储任务id和模型的映射
        private ConcurrentDictionary<int, TimeModel> idModelDict = new ConcurrentDictionary<int, TimeModel>();

        /// <summary>
        /// 要移除的任务id列表
        /// </summary>
        private List<int> removeList = new List<int>();

        public TimerMgr()
        {
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock(removeList)
            {
                TimeModel tempModel = null;
                foreach (var id in removeList)
                {
                    idModelDict.TryRemove(id, out tempModel);
                }
                removeList.Clear();
            }

            foreach(var model in idModelDict.Values)
            {
                model.Run(); 
            }
        }
    }
}
