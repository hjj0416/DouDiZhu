using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.Fight
{
    [Serializable]
    public class CardDto
    {
        public string Name;
        public int Color;//红桃
        public int Weight; //3 J Q 

        public CardDto()
        {

        }

        public CardDto(string name, int color, int weight)
        {
            this.Name = name;
            this.Color = color;
            this.Weight = weight;
        }
    }
}
