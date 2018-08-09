using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
   public class ChatDto
    {
        public int UserId;
        public int ChatType;
        public ChatDto()
        {

        }

        public ChatDto(int userId,int chatType)
        {
            this.UserId = userId;
            this.ChatType = chatType;
        }
    }
}
