using System;
using System.Collections.Generic;
using System.Text;

using WzComparerR2.WzLib;

namespace WzComparerR2.CharaSim
{
    public class Commodity
    {
        public Commodity()
        {
        }
        public int SN;
        public int ItemId;
        public int Count;
        public int Price;
        public int Bonus;
        public int Period;
        public int Priority;
        public int ReqPOP;
        public int ReqLEV;
        public int Gender;
        public int OnSale;
        public int Class;
        public int Limit;
        public string gameWorld;
        public int LimitMax;
        public int LimitQuestID;
        public int originalPrice;
        public int discount;
        public int PbCash;
        public int PbPoint;
        public int PbGift;
        public int Refundable;
        public int WebShop;
        public int termStart;
        public string termEnd;

        public static Commodity CreateFromNode(Wz_Node commodityNode)
        {
            if (commodityNode == null)
                return null;

            Commodity commodity = new Commodity();

            foreach (Wz_Node subNode in commodityNode.Nodes)
            {
                int value;
                Int32.TryParse(Convert.ToString(subNode.Value), out value);
                switch (subNode.Text)
                {
                    case "SN":
                        commodity.SN = value;
                        break;
                    case "ItemId":
                        commodity.ItemId = value;
                        break;
                    case "Count":
                        commodity.Count = value;
                        break;
                    case "Price":
                        commodity.Price = value;
                        break;
                    case "Bonus":
                        commodity.Bonus = value;
                        break;
                    case "Period":
                        commodity.Period = value;
                        break;
                    case "Priority":
                        commodity.Priority = value;
                        break;
                    case "ReqPOP":
                        commodity.ReqPOP = value;
                        break;
                    case "ReqLEV":
                        commodity.ReqLEV = value;
                        break;
                    case "Gender":
                        commodity.Gender = value;
                        break;
                    case "OnSale":
                        commodity.OnSale = value;
                        break;
                    case "Class":
                        commodity.Class = value;
                        break;
                    case "Limit":
                        commodity.Limit = value;
                        break;
                    case "gameWorld":
                        commodity.gameWorld = Convert.ToString(subNode.Value);
                        break;
                    case "originalPrice":
                        commodity.originalPrice = value;
                        break;
                    case "discount":
                        commodity.discount = value;
                        break;
                    case "PbCash":
                        commodity.PbCash = value;
                        break;
                    case "PbPoint":
                        commodity.PbPoint = value;
                        break;
                    case "PbGift":
                        commodity.PbGift = value;
                        break;
                    case "Refundable":
                        commodity.Refundable = value;
                        break;
                    case "WebShop":
                        commodity.WebShop = value;
                        break;
                    case "termStart":
                        commodity.termStart = value;
                        break;
                    case "termEnd":
                        if (value != 0)
                            commodity.termEnd = string.Format("{0:D8}/{1:D2}0000", value / 100, value % 100);
                        else
                            commodity.termEnd = Convert.ToString(subNode.Value);
                        break;
                }
            }

            return commodity;
        }
    }
}
