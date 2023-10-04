using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Resource = CharaSimResource.Resource;
using WzComparerR2.PluginBase;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
using System.Text.RegularExpressions;
using WzComparerR2.CharaSim;

namespace WzComparerR2.CharaSimControl
{
    public class SetItemTooltipRender : TooltipRender
    {
        public SetItemTooltipRender()
        {
        }

        public SetItem SetItem { get; set; }

        public override object TargetItem
        {
            get { return this.SetItem; }
            set { this.SetItem = value as SetItem; }
        }

        public bool IsCombineProperties { get; set; } = true;

        public override Bitmap Render()
        {
            if (this.SetItem == null)
            {
                return null;
            }

            bool specialPetSetEffectName = this.SetItem.ItemIDs.Parts.Any(p => p.Value.ItemIDs.Any(i => isSpecialPet(i.Key)));

            int width = 261;
            int picHeight1;
            Bitmap originBmp = RenderSetItem(specialPetSetEffectName, out picHeight1);
            int picHeight2 = 0;
            Bitmap effectBmp = null;

            if (this.SetItem.ExpandToolTip)
            {
                effectBmp = RenderEffectPart(specialPetSetEffectName, out picHeight2);
                width += 261;
            }

            Bitmap tooltip = new Bitmap(width, Math.Max(picHeight1, picHeight2));
            Graphics g = Graphics.FromImage(tooltip);

            //绘制左侧
            GearGraphics.DrawNewTooltipBack(g, 0, 0, originBmp.Width, picHeight1);
            g.DrawImage(originBmp, 0, 0, new Rectangle(0, 0, originBmp.Width, picHeight1), GraphicsUnit.Pixel);

            //绘制右侧
            if (effectBmp != null)
            {
                GearGraphics.DrawNewTooltipBack(g, originBmp.Width, 0, effectBmp.Width, picHeight2);
                g.DrawImage(effectBmp, originBmp.Width, 0, new Rectangle(0, 0, effectBmp.Width, picHeight2), GraphicsUnit.Pixel);
            }

            originBmp?.Dispose();
            effectBmp?.Dispose();
            g.Dispose();
            return tooltip;
        }

        private bool isSpecialPet(int itemID)
        {
            if (itemID / 1000000 != 5)
            {
                return false;
            }
            Wz_Node itemNode = PluginBase.PluginManager.FindWz(string.Format(@"Item\Pet\{0:D7}.img", itemID));
            if (itemNode != null)
            {
                var item = Item.CreateFromNode(itemNode, PluginManager.FindWz);
                int value;
                return item.Props.TryGetValue(ItemPropType.wonderGrade, out value) && (value == 1 || value == 4 || value == 5 || value == 6);
            }
            return false;
        }

        private Bitmap RenderSetItem(bool specialPetSetEffectName, out int picHeight)
        {
            var MAX_WIDTH = 261;//default value was 261
            Bitmap setBitmap = new Bitmap(MAX_WIDTH, DefaultPicHeight);
            Graphics g = Graphics.FromImage(setBitmap);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            var fmtItemName = new StringFormat()
            {
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };
            var fmtItemType = new StringFormat() { Alignment = StringAlignment.Far };

            picHeight = 10;
            TextRenderer.DrawText(g, this.SetItem.SetItemName, GearGraphics.EquipDetailFont2, new Point(261, 10), ((SolidBrush)GearGraphics.GreenBrush2).Color, TextFormatFlags.HorizontalCenter);//default value is '261' - Used for set name centered in set effect tooltip window
            picHeight += 25;

            format.Alignment = StringAlignment.Far;
            Wz_Node characterWz = PluginManager.FindWz(Wz_Type.Character);

            if (this.SetItem.SetItemID > 0)
            {
                HashSet<string> partNames = new HashSet<string>();

                foreach (var setItemPart in this.SetItem.ItemIDs.Parts)
                {
                    string itemName = setItemPart.Value.RepresentName;
                    string typeName = setItemPart.Value.TypeName;

                    if (string.IsNullOrEmpty(typeName) && SetItem.Parts)
                    {
                        typeName = "Equip";
                    }

                    ItemBase itemBase = null;
                    bool cash = false;

                    if (setItemPart.Value.ItemIDs.Count > 0)
                    {
                        var itemID = setItemPart.Value.ItemIDs.First().Key;

                        switch (itemID / 1000000)
                        {
                            case 0: //avatar
                            case 1: //gear
                                if (characterWz != null)
                                {
                                    foreach (Wz_Node typeNode in characterWz.Nodes)
                                    {
                                        Wz_Node itemNode = typeNode.FindNodeByPath(string.Format("{0:D8}.img", itemID), true);
                                        if (itemNode != null)
                                        {
                                            var gear = Gear.CreateFromNode(itemNode, PluginManager.FindWz);
                                            cash = gear.Cash;
                                            itemBase = gear;
                                            break;
                                        }
                                    }
                                }
                                break;

                            case 5: //Pet
                                {
                                    Wz_Node itemNode = PluginBase.PluginManager.FindWz(string.Format(@"Item\Pet\{0:D7}.img", itemID));
                                    if (itemNode != null)
                                    {
                                        var item = Item.CreateFromNode(itemNode, PluginManager.FindWz);
                                        cash = item.Cash;
                                        itemBase = item;
                                    }
                                }
                                break;
                        }
                    }

                    if (string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(typeName))
                    {
                        if (setItemPart.Value.ItemIDs.Count > 0)
                        {
                            var itemID = setItemPart.Value.ItemIDs.First().Key;
                            StringResult sr = null; ;
                            if (this.StringLinker != null)
                            {
                                if (this.StringLinker.StringEqp.TryGetValue(itemID, out sr))
                                {
                                    itemName = sr.Name;
                                    if (typeName == null)
                                    {
                                        typeName = ItemStringHelper.GetSetItemGearTypeString(Gear.GetGearType(itemID));
                                    }
                                    switch (Gear.GetGender(itemID))
                                    {
                                        case 0: itemName += " (Male)"; break;
                                        case 1: itemName += " (Female)"; break;
                                    }
                                }
                                else if (this.StringLinker.StringItem.TryGetValue(itemID, out sr)) //兼容宠物
                                {
                                    itemName = sr.Name;
                                    //if (typeName == null)
                                    {
                                        if (itemID / 10000 == 500)
                                        {
                                            typeName = "Pet";
                                        }
                                        else
                                        {
                                            typeName = "";
                                        }
                                    }
                                }
                            }
                            if (sr == null)
                            {
                                itemName = "(null)";
                            }
                        }
                    }

                    itemName = itemName ?? string.Empty;
                    typeName = typeName ?? "Equip";

                    if (!Regex.IsMatch(typeName, @"^(\(.*\)|（.*）|\[.*\])$"))
                    {
                        typeName = "(" + typeName + ")";
                    }

                    if (this.SetItem.Effects.Count > 1 && this.SetItem.ItemIDs.Parts.Count == 1)
                    {
                        typeName += "  [0/3]";
                    }

                    if (!partNames.Contains(itemName + typeName))
                    {
                        partNames.Add(itemName + typeName);
                        Brush brush = setItemPart.Value.Enabled ? Brushes.White : GearGraphics.GrayBrush2;
                        //measure itemType's text width
                        var layoutSize = g.MeasureString(typeName, GearGraphics.EquipDetailFont2, MAX_WIDTH, fmtItemType);//remove: + "..." after typeName to fix ellipsis issue
                        if (!cash)
                        {
                            //TextRenderer.DrawText(g, itemName, GearGraphics.EquipDetailFont2, new Rectangle(10, picHeight, MAX_WIDTH - (int)layoutSize.Width, 18), ((SolidBrush)brush).Color, TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                            //TextRenderer.DrawText(g, typeName, GearGraphics.EquipDetailFont2, new Point(250 - TextRenderer.MeasureText(g, typeName, GearGraphics.EquipDetailFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                            //picHeight += 18;
                            int typeWidth = TextRenderer.MeasureText(g, typeName, GearGraphics.EquipDetailFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width;
                            TextRenderer.DrawText(g, typeName, GearGraphics.EquipDetailFont2, new Point(261 - 10 - typeWidth, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding);
                            TextRenderer.DrawText(g, Compact(g, itemName, 261 - 12 - typeWidth - 10), GearGraphics.EquipDetailFont2, new Point(10, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding);
                            picHeight += 18;
                        }
                        else
                        {
                            g.FillRectangle(GearGraphics.GearIconBackBrush2, 10, picHeight, 36, 36);
                            g.DrawImage(Resource.Item_shadow, 10 + 2 + 3, picHeight + 2 + 32 - 6);
                            if (itemBase?.IconRaw.Bitmap != null)
                            {
                                var icon = itemBase.IconRaw;
                                g.DrawImage(icon.Bitmap, 10 + 2 - icon.Origin.X, picHeight + 2 + 32 - icon.Origin.Y);
                            }
                            g.DrawImage(Resource.CashItem_0, 10 + 2 + 20, picHeight + 2 + 32 - 12);
                            //TextRenderer.DrawText(g, itemName, GearGraphics.EquipDetailFont2, new Point(52, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding);
                            //TextRenderer.DrawText(g, typeName, GearGraphics.EquipDetailFont2, new Point(261 - 10 - TextRenderer.MeasureText(g, typeName, GearGraphics.EquipDetailFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding);
                            int typeWidth = TextRenderer.MeasureText(g, typeName, GearGraphics.EquipDetailFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width;
                            TextRenderer.DrawText(g, typeName, GearGraphics.EquipDetailFont2, new Point(261 - 10 - typeWidth, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding);
                            TextRenderer.DrawText(g, Compact(g, itemName, 261 - 10 - typeWidth - 52), GearGraphics.EquipDetailFont2, new Point(52, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding);
                            if (setItemPart.Value.ByGender)
                            {
                                picHeight += 18;
                                foreach (var itemID in setItemPart.Value.ItemIDs.Keys)
                                {
                                    StringResult sr = null; ;
                                    if (this.StringLinker != null)
                                    {
                                        if (this.StringLinker.StringEqp.TryGetValue(itemID, out sr))
                                        {
                                            itemName = sr.Name;
                                            switch (Gear.GetGender(itemID))
                                            {
                                                case 0: itemName += " (Male)"; break;
                                                case 1: itemName += " (Female)"; break;
                                            }
                                        }
                                        else if (this.StringLinker.StringItem.TryGetValue(itemID, out sr)) //兼容宠物
                                        {
                                            itemName = sr.Name;
                                        }
                                    }
                                    if (sr == null)
                                    {
                                        itemName = "(null)";
                                    }
                                    TextRenderer.DrawText(g, "- " + itemName, GearGraphics.EquipDetailFont2, new Point(61, picHeight), ((SolidBrush)brush).Color, TextFormatFlags.NoPadding);
                                    picHeight += 18;
                                }
                            }
                            else
                            {
                                picHeight += 40;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.SetItem.CompleteCount; ++i)
                {
                    TextRenderer.DrawText(g, "(None)", GearGraphics.EquipDetailFont2, new Point(10, picHeight), ((SolidBrush)GearGraphics.GrayBrush2).Color, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
                    TextRenderer.DrawText(g, "Not Equipped", GearGraphics.EquipDetailFont2, new Point(252 - TextRenderer.MeasureText(g, "Not Equipped", GearGraphics.EquipDetailFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding).Width, picHeight), ((SolidBrush)GearGraphics.GrayBrush2).Color, TextFormatFlags.NoPadding);
                    picHeight += 18;
                }
            }

            if (!this.SetItem.ExpandToolTip)
            {
                picHeight += 5;
                g.DrawLine(Pens.White, 6, picHeight, 254, picHeight);//分割线
                picHeight += 9;
                RenderEffect(g, specialPetSetEffectName, ref picHeight);
            }
            picHeight += 11;

            format.Dispose();
            g.Dispose();
            return setBitmap;
        }

        private static string Compact(Graphics g, string text, int width) // https://www.codeproject.com/Articles/37503/Auto-Ellipsis
        {
            Size s = TextRenderer.MeasureText(g, text, GearGraphics.EquipDetailFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);

            // control is large enough to display the whole text 
            if (s.Width <= width)
                return text;

            int len = 0;
            int seg = text.Length;
            string fit = "";

            // find the longest string that fits into
            // the control boundaries using bisection method 
            while (seg > 1)
            {
                seg -= seg / 2;

                int left = len + seg;

                if (left > text.Length)
                    continue;

                // build and measure a candidate string with ellipsis
                string tst = text.Substring(0, left) + "..";

                s = TextRenderer.MeasureText(g, tst, GearGraphics.EquipDetailFont2, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);

                // candidate string fits into control boundaries, 
                // try a longer string
                // stop when seg <= 1 
                if (s.Width <= width)
                {
                    len += seg;
                    fit = tst;
                }
            }

            return fit;
        }

        private Bitmap RenderEffectPart(bool specialPetSetEffectName, out int picHeight)
        {
            Bitmap effBitmap = new Bitmap(261, DefaultPicHeight);
            Graphics g = Graphics.FromImage(effBitmap);
            picHeight = 9;
            RenderEffect(g, specialPetSetEffectName, ref picHeight);
            picHeight += 11;
            g.Dispose();
            return effBitmap;
        }

        /// <summary>
        /// 绘制套装属性。
        /// </summary>
        private void RenderEffect(Graphics g, bool specialPetSetEffectName, ref int picHeight)
        {
            foreach (KeyValuePair<int, SetItemEffect> effect in this.SetItem.Effects)
            {
                string effTitle;
                if (this.SetItem.SetItemID < 0)
                {
                    effTitle = $"Active Within World({effect.Key} / {this.SetItem.CompleteCount})";
                }
                else if (specialPetSetEffectName && this.SetItem.SetItemName.EndsWith(" Set"))
                {
                    effTitle = $"{Regex.Replace(this.SetItem.SetItemName, " Set$", "")} {effect.Key} set effect";
                }
                else
                {
                    effTitle = effect.Key + " Set Items Equipped";
                }
                TextRenderer.DrawText(g, effTitle, GearGraphics.EquipDetailFont, new Point(10, picHeight), ((SolidBrush)GearGraphics.GreenBrush2).Color, TextFormatFlags.NoPadding);
                picHeight += 15;
                //Brush brush = effect.Value.Enabled ? Brushes.White : GearGraphics.GrayBrush2;
                var color = effect.Value.Enabled ? Color.White : GearGraphics.GrayColor2;

                //T116 合并套装
                var props = IsCombineProperties ? Gear.CombineProperties(effect.Value.PropsV5) : effect.Value.PropsV5;
                foreach (KeyValuePair<GearPropType, object> prop in props)
                {
                    if (prop.Key == GearPropType.Option)
                    {
                        List<Potential> ops = (List<Potential>)prop.Value;
                        foreach (Potential p in ops)
                        {
                            GearGraphics.DrawString(g, p.ConvertSummary(), GearGraphics.EquipDetailFont2, 10, 244, ref picHeight, 15, textColor: color);
                        }
                    }
                    else if (prop.Key == GearPropType.OptionToMob)
                    {
                        List<SetItemOptionToMob> ops = (List<SetItemOptionToMob>)prop.Value;
                        foreach (SetItemOptionToMob p in ops)
                        {
                            GearGraphics.DrawPlainText(g, p.ConvertSummary(), GearGraphics.EquipDetailFont2, color, 10, 244, ref picHeight, 15);
                        }
                    }
                    else if (prop.Key == GearPropType.activeSkill)
                    {
                        List<SetItemActiveSkill> ops = (List<SetItemActiveSkill>)prop.Value;
                        foreach (SetItemActiveSkill p in ops)
                        {
                            StringResult sr;
                            if (StringLinker == null || !StringLinker.StringSkill.TryGetValue(p.SkillID, out sr))
                            {
                                sr = new StringResult();
                                sr.Name = p.SkillID.ToString();
                            }
                            //string summary = "<" + sr.Name.Replace(Environment.NewLine, "") + "> Skill Available";
                            string summary = $"<{sr.Name.Replace(Environment.NewLine, "")}> Skill Available";
                            GearGraphics.DrawPlainText(g, summary, GearGraphics.EquipDetailFont2, color, 10, 244, ref picHeight, 15);
                        }
                    }
                    else if (prop.Key == GearPropType.bonusByTime)
                    {
                        var ops = (List<SetItemBonusByTime>)prop.Value;
                        foreach (SetItemBonusByTime p in ops)
                        {
                            GearGraphics.DrawPlainText(g, $"{p.TermStart} Hr Effect", GearGraphics.EquipDetailFont2, color, 10, 244, ref picHeight, 15);
                            foreach (var bonusProp in p.Props)
                            {
                                var summary = ItemStringHelper.GetGearPropString(bonusProp.Key, Convert.ToInt32(bonusProp.Value));
                                GearGraphics.DrawPlainText(g, summary, GearGraphics.EquipDetailFont2, color, 10, 244, ref picHeight, 15);
                            }
                        }
                    }
                    else
                    {
                        var summary = ItemStringHelper.GetGearPropString(prop.Key, Convert.ToInt32(prop.Value));
                        GearGraphics.DrawPlainText(g, summary, GearGraphics.EquipDetailFont2, color, 10, 244, ref picHeight, 15);
                    }
                }
            }
        }
    }
}