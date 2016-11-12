using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
   {
    /// <summary>
    /// Badugi Hand Evaluator
    /// -------------------------------------------------
    /// Version: 2.0
    /// Last revision: 22-11-2008
    /// Author: Tony Pottier
    /// Website: http://www.cartridgesoftware.com
    /// Contact: mailto:support@cartridgesoftware.com
    /// -------------------------------------------------
    /// This code is distributed under the LGPL license
    /// You can use this code in any project, but notify
    /// me if you make any change.
    /// <example>
    /// h = new BadugiHand("Ac", "Th", "3c", "Kc");
    /// string s = BadugiHand.DescriptionFromHandMask(h.HandMask);
    /// or s = h.ToString();
    /// or s = (string)h;
    /// </example>
    /// </summary>
    public class BadugiHand : IComparable
    {

        #region declarations
        private uint handmask = 0x00000000;
        private uint handvalue = 0x00000000;
        private int handrank = 0;
        private BadugiCard[] hand = new BadugiCard[4];
        private static Dictionary<char, int> suitsCount = new Dictionary<char, int>();
        public BadugiHandValue Category = new BadugiHandValue();
        #endregion

        #region properties
        /// <summary>
        /// Access the hand mask of this BadugiHand instance.
        /// </summary>
        public uint HandMask
        {
            get { return handmask; }
        }

        public uint HandValue
        {
            get { return handvalue; }
        }

        /// <summary>
        /// 4 = Badugi
        /// 3 = 3-Card
        /// etc
        /// </summary>
        public int HandRank
        {
            get { return handrank; }
        }

        #endregion
        
        #region constructor
        static BadugiHand()
        {
            suitsCount.Add('c', 0);
            suitsCount.Add('d', 0);
            suitsCount.Add('h', 0);
            suitsCount.Add('s', 0);
        }

        /// <summary>
        /// Construct and evaluate a badugi hand
        /// Cards must be formatted as follow: As Kc Td 3h, etc. etc.
        /// 10d or KC will throw an exception.
        /// </summary>
        public BadugiHand(string card0, string card1, string card2, string card3)
        {
            if (card0 != "")
            {
                //generate the array used
                hand[0] = new BadugiCard(card0);
                hand[1] = new BadugiCard(card1);
                hand[2] = new BadugiCard(card2);
                hand[3] = new BadugiCard(card3);

                //sort it so that we only have to iterate once
                //through the array
                Array.Sort(hand);

                //evaluate
                this.Evaluate();
                this.Category = this.HandVal();
            }
        }
        public BadugiHand(string s)
        {
            for (int i=1;i<s.Length; i+=2)
            {
                hand[i / 2] = new BadugiCard(s[i - 1].ToString() + s[i]);
            }
            Array.Sort(hand);            
            this.Evaluate();
            this.Category = this.HandVal();
        }
        #endregion

        #region members
        /// <summary>
        /// Get a complete description from a hand value
        /// </summary>
        /// <param name="hvalue">A badugi hand value</param>
        /// <returns></returns>
        public static string DescriptionFromHandValue(uint hvalue)
        {
            if (hvalue == 0) return "";

            string s = "";
            string c = "";

            // Badugi/3-card/2-card/1-card
            uint badugi = hvalue >> 28;
            if (badugi == 4)
                s += "Badugi: ";
            else
                s += badugi.ToString() + "-card: ";

            //cards of the badugi hand
            int offset = 7 * (4 - (int)badugi);
            for (uint i = 0; i < badugi; i++)
            {
                uint ui = hvalue;
                ui = ui >> offset; //remove cards that are not part of the badugi hand
                ui = ui >> (int)i * 7; //shift card to LSB
                ui = ui & 0x0000007F; //remove everything but the card value
                if (i != 0)
                    c = "," + c;
                c = BadugiCard.CreateCardFromValue((byte)ui).Card[0] + c;
            }


            return s + c;
        }



        /// <summary>
        /// Get a complete description from a mask
        /// </summary>
        /// <param name="hvalue">A badugi hand mask</param>
        /// <returns></returns>
        public static string DescriptionFromHandMask(uint hmask)
        {
            if (hmask == 0) return "";

            string s = "";
            string c = "";

            // Badugi/3-card/2-card/1-card
            uint badugi = hmask >> 28;
            s += badugi.ToString() + ";";

            //cards of the badugi hand
            int offset = 7 * (4 - (int)badugi);
            for (uint i = 0; i < badugi; i++)
            {
                uint ui = hmask;
                ui = ui >> offset; //remove cards that are not part of the badugi hand
                ui = ui >> (int)i * 7; //shift card to LSB
                ui = ui & 0x0000007F; //remove everything but the card value
                if (i != 0)
                    c = "," + c;
                c = BadugiCard.CreateCardFromValue((byte)ui).Card[0] + c;
            }
            return s + c;
        }
        public BadugiHandValue HandVal()
        {
            var bhv = new BadugiHandValue();
            var val = DescriptionFromHandMask(this.HandMask);
            var vals = val.Split(new char[] { ';' });
            bhv.CardsEval = Convert.ToInt32(vals[0]);
            bhv.Cards = String.Concat(vals[1].Split(new char[] { ',' }).OrderBy(c=>c));
            return bhv;
        }
        public string Show()
        {
            return String.Concat(this.hand.Select(j => j.Show));
        }
        private void Evaluate()
        {
            //declarations
            bool[] toRemove = new bool[4];
            int toRemoveCount = 0;

            //count different suits
            suitsCount['c'] = 0;
            suitsCount['d'] = 0;
            suitsCount['h'] = 0;
            suitsCount['s'] = 0;
            for (int i = 0; i < 4; i++)
            {
                suitsCount[hand[i].Card[1]]++;
                toRemove[i] = false;
            }


            //eliminate same card face by chosing one from the highest suit count
            BadugiCard c = hand[0]; int ci = 0;
            for (int i = 1; i < 4; i++)
            {
                if (!toRemove[i])
                {
                    //same face
                    if (c.Card[0] == hand[i].Card[0])
                    {
                        if (suitsCount[c.Card[1]] > suitsCount[hand[i].Card[1]])
                        {
                            toRemove[ci] = true;
                            toRemoveCount++;
                            suitsCount[c.Card[1]]--;
                            ci = i;
                            c = hand[i];
                        }
                        else
                        {
                            toRemove[i] = true;
                            toRemoveCount++;
                            suitsCount[hand[i].Card[1]]--;
                        }
                    }
                    else
                    {
                        //not same face?
                        ci = i;
                        c = hand[i];
                    }
                }
            }

            //eliminate same suits
            for (int i = 0; i < 4; i++)
            {
                if (!(toRemove[i]) && suitsCount[hand[i].Card[1]] > 1)
                {
                    toRemove[i] = true;
                    toRemoveCount++;
                    suitsCount[hand[i].Card[1]]--;
                }
            }

            //-------------------------------------------------------------------
            // CREATE THE HAND MASK/VALUE
            //-------------------------------------------------------------------

            //uint handvalue:
            // b31 (unused) |b30 b29 b28 (Badugi: 0-4) | 7 bits (card value) | 7 bits (card value) | 7 bits (card value) | 7 bits (card value)
            byte badugiCount = (byte)(4 - toRemoveCount);
            handrank = badugiCount;
            handmask = (uint)badugiCount << 28;
            handvalue = handmask;

            int boffset = 21; //Offset for cards that are part of the Badugi hand
            int nboffset = 7 * (toRemoveCount - 1); //Offset for cards that are Not par of the Badugi hand
            for (int i = 0; i < 4; i++)
            {
                if (toRemove[i])
                {
                    handmask = handmask | ((uint)hand[i].Value << nboffset);
                    nboffset -= 7;
                }
                else
                {
                    handmask = handmask | ((uint)hand[i].Value << boffset);
                    //handvalue = handvalue | ( ((uint)0x0000007F - ((uint)hand[i].Value & 0xFFFFFFFC)) << boffset); //strip the suit from card
                    handvalue = handvalue | (((uint)hand[i].Value & 0xFFFFFFFC) << boffset); //strip the suit from card
                    boffset -= 7;
                }
            }
        }
        #endregion

        #region overrides
        public static bool operator ==(BadugiHand x, BadugiHand y)
        {
            return x.HandValue == y.HandValue;
        }

        public static bool operator !=(BadugiHand x, BadugiHand y)
        {
            return x.HandValue != y.HandValue;
        }

        public static bool operator >(BadugiHand x, BadugiHand y)
        {
            return x.HandValue > y.HandValue;
        }

        public static bool operator <(BadugiHand x, BadugiHand y)
        {
            return x.HandValue < y.HandValue;
        }

        public static bool operator >=(BadugiHand x, BadugiHand y)
        {
            return x.HandValue >= y.HandValue;
        }

        public static bool operator <=(BadugiHand x, BadugiHand y)
        {
            return x.HandValue <= y.HandValue;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BadugiHand)
            {
                BadugiHand h = (BadugiHand)obj;
                return this.HandValue == h.HandValue;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return DescriptionFromHandMask(this.HandMask);
        }

        public string Display()
        {
            return (string)hand[0] + (string)hand[1] + (string)hand[2] + (string)hand[3];
        }

        public static implicit operator string(BadugiHand typ)
        {
            return BadugiHand.DescriptionFromHandMask(typ.HandMask);
        }
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is BadugiHand)
            {
                BadugiHand h = (BadugiHand)obj;
                return this.HandValue.CompareTo(h.HandValue);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        #endregion
    }



    public class BadugiCard : IComparable
    {
        private string _Card;
        private byte _Value;
        public string Show { get; set; }

        static Dictionary<string, byte> cards;
        static Dictionary<byte, string> rCards;

        public string Card
        {
            get { return _Card; }
        }

        public byte Value
        {
            get { return _Value; }
        }

        public static BadugiCard CreateCardFromValue(byte v)
        {
            return new BadugiCard(rCards[v]);
        }

        public static implicit operator string(BadugiCard typ)
        {
            return typ.Card;
        }

        #region Card Dictionnary
        static BadugiCard()
        {
            cards = new Dictionary<string, byte>();
            cards.Add("Kc", 0x04); //0000 0100
            cards.Add("Kd", 0x05); //0000 0101
            cards.Add("Kh", 0x06); //0000 0110
            cards.Add("Ks", 0x07); //0000 0111
            cards.Add("Qc", 0x08); //0000 1000
            cards.Add("Qd", 0x09); //0000 1001
            cards.Add("Qh", 0x0A); //0000 1010
            cards.Add("Qs", 0x0B); //0000 1011
            cards.Add("Jc", 0x0C); //0000 1100
            cards.Add("Jd", 0x0D); //0000 1101
            cards.Add("Jh", 0x0E); //0000 1110
            cards.Add("Js", 0x0F); //0000 1111
            cards.Add("Tc", 0x10); //0001 0000
            cards.Add("Td", 0x11);
            cards.Add("Th", 0x12);
            cards.Add("Ts", 0x13);
            cards.Add("9c", 0x14); //0001 0100
            cards.Add("9d", 0x15);
            cards.Add("9h", 0x16);
            cards.Add("9s", 0x17);
            cards.Add("8c", 0x18);
            cards.Add("8d", 0x19);
            cards.Add("8h", 0x1A);
            cards.Add("8s", 0x1B);
            cards.Add("7c", 0x1C);
            cards.Add("7d", 0x1D);
            cards.Add("7h", 0x1E);
            cards.Add("7s", 0x1F);
            cards.Add("6c", 0x20);
            cards.Add("6d", 0x21);
            cards.Add("6h", 0x22);
            cards.Add("6s", 0x23);
            cards.Add("5c", 0x24);
            cards.Add("5d", 0x25);
            cards.Add("5h", 0x26);
            cards.Add("5s", 0x27);
            cards.Add("4c", 0x28);
            cards.Add("4d", 0x29);
            cards.Add("4h", 0x2A);
            cards.Add("4s", 0x2B);
            cards.Add("3c", 0x2C);
            cards.Add("3d", 0x2D);
            cards.Add("3h", 0x2E);
            cards.Add("3s", 0x2F);
            cards.Add("2c", 0x30);
            cards.Add("2d", 0x31);
            cards.Add("2h", 0x32);
            cards.Add("2s", 0x33);
            cards.Add("Ac", 0x34);
            cards.Add("Ad", 0x35);
            cards.Add("Ah", 0x36);
            cards.Add("As", 0x37);

            rCards = new Dictionary<byte, string>();

            rCards.Add(0x04, "Kc");
            rCards.Add(0x05, "Kd");
            rCards.Add(0x06, "Kh");
            rCards.Add(0x07, "Ks");
            rCards.Add(0x08, "Qc");
            rCards.Add(0x09, "Qd");
            rCards.Add(0x0A, "Qh");
            rCards.Add(0x0B, "Qs");
            rCards.Add(0x0C, "Jc");
            rCards.Add(0x0D, "Jd");
            rCards.Add(0x0E, "Jh");
            rCards.Add(0x0F, "Js");
            rCards.Add(0x10, "Tc");
            rCards.Add(0x11, "Td");
            rCards.Add(0x12, "Th");
            rCards.Add(0x13, "Ts");
            rCards.Add(0x14, "9c");
            rCards.Add(0x15, "9d");
            rCards.Add(0x16, "9h");
            rCards.Add(0x17, "9s");
            rCards.Add(0x18, "8c");
            rCards.Add(0x19, "8d");
            rCards.Add(0x1A, "8h");
            rCards.Add(0x1B, "8s");
            rCards.Add(0x1C, "7c");
            rCards.Add(0x1D, "7d");
            rCards.Add(0x1E, "7h");
            rCards.Add(0x1F, "7s");
            rCards.Add(0x20, "6c");
            rCards.Add(0x21, "6d");
            rCards.Add(0x22, "6h");
            rCards.Add(0x23, "6s");
            rCards.Add(0x24, "5c");
            rCards.Add(0x25, "5d");
            rCards.Add(0x26, "5h");
            rCards.Add(0x27, "5s");
            rCards.Add(0x28, "4c");
            rCards.Add(0x29, "4d");
            rCards.Add(0x2A, "4h");
            rCards.Add(0x2B, "4s");
            rCards.Add(0x2C, "3c");
            rCards.Add(0x2D, "3d");
            rCards.Add(0x2E, "3h");
            rCards.Add(0x2F, "3s");
            rCards.Add(0x30, "2c");
            rCards.Add(0x31, "2d");
            rCards.Add(0x32, "2h");
            rCards.Add(0x33, "2s");
            rCards.Add(0x34, "Ac");
            rCards.Add(0x35, "Ad");
            rCards.Add(0x36, "Ah");
            rCards.Add(0x37, "As");

        }
        #endregion

        public BadugiCard(string card)
        {
            if (cards.ContainsKey(card))
            {
                this.Show = card;
                _Value = cards[card];
                _Card = card;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public static bool operator ==(BadugiCard x, BadugiCard y)
        {
            return x.Value == y.Value;
        }

        public static bool operator !=(BadugiCard x, BadugiCard y)
        {
            return x.Value != y.Value;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object o)
        {
            if (o is BadugiCard)
            {
                return this == (BadugiCard)o;
            }
            else
            {
                return false;
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj is BadugiCard)
            {
                BadugiCard bc = (BadugiCard)obj;
                return _Value.CompareTo(bc._Value);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        #endregion
    }
}


//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BadugiHandEval
//{
//    /// <summary>
//    /// Badugi Hand Evaluator
//    /// -------------------------------------------------
//    /// Version: 2.0
//    /// Last revision: 22-11-2008
//    /// Author: Tony Pottier
//    /// Website: http://www.cartridgesoftware.com
//    /// Contact: mailto:support@cartridgesoftware.com
//    /// -------------------------------------------------
//    /// This code is distributed under the LGPL license
//    /// You can use this code in any project, but notify
//    /// me if you make any change.
//    /// <example>
//    /// h = new BadugiHand("Ac", "Th", "3c", "Kc");
//    /// string s = BadugiHand.DescriptionFromHandMask(h.HandMask);
//    /// or s = h.ToString();
//    /// or s = (string)h;
//    /// </example>
//    /// </summary>
//    public class BadugiHand : IComparable
//    {

//        #region declarations
//        private uint handmask = 0x00000000;
//        private uint handvalue = 0x00000000;
//        private int handrank = 0;
//        private BadugiCard[] hand = new BadugiCard[4];
//        private static Dictionary<char, int> suitsCount = new Dictionary<char, int>();
//        #endregion

//        #region properties
//        /// <summary>
//        /// Access the hand mask of this BadugiHand instance.
//        /// </summary>
//        public uint HandMask
//        {
//            get { return handmask; }
//        }

//        public uint HandValue
//        {
//            get { return handvalue; }
//        }

//        /// <summary>
//        /// 4 = Badugi
//        /// 3 = 3-Card
//        /// etc
//        /// </summary>
//        public int HandRank
//        {
//            get { return handrank; }
//        }

//        #endregion

//        #region constructor

//        static BadugiHand()
//        {
//            suitsCount.Add('c', 0);
//            suitsCount.Add('d', 0);
//            suitsCount.Add('h', 0);
//            suitsCount.Add('s', 0);
//        }

//        /// <summary>
//        /// Construct and evaluate a badugi hand
//        /// Cards must be formatted as follow: As Kc Td 3h, etc. etc.
//        /// 10d or KC will throw an exception.
//        /// </summary>
//        public BadugiHand(string card0, string card1, string card2, string card3)
//        {
//            if (card0 != "")
//            {
//                //generate the array used
//                hand[0] = new BadugiCard(card0);
//                hand[1] = new BadugiCard(card1);
//                hand[2] = new BadugiCard(card2);
//                hand[3] = new BadugiCard(card3);

//                //sort it so that we only have to iterate once
//                //through the array
//                Array.Sort(hand);

//                //evaluate
//                this.Evaluate();
//            }
//        }
//        #endregion

//        #region members

//        public static string DescriptionFromHandValue(long hvalue)
//        {
//            return DescriptionFromHandValue((uint)hvalue);
//        }
//        /// <summary>
//        /// Get a complete description from a hand value
//        /// </summary>
//        /// <param name="hvalue">A badugi hand value</param>
//        /// <returns></returns>
//        public static string DescriptionFromHandValue(uint hvalue)
//        {
//            if (hvalue == 0) return "";

//            string s = "";

//            // Badugi/3-card/2-card/1-card
//            uint badugi = hvalue >> 28;
//            if (badugi == 4)
//                s += "Badugi: ";
//            else
//                s += badugi.ToString() + "-card: ";

//            //cards of the badugi hand
//            int offset = 7 * (4 - (int)badugi);
//            for (uint i = 0; i < badugi; i++)
//            {
//                uint ui = hvalue;
//                ui = ui >> offset; //remove cards that are not part of the badugi hand
//                ui = ui >> (int)i * 7; //shift card to LSB
//                ui = ui & 0x0000007F; //remove everything but the card value
//                string card = BadugiCard.CreateCardFromValue( (byte)(0x0000007F - ui));
//                s += card[0];

//                if (i != badugi - 1)
//                    s += ",";
//            }


//            return s;
//        }


//        /// <summary>
//        /// Get a complete description from a mask
//        /// </summary>
//        /// <param name="hvalue">A badugi hand mask</param>
//        /// <returns></returns>
//        public static string DescriptionFromHandMask(uint hmask)
//        {
//            if (hmask == 0) return "";

//            string s = "";

//            // Badugi/3-card/2-card/1-card
//            uint badugi = hmask >> 28;
//            if (badugi == 4)
//                s += "Badugi: ";
//            else
//                s += badugi.ToString() + "-card: ";

//            //cards of the badugi hand
//            int offset = 7 * (4 - (int)badugi);
//            for (uint i = 0; i < badugi; i++)
//            {
//                uint ui = hmask;
//                ui = ui >> offset; //remove cards that are not part of the badugi hand
//                ui = ui >> (int)i * 7; //shift card to LSB
//                ui = ui & 0x0000007F; //remove everything but the card value
//                string card = BadugiCard.CreateCardFromValue((byte)ui);
//                s += card[0];

//                if (i != badugi - 1)
//                    s += ",";
//            }


//            return s;
//        }

//        private void Evaluate()
//        {
//            //declarations
//            bool[] toRemove = new bool[4];
//            int toRemoveCount = 0;

//            //count different suits
//            suitsCount['c'] = 0;
//            suitsCount['d'] = 0;
//            suitsCount['h'] = 0;
//            suitsCount['s'] = 0;
//            for(int i=0;i<4;i++)
//            {
//                suitsCount[hand[i].Card[1]]++;
//                toRemove[i] = false;
//            }


//            //eliminate same card face by chosing one from the highest suit count
//            BadugiCard c = hand[0]; int ci = 0;
//            for (int i = 1; i < 4; i++)
//            {
//                if (!toRemove[i])
//                {
//                    //same face
//                    if (c.Card[0] == hand[i].Card[0])
//                    {
//                        if (suitsCount[c.Card[1]] > suitsCount[hand[i].Card[1]])
//                        {
//                            toRemove[ci] = true;
//                            toRemoveCount++;
//                            suitsCount[c.Card[1]]--;
//                            ci = i;
//                            c = hand[i];
//                        }
//                        else
//                        {
//                            toRemove[i] = true;
//                            toRemoveCount++;
//                            suitsCount[hand[i].Card[1]]--;
//                        }
//                    }
//                    else
//                    {
//                        //not same face?
//                        ci = i;
//                        c = hand[i];
//                    }
//                }
//            }

//            //eliminate same suits
//            for (int i = 0; i < 4; i++)
//            {
//                if (!(toRemove[i]) && suitsCount[hand[i].Card[1]] > 1)
//                {
//                    toRemove[i] = true;
//                    toRemoveCount++;
//                    suitsCount[hand[i].Card[1]]--;
//                }
//            }

//            //-------------------------------------------------------------------
//            // CREATE THE HAND MASK/VALUE
//            //-------------------------------------------------------------------

//            //uint handvalue:
//            // b31 (unused) |b30 b29 b28 (Badugi: 0-4) | 7 bits (card value) | 7 bits (card value) | 7 bits (card value) | 7 bits (card value)
//            byte badugiCount = (byte)(4 - toRemoveCount);
//            handrank = badugiCount;
//            handmask = (uint)badugiCount << 28;
//            handvalue = handmask;

//            int boffset = 21; //Offset for cards that are part of the Badugi hand
//            int nboffset = 7 * (toRemoveCount - 1); //Offset for cards that are Not par of the Badugi hand
//            for (int i = 3; i >= 0; i--)
//            {
//                if (toRemove[i])
//                {
//                    handmask = handmask | ((uint)hand[i].Value << nboffset);
//                    nboffset -= 7;
//                }
//                else
//                {
//                    handmask = handmask | ((uint)hand[i].Value << boffset);
//                    handvalue = handvalue | ( ((uint)0x0000007F - ((uint)hand[i].Value & 0xFFFFFFFC)) << boffset); //strip the suit from card
//                    boffset -= 7;
//                }
//            }
//        }
//#endregion

//        #region overrides
//        public static bool operator ==(BadugiHand x, BadugiHand y)
//        {
//            return x.HandValue == y.HandValue;
//        }

//        public static bool operator !=(BadugiHand x, BadugiHand y)
//        {
//            return x.HandValue != y.HandValue;
//        }

//        public static bool operator >(BadugiHand x, BadugiHand y)
//        {
//            return x.HandValue > y.HandValue;
//        }

//        public static bool operator <(BadugiHand x, BadugiHand y)
//        {
//            return x.HandValue < y.HandValue;
//        }

//        public static bool operator >=(BadugiHand x, BadugiHand y)
//        {
//            return x.HandValue >= y.HandValue;
//        }

//        public static bool operator <=(BadugiHand x, BadugiHand y)
//        {
//            return x.HandValue <= y.HandValue;
//        }

//        public override int GetHashCode()
//        {
//            return base.GetHashCode();
//        }

//        public override bool Equals(object obj)
//        {
//            if (obj is BadugiHand)
//            {
//                BadugiHand h = (BadugiHand)obj;
//                return this.HandValue == h.HandValue;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public override string ToString()
//        {
//            return DescriptionFromHandMask(this.HandMask);
//        }

//        public static implicit operator string(BadugiHand typ)
//        {
//            return BadugiHand.DescriptionFromHandMask(typ.HandMask);
//        }
//        #endregion

//        #region IComparable Members

//        public int CompareTo(object obj)
//        {
//            if (obj is BadugiHand)
//            {
//                BadugiHand h = (BadugiHand)obj;
//                return this.HandValue.CompareTo(h.HandValue);
//            }
//            else
//            {
//                throw new InvalidOperationException();
//            }
//        }

//        #endregion
//    }



//    public class BadugiCard : IComparable
//    {
//        private string _Card;
//        private byte _Value;

//        static Dictionary<string, byte> cards;
//        static Dictionary<byte, string> rCards;

//        public string Card
//        {
//            get { return _Card; }
//        }

//        public byte Value
//        {
//            get { return _Value; }
//        }

//        public static BadugiCard CreateCardFromValue(byte v)
//        {
//            return new BadugiCard(rCards[v]);
//        }

//        public static implicit operator string(BadugiCard typ)
//        {
//            return typ.Card;
//        }

//        #region Card Dictionnary
//        static BadugiCard()
//        {
//            cards = new Dictionary<string, byte>();
//            cards.Add("Kc", 0x04); //0000 0100
//            cards.Add("Kd", 0x05); //0000 0101
//            cards.Add("Kh", 0x06); //0000 0110
//            cards.Add("Ks", 0x07); //0000 0111
//            cards.Add("Qc", 0x08); //0000 1000
//            cards.Add("Qd", 0x09); //0000 1001
//            cards.Add("Qh", 0x0A); //0000 1010
//            cards.Add("Qs", 0x0B); //0000 1011
//            cards.Add("Jc", 0x0C); //0000 1100
//            cards.Add("Jd", 0x0D); //0000 1101
//            cards.Add("Jh", 0x0E); //0000 1110
//            cards.Add("Js", 0x0F); //0000 1111
//            cards.Add("Tc", 0x10); //0001 0000
//            cards.Add("Td", 0x11);
//            cards.Add("Th", 0x12);
//            cards.Add("Ts", 0x13);
//            cards.Add("9c", 0x14); //0001 0100
//            cards.Add("9d", 0x15);
//            cards.Add("9h", 0x16);
//            cards.Add("9s", 0x17);
//            cards.Add("8c", 0x18);
//            cards.Add("8d", 0x19);
//            cards.Add("8h", 0x1A);
//            cards.Add("8s", 0x1B);
//            cards.Add("7c", 0x1C);
//            cards.Add("7d", 0x1D);
//            cards.Add("7h", 0x1E);
//            cards.Add("7s", 0x1F);
//            cards.Add("6c", 0x20);
//            cards.Add("6d", 0x21);
//            cards.Add("6h", 0x22);
//            cards.Add("6s", 0x23);
//            cards.Add("5c", 0x24);
//            cards.Add("5d", 0x25);
//            cards.Add("5h", 0x26);
//            cards.Add("5s", 0x27);
//            cards.Add("4c", 0x28);
//            cards.Add("4d", 0x29);
//            cards.Add("4h", 0x2A);
//            cards.Add("4s", 0x2B);
//            cards.Add("3c", 0x2C);
//            cards.Add("3d", 0x2D);
//            cards.Add("3h", 0x2E);
//            cards.Add("3s", 0x2F);
//            cards.Add("2c", 0x30);
//            cards.Add("2d", 0x31);
//            cards.Add("2h", 0x32);
//            cards.Add("2s", 0x33);
//            cards.Add("Ac", 0x34);
//            cards.Add("Ad", 0x35);
//            cards.Add("Ah", 0x36);
//            cards.Add("As", 0x37);

//            rCards = new Dictionary<byte, string>();

//            rCards.Add(0x04, "Kc");
//            rCards.Add(0x05, "Kd");
//            rCards.Add(0x06, "Kh");
//            rCards.Add(0x07, "Ks");
//            rCards.Add(0x08, "Qc");
//            rCards.Add(0x09, "Qd");
//            rCards.Add(0x0A, "Qh");
//            rCards.Add(0x0B, "Qs");
//            rCards.Add(0x0C, "Jc");
//            rCards.Add(0x0D, "Jd");
//            rCards.Add(0x0E, "Jh");
//            rCards.Add(0x0F, "Js");
//            rCards.Add(0x10, "Tc");
//            rCards.Add(0x11, "Td");
//            rCards.Add(0x12, "Th");
//            rCards.Add(0x13, "Ts");
//            rCards.Add(0x14, "9c");
//            rCards.Add(0x15, "9d");
//            rCards.Add(0x16, "9h");
//            rCards.Add(0x17, "9s");
//            rCards.Add(0x18, "8c");
//            rCards.Add(0x19, "8d");
//            rCards.Add(0x1A, "8h");
//            rCards.Add(0x1B, "8s");
//            rCards.Add(0x1C, "7c");
//            rCards.Add(0x1D, "7d");
//            rCards.Add(0x1E, "7h");
//            rCards.Add(0x1F, "7s");
//            rCards.Add(0x20, "6c");
//            rCards.Add(0x21, "6d");
//            rCards.Add(0x22, "6h");
//            rCards.Add(0x23, "6s");
//            rCards.Add(0x24, "5c");
//            rCards.Add(0x25, "5d");
//            rCards.Add(0x26, "5h");
//            rCards.Add(0x27, "5s");
//            rCards.Add(0x28, "4c");
//            rCards.Add(0x29, "4d");
//            rCards.Add(0x2A, "4h");
//            rCards.Add(0x2B, "4s");
//            rCards.Add(0x2C, "3c");
//            rCards.Add(0x2D, "3d");
//            rCards.Add(0x2E, "3h");
//            rCards.Add(0x2F, "3s");
//            rCards.Add(0x30, "2c");
//            rCards.Add(0x31, "2d");
//            rCards.Add(0x32, "2h");
//            rCards.Add(0x33, "2s");
//            rCards.Add(0x34, "Ac");
//            rCards.Add(0x35, "Ad");
//            rCards.Add(0x36, "Ah");
//            rCards.Add(0x37, "As");

//        }
//        #endregion

//        public BadugiCard(string card)
//        {
//            if (cards.ContainsKey(card))
//            {
//                _Value = cards[card];
//                _Card = card;
//            }
//            else
//            {
//                throw new ArgumentException();
//            }
//        }

//        public static bool operator ==(BadugiCard x, BadugiCard y)
//        {
//            return x.Value == y.Value;
//        }

//        public static bool operator !=(BadugiCard x, BadugiCard y)
//        {
//            return x.Value != y.Value;
//        }


//        public override int GetHashCode()
//        {
//            return base.GetHashCode();
//        }

//        public override bool Equals(object o)
//        {
//            if (o is BadugiCard)
//            {
//                return this == (BadugiCard)o;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        #region IComparable Members

//        public int CompareTo(object obj)
//        {
//            if (obj is BadugiCard)
//            {
//                BadugiCard bc = (BadugiCard)obj;
//                return _Value.CompareTo(bc._Value);
//            }
//            else
//            {
//                throw new InvalidOperationException();
//            }
//        }

//        #endregion
//    }
//}

