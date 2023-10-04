using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Resource = CharaSimResource.Resource;
using WzComparerR2.PluginBase;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
using WzComparerR2.CharaSim;

namespace WzComparerR2.CharaSimControl
{
    public class CashPackageTooltipRender : TooltipRender
    {
        public CashPackageTooltipRender()
        {
        }

        public CashPackage CashPackage { get; set; }

        public override object TargetItem
        {
            get { return this.CashPackage; }
            set { this.CashPackage = value as CashPackage; }
        }

        public override Bitmap Render()
        {
            int picHeight;
            Bitmap originBmp = RenderCashPackage(out picHeight);
            Bitmap tooltip = new Bitmap(originBmp.Width, picHeight);
            Graphics g = Graphics.FromImage(tooltip);

            //绘制背景区域
            GearGraphics.DrawNewTooltipBack(g, 0, 0, tooltip.Width, tooltip.Height);

            //复制图像
            g.DrawImage(originBmp, 0, 0, new Rectangle(0, 0, tooltip.Width, picHeight), GraphicsUnit.Pixel);

            if (originBmp != null)
                originBmp.Dispose();

            if (this.ShowObjectID)
            {
                GearGraphics.DrawGearDetailNumber(g, 3, 3, CashPackage.ItemID.ToString("d8"), true);
            }

            g.Dispose();
            return tooltip;
        }

        private Bitmap RenderCashPackage(out int picH)
        {
            Bitmap cashBitmap = new Bitmap(260, DefaultPicHeight);
            Graphics g = Graphics.FromImage(cashBitmap);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            int totalPrice = 0, totalOriginalPrice = 0;
            Commodity commodityPackage = new Commodity();
            if (CharaSimLoader.LoadedCommoditiesByItemId.ContainsKey(CashPackage.ItemID))
                commodityPackage = CharaSimLoader.LoadedCommoditiesByItemId[CashPackage.ItemID];

            int fullWidth = Math.Max(220, TextRenderer.MeasureText(g, CashPackage.name, GearGraphics.ItemNameFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPrefix).Width + 12 * 2);
            int[] columnWidth = { CashPackage.SN.Count < 8 ? fullWidth : 220, 220, 220 };

            for (int i = 0; i < CashPackage.SN.Count; ++i)
            {
                Commodity commodity = CharaSimLoader.LoadedCommoditiesBySN[CashPackage.SN[i]];
                string name = null;

                StringResult sr = null;
                if (StringLinker != null)
                {
                    if (StringLinker.StringEqp.TryGetValue(commodity.ItemId, out sr))
                    {
                        name = sr.Name;
                    }
                    else if (StringLinker.StringItem.TryGetValue(commodity.ItemId, out sr))
                    {
                        name = sr.Name;
                    }
                    else
                    {
                        name = "(null)";
                    }
                }
                if (sr == null)
                {
                    name = "(null)";
                }

                int nameWidth = TextRenderer.MeasureText(g, name.Replace(Environment.NewLine, ""), GearGraphics.ItemDetailFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix).Width;
                if (commodity.Bonus == 0)
                {
                    if (commodity.originalPrice > 0 && commodity.Price < commodity.originalPrice)
                        nameWidth += 55 + 31 + 6 + 8;
                    else
                        nameWidth += 55 + 8;
                }
                else
                    nameWidth += 55 + 38 + 6 + 8;

                if (CashPackage.SN.Count < 8)
                {
                    columnWidth[0] = Math.Max(columnWidth[0], nameWidth);
                }
                else if (CashPackage.SN.Count < 27)
                {
                    if (i < (CashPackage.SN.Count + 1) / 2)
                        columnWidth[0] = Math.Max(columnWidth[0], nameWidth);
                    else
                        columnWidth[1] = Math.Max(columnWidth[1], nameWidth);
                }
                else
                {
                    if (i < (CashPackage.SN.Count + 2) / 3)
                        columnWidth[0] = Math.Max(columnWidth[0], nameWidth);
                    else if (i < (2 * CashPackage.SN.Count + 2) / 3)
                        columnWidth[1] = Math.Max(columnWidth[1], nameWidth);
                    else
                        columnWidth[2] = Math.Max(columnWidth[2], nameWidth);
                }
            }

            if (CashPackage.SN.Count < 8)
                fullWidth = Math.Max(fullWidth, columnWidth[0]);
            else if (CashPackage.SN.Count < 27)
                fullWidth = Math.Max(fullWidth, columnWidth[0] + columnWidth[1] - 4);
            else
                fullWidth = Math.Max(fullWidth, columnWidth[0] + columnWidth[1] + columnWidth[2] - 8);

            if (fullWidth > 220)
            {
                //重构大小
                g.Dispose();
                cashBitmap.Dispose();

                cashBitmap = new Bitmap(fullWidth, DefaultPicHeight);
                g = Graphics.FromImage(cashBitmap);
            }

            picH = 10;
            TextRenderer.DrawText(g, CashPackage.name, GearGraphics.ItemNameFont2, new Point(cashBitmap.Width, picH), Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPrefix);
            picH += 14;
            if (commodityPackage.termStart > 0 || commodityPackage.termEnd != null)
            {
                string term = "";
                if (commodityPackage.termStart > 0)
                    //term += string.Format("{1:D2}/{2:D2}/{0:D2} {3:D2}:00 ", commodityPackage.termStart / 1000000, (commodityPackage.termStart / 10000) % 100, (commodityPackage.termStart / 100) % 100);
                    //term += string.Format("{1:D2}/{2}/{0} {3}:00:00", commodityPackage.termStart / 1000000, (commodityPackage.termStart / 10000) % 100, (commodityPackage.termStart / 100) % 100);
                term += string.Format("{1:D2}/{2}/{0} {3}:00:00", commodityPackage.termStart / 1000000, (commodityPackage.termStart / 10000) % 100, (commodityPackage.termStart / 100) % 100, commodityPackage.termStart % 100);
                //term += "-";
                if (commodityPackage.termStart > 0 && commodityPackage.termEnd != null)
                    term += "\n~";
                else
                    term += " ~";

                if (commodityPackage.termEnd != null)
                {
                    int termEndDate = Convert.ToInt32(commodityPackage.termEnd.Split('/')[0]);
                    int termEndTime = Convert.ToInt32(commodityPackage.termEnd.Split('/')[1]);
                    term += string.Format(" {1:D2}/{2}/{0} {3:D2}:{4:D2}:{5:D2} UTC", termEndDate / 10000, (termEndDate / 100) % 100, termEndDate % 100, termEndTime / 10000, (termEndTime / 100) % 100, termEndTime % 100);
                }
                //term += " >";

                picH += 8;
                //term += " >";
                TextRenderer.DrawText(g, term, GearGraphics.ItemDetailFont2, new Point(cashBitmap.Width, picH), ((SolidBrush)GearGraphics.OrangeBrush4).Color, TextFormatFlags.HorizontalCenter);
                picH += 16 * term.Split('\n').Length;
                //picH += 12; < --- commented because of line above, check!
            }
            if (commodityPackage.Limit > 0)
            {
                string limit = null;
                switch (commodityPackage.Limit)
                {
                    case 2:
                        //Max Purchase
                        break;
                    case 3:
                        limit = "Purchase Limit per Nexon ID";
                        break;
                    case 4:
                        limit = "Character Limited Sale";
                        break;
                    default:
                        limit = commodityPackage.Limit.ToString();
                        break;
                }
                if (limit != null && limit.Length > 0)
                {
                    TextRenderer.DrawText(g, "<" + limit + ">", GearGraphics.ItemDetailFont2, new Point(cashBitmap.Width, picH), ((SolidBrush)GearGraphics.OrangeBrush4).Color, TextFormatFlags.HorizontalCenter);
                    picH += 12;
                }
            }
            picH += 19;

            int right = cashBitmap.Width - 18;
            if (CashPackage.desc != null && CashPackage.desc.Length > 0)
                CashPackage.desc += "";
            CashPackage.desc += "\n";
            if (CashPackage.onlyCash == 0)
                GearGraphics.DrawString(g, CashPackage.desc + "", GearGraphics.ItemDetailFont2, 11, right, ref picH, 16);
            //GearGraphics.DrawString(g, CashPackage.desc + "\n#(Not applicable to free bonus items) Buy this with Nexon Cash and you can trade it with another user once if unused.", GearGraphics.ItemDetailFont2, 11, right, ref picH, 16);
            else
                GearGraphics.DrawString(g, CashPackage.desc + "\n#Can only be purchased with NX.#", GearGraphics.ItemDetailFont2, 11, right, ref picH, 16);

            bool hasLine = false;
            picH -= 0;//default is 4

            int picStartH = picH, picEndH = 0, columnLeft = 0, columnRight = columnWidth[0];

            for (int i = 0; i < CashPackage.SN.Count; ++i)
            {
                if (CashPackage.SN.Count >= 8 && CashPackage.SN.Count < 27)
                {
                    if (i == (CashPackage.SN.Count + 1) / 2)
                    {
                        hasLine = false;
                        picEndH = picH;
                        picH = picStartH;
                        columnLeft = columnWidth[0] - 2;
                        columnRight = columnWidth[0] + columnWidth[1] - 4;
                    }
                }
                else if (CashPackage.SN.Count >= 27)
                {
                    if (i == (CashPackage.SN.Count + 2) / 3)
                    {
                        hasLine = false;
                        picEndH = picH;
                        picH = picStartH;
                        columnLeft = columnWidth[0] - 2;
                        columnRight = columnWidth[0] + columnWidth[1] - 4;
                    }
                    else if (i == (2 * CashPackage.SN.Count + 2) / 3)
                    {
                        hasLine = false;
                        picEndH = picH;
                        picH = picStartH;
                        columnLeft = columnWidth[0] + columnWidth[1] - 6;
                        columnRight = columnWidth[0] + columnWidth[1] + columnWidth[2] - 8;
                    }
                }

                if (hasLine)
                {
                    g.DrawImage(Resource.CSDiscount_Line, columnLeft + 13, picH);
                    picH += 1;
                }

                Commodity commodity = CharaSimLoader.LoadedCommoditiesBySN[CashPackage.SN[i]];
                string name = null, info = null, time = null;
                BitmapOrigin IconRaw = new BitmapOrigin();

                StringResult sr = null;
                if (StringLinker != null)
                {
                    Wz_Node iconNode = null;
                    if (StringLinker.StringEqp.TryGetValue(commodity.ItemId, out sr))
                    {
                        name = sr.Name;
                        string[] fullPaths = sr.FullPath.Split('\\');
                        iconNode = PluginBase.PluginManager.FindWz(string.Format(@"Character\{0}\{1:D8}.img\info\iconRaw", String.Join("\\", new List<string>(fullPaths).GetRange(2, fullPaths.Length - 3).ToArray()), commodity.ItemId));
                    }
                    else if (StringLinker.StringItem.TryGetValue(commodity.ItemId, out sr))
                    {
                        name = sr.Name;
                        if (Regex.IsMatch(sr.FullPath, @"^(Cash|Consume|Etc|Ins).img\\.+$"))
                        {
                            string itemType = null;
                            if (Regex.IsMatch(sr.FullPath, @"^Cash.img\\.+$"))
                                itemType = "Cash";
                            else if (Regex.IsMatch(sr.FullPath, @"^Consume.img\\.+$"))
                                itemType = "Consume";
                            else if (Regex.IsMatch(sr.FullPath, @"^Etc.img\\.+$"))
                                itemType = "Etc";
                            else if (Regex.IsMatch(sr.FullPath, @"^Ins.img\\.+$"))
                                itemType = "Install";
                            iconNode = PluginBase.PluginManager.FindWz(string.Format(@"Item\{0}\{1:D4}.img\{2:D8}\info\iconRaw", itemType, commodity.ItemId / 10000, commodity.ItemId));
                        }
                        else if (Regex.IsMatch(sr.FullPath, @"^Pet.img\\.+$"))
                        {
                            iconNode = PluginBase.PluginManager.FindWz(string.Format(@"Item\Pet\{0:D7}.img\info\iconRaw", commodity.ItemId));
                        }
                    }
                    else
                    {
                        name = "(null)";
                    }
                    if (iconNode != null)
                    {
                        IconRaw = BitmapOrigin.CreateFromNode(iconNode, PluginBase.PluginManager.FindWz);
                    }
                }
                if (sr == null)
                {
                    name = "(null)";
                }

                if (commodity.Bonus == 0)
                {
                    if (commodity.Count > 1)
                        info += "(" + commodity.Count + ") ";//count (개)
                    if (commodity.originalPrice == 0)
                    {
                        foreach (var commodity2 in CharaSimLoader.LoadedCommoditiesBySN.Values)
                        {
                            if (commodity2.ItemId == commodity.ItemId && commodity2.Count == commodity.Count && commodity2.Period == commodity.Period && commodity2.gameWorld == commodity.gameWorld && commodity2.Price > commodity.originalPrice)
                                commodity.originalPrice = commodity2.Price;
                        }
                        if (commodity.originalPrice == commodity.Price)
                            commodity.originalPrice = 0;
                    }
                    if (commodity.originalPrice > 0 && commodity.Price < commodity.originalPrice)
                    {
                        info += commodity.originalPrice + " NX        "; // HERE is making space between original price and discounted price
                        totalOriginalPrice += commodity.originalPrice;
                    }
                    else
                    {
                        totalOriginalPrice += commodity.Price;
                    }
                    info += commodity.Price + " NX";
                    totalPrice += commodity.Price;
                }
                else
                {
                    info += "(" + commodity.Count + ") ";//count (개)
                    if (commodity.originalPrice > 0)
                    {
                        info += commodity.originalPrice + " NX";
                        totalOriginalPrice += commodity.originalPrice;
                    }
                    else
                    {
                        info += commodity.Price + " NX";
                        totalOriginalPrice += commodity.Price;
                    }
                }

                if (commodity.Period > 0)
                {
                    time = "AVAILABLE FOR " + commodity.Period + " DAYS.";
                }

                g.DrawImage(Resource.CSDiscount_backgrnd, columnLeft + 13, picH + 12);
                if (IconRaw.Bitmap != null)
                {
                    g.DrawImage(IconRaw.Bitmap, columnLeft + 13 + 1 - IconRaw.Origin.X, picH + 12 + 33 - IconRaw.Origin.Y);
                }
                if (time == null)
                {
                    TextRenderer.DrawText(g, name.TrimEnd(Environment.NewLine.ToCharArray()), GearGraphics.ItemDetailFont, new Point(columnLeft + 55, picH + 17), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                    if (commodity.Bonus == 0)
                    {
                        TextRenderer.DrawText(g, info, GearGraphics.ItemDetailFont, new Point(columnLeft + 55, picH + 33), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                        if (commodity.originalPrice > 0 && commodity.Price < commodity.originalPrice)
                        {
                            int width = TextRenderer.MeasureText(g, info.Substring(0, info.IndexOf("      ")), GearGraphics.ItemDetailFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width;
                            g.DrawLine(Pens.White, columnLeft + 55, picH + 33 + 4, columnLeft + 55 + width + 1, picH + 33 + 4);
                            g.DrawImage(Resource.CSDiscount_arrow, columnLeft + 55 + width + 10, picH + 33 + 1);
                            DrawDiscountNum(g, "-" + (int)(100 - 100.0 * commodity.Price / commodity.originalPrice) + "%", columnRight - 40, picH + 16, StringAlignment.Near);
                        }
                    }
                    else
                    {
                        TextRenderer.DrawText(g, info, GearGraphics.ItemDetailFont, new Point(columnLeft + 55, picH + 33), Color.Red, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                        g.DrawImage(Resource.CSDiscount_bonus, columnRight - 47, picH + 29);
                    }
                }
                else
                {
                    TextRenderer.DrawText(g, name.Replace(Environment.NewLine, ""), GearGraphics.ItemDetailFont, new Point(columnLeft + 55, picH + 8), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                    if (commodity.Bonus == 0)
                    {
                        TextRenderer.DrawText(g, info, GearGraphics.ItemDetailFont, new Point(columnLeft + 55, picH + 24), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                        if (commodity.originalPrice > 0 && commodity.Price < commodity.originalPrice)
                        {
                            int width = TextRenderer.MeasureText(g, info.Substring(0, info.IndexOf("      ")), GearGraphics.ItemDetailFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width;
                            g.DrawLine(Pens.White, columnLeft + 55, picH + 24 + 4, columnLeft + 55 + width + 1, picH + 24 + 4);
                            g.DrawImage(Resource.CSDiscount_arrow, columnLeft + 55 + width + 10, picH + 24 + 1);
                            DrawDiscountNum(g, "-" + (int)(100 - 100.0 * commodity.Price / commodity.originalPrice) + "%", columnRight - 40, picH + 7, StringAlignment.Near);
                        }
                    }
                    else
                    {
                        TextRenderer.DrawText(g, info, GearGraphics.ItemDetailFont, new Point(columnLeft + 55, picH + 24), Color.Red, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                        g.DrawImage(Resource.CSDiscount_bonus, columnRight - 47, picH + 20);
                    }
                    TextRenderer.DrawText(g, time, GearGraphics.ItemDetailFont, new Point(columnLeft + 55, picH + 39), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                }
                picH += 57;

                hasLine = true;
            }

            if (picEndH != 0)
                picH = picEndH;

            g.DrawLine(Pens.White, 13, picH, cashBitmap.Width - 8, picH);
            picH += 11;

            g.DrawImage(Resource.CSDiscount_total, 9, picH + 1);
            if (totalOriginalPrice == totalPrice)
            {
                TextRenderer.DrawText(g, totalPrice + " NX", GearGraphics.ItemDetailFont, new Point(35, picH), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
            }
            else
            {
                TextRenderer.DrawText(g, totalOriginalPrice + " NX      " + totalPrice + " NX", GearGraphics.ItemDetailFont, new Point(35, picH), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                TextRenderer.DrawText(g, totalOriginalPrice + " NX", GearGraphics.ItemDetailFont, new Point(35, picH), Color.Red, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                g.DrawImage(Resource.CSDiscount_arrow, 35 + TextRenderer.MeasureText(g, totalOriginalPrice + " NX", GearGraphics.ItemDetailFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width + 5, picH + 1);
                DrawDiscountNum(g, "-" + (int)((100 - 100.0 * totalPrice / totalOriginalPrice)) + "%", cashBitmap.Width - 40, picH - 1, StringAlignment.Near);
            }
            picH += 11;

            picH += 13;
            format.Dispose();
            g.Dispose();
            return cashBitmap;
        }

        private void DrawDiscountNum(Graphics g, string numString, int x, int y, StringAlignment align)
        {
            if (g == null || numString == null)
                return;
            bool near = align == StringAlignment.Near;

            for (int i = 0; i < numString.Length; i++)
            {
                char c = near ? numString[i] : numString[numString.Length - i - 1];
                Image image = null;
                Point origin = Point.Empty;
                switch (c)
                {
                    case '-':
                        image = Resource.ResourceManager.GetObject("CSDiscount_w") as Image;
                        break;
                    case '%':
                        image = Resource.ResourceManager.GetObject("CSDiscount_e") as Image;
                        break;
                    default:
                        if ('0' <= c && c <= '9')
                        {
                            image = Resource.ResourceManager.GetObject("CSDiscount_" + c) as Image;
                        }
                        break;
                }

                if (image != null)
                {
                    if (near)
                    {
                        g.DrawImage(image, x + origin.X, y + origin.Y);
                        x += image.Width + origin.X;
                    }
                    else
                    {
                        x -= image.Width + origin.X;
                        g.DrawImage(image, x + origin.X, y + origin.Y);
                    }
                }
            }
        }
    }
}