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
    public delegate void TimeDelegate();

    public class TimeModel
    {
        public int Id;

        /// <summary>
        /// 任务执行时间
        /// </summary>
        public long Time;

        public TimeDelegate timeDelegate;

        public TimeModel(int id,long time,TimeDelegate td)
        {
            this.Id = id;
            this.Time = time;
            this.timeDelegate = td; 
        }

        public void Run()
        {
            timeDelegate();
        }
    }
}
