using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class OverDto
    {
        public int WinIdentity;
        public List<int> WinUIdList;
        public int BeenCount;

        public OverDto()
        {

        }
    }
}
