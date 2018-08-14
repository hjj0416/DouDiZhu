using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class ResultDto
    {
        public int Id;//用户id
        public bool result;//处理的结果

        public ResultDto()
        {

        }

        public ResultDto(int id,bool result)
        {
            this.Id = id;
            this.result = result;
        }
    }
}
