using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    public class CardWeight
    {
        public const int THREE = 3;
        public const int FOUR = 4;
        public const int FIVE = 5;
        public const int SIX = 6;
        public const int SEVEN = 7;
        public const int EIGHT = 8;
        public const int NINE = 9;
        public const int TEN = 10;

        public const int JACK = 11;
        public const int QUEEN = 12;
        public const int KING = 13;

        public const int ONE = 14;
        public const int TWO = 15;

        public const int SJOKER = 16;
        public const int LJOKER = 17;

        public static string GetString(int weight)
        {
            switch (weight)
            {
                case 3:
                    return "Three";
                case 4:
                    return "Four";
                case 5:
                    return "Five";
                case 6:
                    return "Six";
                case 7:
                    return "Seven";
                case 8:
                    return "Eight";
                case 9:
                    return "Nine";
                case 10:
                    return "Ten";
                case 11:
                    return "Jack";
                case 12:
                    return "Queen";
                case 13:
                    return "King";
                case 14:
                    return "One";
                case 15:
                    return "Two";
                case 16:
                    return "SJoker";
                case 17:
                    return "LJoker";
                default:
                    throw new Exception("不存在这样的类型");
            }
        }

        /// <summary>
        /// 获取权值
        /// </summary>
        /// <param name="cardList"></param>
        /// <param name="cardType"></param>
        /// <returns></returns>
        public static int GetWeight(List<CardDto> cardList,int cardType)
        {
            int totalWeight = 0;
            if(cardType==CardType.THREE_ONE||cardType ==CardType.THREE_TWO)
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    if(cardList[i].Weight==cardList[i+1].Weight&&cardList[i].Weight==cardList[i+2].Weight)
                    {
                        totalWeight += (cardList[i].Weight * 3);
                    }
                }
            }
            else 
            {
                for (int i = 0; i < cardList.Count; i++)
                {
                    totalWeight += cardList[i].Weight;
                }
            }
            return totalWeight;
        }
    }
}
