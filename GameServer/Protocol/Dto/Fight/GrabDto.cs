using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
   public class GrabDto
    {
        public int userId;
        public List<CardDto> TableCardList;
        public List<CardDto> PlayerCardList;

        public GrabDto()
        {

        }

        public GrabDto(int userId,List<CardDto> cards,List<CardDto> playerCards)
        {
            this.userId = userId;
            this.TableCardList = cards;
            this.PlayerCardList = playerCards;
        }
    }
}
