using Protocol.Constant;
using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Cache.Fight
{

    public class LibraryModel
    {
        /// <summary>
        /// 牌库
        /// </summary>
        public Queue<CardDto> cardQueue { get; set; }

        public LibraryModel()
        {
            //创建牌
            Create();
            //洗牌
            shuffle();
        }

        public void Init()
        {
            //创建牌
            Create();
            //洗牌
            shuffle();
        }

        private void Create()
        {
            cardQueue = new Queue<CardDto>();
            //创建普通的牌
            for (int color = CardColor.CLUB; color <= CardColor.SQUARE; color++)
            {
                for (int weight = CardWeight.THREE; weight <= CardWeight.TWO; weight++)
                {
                    string cardName = CardColor.GetString(color) + CardWeight.GetString(weight);
                    CardDto card = new CardDto(cardName,color,weight);
                    //添加到CardQuene里面
                    cardQueue.Enqueue(card);
                }
            }
            CardDto sJoker = new CardDto("SJoker", CardColor.NONE, CardWeight.SJOKER);
            CardDto lJoker = new CardDto("LJoker",CardColor.NONE,CardWeight.LJOKER);
            cardQueue.Enqueue(sJoker);
            cardQueue.Enqueue(lJoker);
        }

        /// <summary>
        /// 洗牌
        /// </summary>
        private void shuffle()
        {
            List<CardDto> cardDto = new List<CardDto>();
            Random r = new Random();
            foreach (CardDto card in cardQueue)
            {
                int index = r.Next(0,cardDto.Count+1);
                cardDto.Insert(index,card);
            }
            cardQueue.Clear();
            foreach (CardDto card in cardDto)
            {
                cardQueue.Enqueue(card);
            }
        }

        public CardDto Deal()
        {
            return cardQueue.Dequeue();
        }
    }
}
