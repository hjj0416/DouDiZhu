using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    /// <summary>
    /// 玩家的传输模型
    /// </summary>
    [Serializable]
   public class PlayerDto
    {
        public int UserId;//id
        public int Identity;//身份
        public List<CardDto> CardList;//手牌

        public PlayerDto(int userId)
        {
            this.Identity = Protocol.Constant.Identity.FARMER;
            this.UserId = userId;
            this.CardList = new List<CardDto>();
        }

        /// <summary>
        /// 是否有手牌
        /// </summary>
        public bool HasCard
        {
            get { return CardList.Count != 0; }
        }

        /// <summary>
        /// 当期卡牌数量
        /// </summary>
        public int CardCount
        {
            get { return CardList.Count; }
        }

        /// <summary>
        /// 添加卡牌
        /// </summary>
        /// <param name="card"></param>
        public void Add(CardDto card)
        {
            CardList.Add(card);
        }

        /// <summary>
        /// 移除卡牌
        /// </summary>
        /// <param name="card"></param>
        public void Remove(CardDto card)
        {
            CardList.Remove(card);
        }

    }
}
