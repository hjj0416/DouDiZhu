using AhpilyServer.Concurrent;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AhpilyServer.Timers
{
    public class TimeOutManager
    {
        private static TimeOutManager instance = null;
        public static TimeOutManager Instance
        {
            get
            {
                lock (obj)
                {
                    if (instance == null)
                        instance = new TimeOutManager();
                    return instance;
                }
            }
        }

        private static object obj = 1;

        Timer timer;

        //存储任务id和模型的映射
        private ConcurrentDictionary<int, TimeOutModel> idModelDict = new ConcurrentDictionary<int, TimeOutModel>();

        /// <summary>
        /// 要移除的任务id列表
        /// </summary>
        private List<int> removeList = new List<int>();

        //表示id
        private ConcurrentInt id = new ConcurrentInt(-1);

        public TimeOutManager()
        {
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (removeList)
            {
                TimeOutModel tempModel = null;
                foreach (var id in removeList)
                {
                    idModelDict.TryRemove(id, out tempModel);
                }
                removeList.Clear();
            }

            foreach (var model in idModelDict.Values)
            {
                if (model.Time <= DateTime.Now.Ticks)
                    model.Run();
            }
        }

        //添加定时任务 XX：XX：XX执行
        public void AddTimerEvent(DateTime dateTime, int userId,TimeOutDelegate timeOutDelegate)
        {
            long delayTime = dateTime.Ticks - DateTime.Now.Ticks;
            if (delayTime <= 0)
                return;
            AddTimeEvent(delayTime, userId, timeOutDelegate);
        }

        //添加定时任务 XX时间后执行
        public void AddTimeEvent(long delayTime,int userId,TimeOutDelegate timeOutDelegate)
        {
            Console.WriteLine("add timeout event   "+ userId);
            TimeOutModel model = new TimeOutModel(id.Add_Get(),userId,DateTime.Now.Ticks + delayTime, timeOutDelegate);
            idModelDict.TryAdd(model.Id, model);
        }

        //移除任务
        public void RemoveEvent(int userId)
        {
            Console.WriteLine("remove timeout event   " + userId);
            foreach (TimeOutModel td in idModelDict.Values)
            {
                if (td.userId == userId)
                {
                    removeList.Add(td.Id);
                    break;
                }

            }
        }

        public bool IsHaveEvent(int userId)
        {
            foreach (TimeOutModel td in idModelDict.Values)
            {
                if (td.userId == userId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
