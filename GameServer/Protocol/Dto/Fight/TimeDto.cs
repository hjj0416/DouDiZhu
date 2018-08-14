using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
   public class TimeDto
    {
        public int Id;//唯一id
        public long time;//计时多久
        public long timeStamp;//开始时的时间戳（单位：毫秒）

        public TimeDto()
        {

        }

        public TimeDto(int id,long time,long timestamp)
        {
            this.Id = id;
            this.time = time;
            this.timeStamp = timestamp;
        }

        public void Change(int id,long time, long timestamp)
        {
            this.Id = id;
            this.time = time;
            this.timeStamp = timestamp;
        }
    }
}
