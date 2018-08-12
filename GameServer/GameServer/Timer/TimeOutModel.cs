using GameServer.Cache.Fight;
using GameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AhpilyServer.Timers
{
    /// <summary>
    /// 定时器到时间触发
    /// </summary>
    public delegate void TimeOutDelegate(int userId);

    public class TimeOutModel
    {
        public int Id;

        public int userId;

        /// <summary>
        /// 任务执行时间
        /// </summary>
        public long Time;

        public TimeOutDelegate timeOutDelegate;

        public TimeOutModel(int id, int userId, long time, TimeOutDelegate td)
        {
            this.Id = id;
            this.Time = time;
            this.timeOutDelegate = td;
            this.userId = userId;
        }

        public void Run()
        {
            timeOutDelegate(userId);
        }
    }
}