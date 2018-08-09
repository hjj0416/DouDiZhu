using Protocol.Dto.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constant
{
    /// <summary>
    /// 卡牌类型
    /// </summary>
   public class CardType
    {
        public const int NONE = 0;
        public const int SINGLE = 1;//单张
        public const int DOUBLE = 2;//对子
        public const int STRAIGHT = 3;//顺子
        public const int DOUBLE_STRAIGHT = 4;//双顺
        public const int TRIPLE_STRAIGHT = 5;//飞机
        public const int THREE = 6;//三不带
        public const int THREE_ONE = 7;//三带一
        public const int THREE_TWO = 8;//三带二
        public const int BOOM = 9;//炸弹
        public const int JOKER_BOOM = 10;//王炸

        /// <summary>
        /// 是否是单牌
        /// </summary>
        /// <param name="cards">选择的手牌</param>
        /// <returns></returns>
        public static bool IsSingle(List<CardDto> cards)
        {
            return cards.Count == 1;
        }

        /// <summary>
        /// 判断是否是对儿
        /// </summary>
        /// <param name="cards">选择的手牌</param>
        /// <returns></returns>
        public static bool IsDouble(List<CardDto> cards)
        {
            if (cards.Count == 2)
            {
                if (cards[0].Weight == cards[1].Weight)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsStraight(List<CardDto> cards)
        {
            if (cards.Count < 5 || cards.Count > 12)
                return false;

            // 34567   45679  JQKA2
            for (int i = 0; i < cards.Count - 1; i++)
            {
                int tempWeight = cards[i].Weight;
                if (cards[i + 1].Weight - tempWeight != 1)
                    return false;
                //不能超过A
                if (tempWeight > CardWeight.ONE || cards[i + 1].Weight > CardWeight.ONE)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否是双顺
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsDoubleStraight(List<CardDto> cards)
        {
            //  33 44 55 
            if (cards.Count < 6 || cards.Count % 2 != 0)
                return false;

            for (int i = 0; i < cards.Count - 2; i += 2)
            {
                if (cards[i].Weight != cards[i + 1].Weight)
                    return false;
                if (cards[i + 2].Weight - cards[i].Weight != 1)
                    return false;
                //不能超过A
                if (cards[i].Weight > CardWeight.ONE || cards[i + 2].Weight > CardWeight.ONE)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否是飞机
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsTripleStraight(List<CardDto> cards)
        {
            //333 444 555
            // 33344456  333444 66 77
            if (cards.Count < 6 || cards.Count % 3 != 0)
                return false;

            for (int i = 0; i < cards.Count - 3; i += 3)
            {
                if (cards[i].Weight != cards[i + 1].Weight)
                    return false;
                if (cards[i + 2].Weight != cards[i + 1].Weight)
                    return false;
                if (cards[i].Weight != cards[i + 2].Weight)
                    return false;

                if (cards[i + 3].Weight - cards[i].Weight != 1)
                    return false;
                //不能超过A
                if (cards[i].Weight > CardWeight.ONE || cards[i + 3].Weight > CardWeight.ONE)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否是三不带
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThree(List<CardDto> cards)
        {
            //333
            if (cards.Count != 3)
                return false;
            if (cards[0].Weight != cards[1].Weight)
                return false;
            if (cards[2].Weight != cards[1].Weight)
                return false;
            if (cards[0].Weight != cards[2].Weight)
                return false;

            return true;
        }

        /// <summary>
        /// 是否是三带一
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndOne(List<CardDto> cards)
        {
            if (cards.Count != 4)
                return false;

            //5333 3335
            if (cards[0].Weight == cards[1].Weight && cards[1].Weight == cards[2].Weight)
                return true;
            else if (cards[1].Weight == cards[2].Weight && cards[2].Weight == cards[3].Weight)
                return true;

            return false;
        }

        /// <summary>
        /// 是否是三带二
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsThreeAndTwo(List<CardDto> cards)
        {
            if (cards.Count != 5)
                return false;
            //33355 55333
            if (cards[0].Weight == cards[1].Weight && cards[1].Weight == cards[2].Weight)
            {
                if (cards[3].Weight == cards[4].Weight)
                    return true;
            }
            else if (cards[2].Weight == cards[3].Weight && cards[3].Weight == cards[4].Weight)
            {
                if (cards[0].Weight == cards[1].Weight)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判断是否是炸弹
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsBoom(List<CardDto> cards)
        {
            if (cards.Count != 4)
                return false;
            // 0000
            if (cards[0].Weight != cards[1].Weight)
                return false;
            if (cards[1].Weight != cards[2].Weight)
                return false;
            if (cards[2].Weight != cards[3].Weight)
                return false;

            return true;
        }

        /// <summary>
        /// 判断是不是王炸
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static bool IsJokerBoom(List<CardDto> cards)
        {
            if (cards.Count != 2)
                return false;

            if (cards[0].Weight == CardWeight.SJOKER && cards[1].Weight == CardWeight.LJOKER)
                return true;
            else if (cards[0].Weight == CardWeight.LJOKER && cards[1].Weight == CardWeight.SJOKER)
                return true;

            return false;
        }


        /// <summary>
        /// 获取卡牌类型
        /// </summary>
        /// <param name="cardList">要出的牌</param>
        public static int GetCardType(List<CardDto> cardList)
        {
            int cardType = CardType.NONE;

            switch (cardList.Count)
            {
                case 1:
                    if (IsSingle(cardList))
                    {
                        cardType = CardType.SINGLE;
                    }
                    break;
                case 2:
                    if (IsDouble(cardList))
                    {
                        cardType = CardType.DOUBLE;
                    }
                    else if (IsJokerBoom(cardList))
                    {
                        cardType = CardType.JOKER_BOOM;
                    }
                    break;
                case 3:
                    if (IsThree(cardList))
                    {
                        cardType = CardType.THREE;
                    }
                    break;
                case 4:
                    if (IsBoom(cardList))
                    {
                        cardType = CardType.BOOM;
                    }
                    else if (IsThreeAndOne(cardList))
                    {
                        cardType = CardType.THREE_ONE;
                    }
                    break;
                case 5:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    else if (IsThreeAndTwo(cardList))
                    {
                        cardType = CardType.THREE_TWO;
                    }
                    break;
                case 6:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    else if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    else if (IsTripleStraight(cardList))
                    {
                        cardType = CardType.TRIPLE_STRAIGHT;
                    }
                    break;
                case 7:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    break;
                case 8:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    else if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    break;
                case 9:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    //777 888 999 
                    else if (IsTripleStraight(cardList))
                    {
                        cardType = CardType.TRIPLE_STRAIGHT;
                    }
                    break;
                case 10:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    else if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    break;
                case 11:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    break;
                case 12:
                    if (IsStraight(cardList))
                    {
                        cardType = CardType.STRAIGHT;
                    }
                    else if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    // 444 555 666 777
                    else if (IsTripleStraight(cardList))
                    {
                        cardType = CardType.TRIPLE_STRAIGHT;
                    }
                    break;
                case 13:
                    //345678910JQKA
                    break;
                case 14:
                    if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    break;
                case 15:
                    if (IsTripleStraight(cardList))
                    {
                        cardType = CardType.TRIPLE_STRAIGHT;
                    }
                    break;
                case 16:
                    if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    break;
                case 17:
                    break;
                case 18:
                    if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    // 444 555 666 777 888 999 
                    else if (IsTripleStraight(cardList))
                    {
                        cardType = CardType.TRIPLE_STRAIGHT;
                    }
                    break;
                case 19:
                    break;
                case 20:
                    //33 44 55 66 77 88 99 1010 JJ QQ KK AA
                    if (IsDoubleStraight(cardList))
                    {
                        cardType = CardType.DOUBLE_STRAIGHT;
                    }
                    break;
                default:
                    break;
            }

            return cardType;
        }


    }
}
