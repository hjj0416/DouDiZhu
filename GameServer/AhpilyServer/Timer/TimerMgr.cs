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

        //表示id
        private ConcurrentInt id = new ConcurrentInt(-1);

        public TimerMgr()
        {
            timer = new Timer(10);
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

            foreach (var model in idModelDict.Values)
            {
                if (model.Time <= DateTime.Now.Ticks)
                    model.Run();
            }
        }

        //添加定时任务 XX：XX：XX执行
        public void AddTimerEvent(DateTime dateTime,TimeDelegate timeDelegate)
        {
           long delayTime=dateTime.Ticks - DateTime.Now.Ticks;
            if (delayTime <= 0)
                return;
            AddTimeEvent(delayTime, timeDelegate);
        }

        //添加定时任务 XX时间后执行
        public void AddTimeEvent(long delayTime,TimeDelegate timeDelegate)
        {
            TimeModel model = new TimeModel(id.Add_Get(),DateTime.Now.Ticks+delayTime,timeDelegate);
            idModelDict.TryAdd(model.Id,model);
        }

        //移除任务
        public void RemoveEvent(TimeDelegate timeDelegate)
        {
            foreach (TimeModel td in idModelDict.Values)
            {
                if (td.timeDelegate == timeDelegate)
                {
                    TimeModel timeModel;
                    idModelDict.TryRemove(td.Id, out timeModel);
                    break;
                }
                    
            }
        }
    }
}
 