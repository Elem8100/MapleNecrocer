using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WzComparerR2.CharaSim
{
    public static class ItemStringHelper
    {
        /// <summary>
        /// 获取怪物category属性对应的类型说明。
        /// </summary>
        /// <param Name="category">怪物的category属性的值。</param>
        /// <returns></returns>
        public static string GetMobCategoryName(int category)
        {
            switch (category)
            {
                case 0: return "No type";
                case 1: return "Mammal type";
                case 2: return "Plant type";
                case 3: return "Fish type";
                case 4: return "Reptile type";
                case 5: return "Spirit type";
                case 6: return "Devil type";
                case 7: return "Undead type";
                case 8: return "Enchanted type";
                default: return null;
            }
        }

        public static string GetGearPropString(GearPropType propType, int value)
        {
            return GetGearPropString(propType, value, 0);
        }

        /// <summary>
        /// 获取GearPropType所对应的文字说明。
        /// </summary>
        /// <param Name="propType">表示装备属性枚举GearPropType。</param>
        /// <param Name="Value">表示propType属性所对应的值。</param>
        /// <returns></returns>
        public static string GetGearPropString(GearPropType propType, int value, int signFlag)
        {

            string sign;
            switch (signFlag)
            {
                default:
                case 0: //默认处理符号
                    sign = value > 0 ? "+" : null;
                    break;

                case 1: //固定加号
                    sign = "+";
                    break;

                case 2: //无特别符号
                    sign = "";
                    break;
            }
            switch (propType)
            {
                case GearPropType.incSTR: return "STR : " + sign + value;
                case GearPropType.incSTRr: return "STR : " + sign + value + "%";
                case GearPropType.incDEX: return "DEX : " + sign + value;
                case GearPropType.incDEXr: return "DEX : " + sign + value + "%";
                case GearPropType.incINT: return "INT : " + sign + value;
                case GearPropType.incINTr: return "INT : " + sign + value + "%";
                case GearPropType.incLUK: return "LUK : " + sign + value;
                case GearPropType.incLUKr: return "LUK : " + sign + value + "%";
                case GearPropType.incAllStat: return "All Stats: " + sign + value;
                case GearPropType.statR: return "All Stats: " + sign + value + "%";
                case GearPropType.incMHP: return "MaxHP : " + sign + value;
                case GearPropType.incMHPr: return "MaxHP : " + sign + value + "%";
                case GearPropType.incMMP: return "MaxMP : " + sign + value;
                case GearPropType.incMMPr: return "MaxMP : " + sign + value + "%";
                case GearPropType.incMDF: return "MaxDF : " + sign + value;
                case GearPropType.incPAD: return "Attack Power: " + sign + value;
                case GearPropType.incPADr: return "Attack Power: " + sign + value + "%";
                case GearPropType.incMAD: return "Magic Attack: " + sign + value;
                case GearPropType.incMADr: return "Magic Attack: " + sign + value + "%";
                case GearPropType.incPDD: return "Defense: " + sign + value;
                case GearPropType.incPDDr: return "Defense: " + sign + value + "%";
                //case GearPropType.incMDD: return "MAGIC DEF. : " + sign + value;
                //case GearPropType.incMDDr: return "MAGIC DEF. : " + sign + value + "%";
                //case GearPropType.incACC: return "ACCURACY : " + sign + value;
                //case GearPropType.incACCr: return "ACCURACY : " + sign + value + "%";
                //case GearPropType.incEVA: return "AVOIDABILITY : " + sign + value;
                //case GearPropType.incEVAr: return "AVOIDABILITY : " + sign + value + "%";
                case GearPropType.incSpeed: return "Speed: " + sign + value;
                case GearPropType.incJump: return "Jump: " + sign + value;
                case GearPropType.incCraft: return "Diligence: " + sign + value;
                case GearPropType.damR:
                case GearPropType.incDAMr: return "Damage: " + sign + value + "%";
                case GearPropType.incCr: return "Critical Rate: " + sign + value + "%";
                case GearPropType.incCDr: return "Critical Damage: " + sign + value + "%";
                case GearPropType.knockback: return "Knockback Chance: " + value + "%";
                //case GearPropType.incPVPDamage: return "Battle Mode ATT " + sign + " " + value;
                case GearPropType.incPQEXPr: return "Party Quest EXP: +" + value + "%";
                case GearPropType.incEXPr: return "Party EXP: +" + value + "%";
                case GearPropType.incBDR:
                case GearPropType.bdR: return "Boss Damage: +" + value + "%";
                case GearPropType.incIMDR:
                case GearPropType.imdR: return "Ignored Enemy DEF: +" + value + "%";
                case GearPropType.limitBreak: return "Damage Cap: " + value;
                case GearPropType.reduceReq: return "Required Level: -" + value;
                case GearPropType.nbdR: return "Damage Against Normal Monsters: +" + value + "%"; //KMST 1069

                case GearPropType.only: return value == 0 ? null : "One-of-a-kind Item";
                case GearPropType.tradeBlock: return value == 0 ? null : "Untradable";
                case GearPropType.equipTradeBlock: return value == 0 ? null : "Cannot be Traded when equipped";
                case GearPropType.accountSharable: return value == 0 ? null : "Account-bound. Transferable within world."; //v218 Transferable within world
                case GearPropType.sharableOnce: return value == 0 ? null : "Tradable once within the same world.\n(Cannot be traded after transfer)"; //old "Can be traded once within account"
                case GearPropType.onlyEquip: return value == 0 ? null : "Unique Equipped Item";
                case GearPropType.notExtend: return value == 0 ? null : "Duration cannot be extended.";
                case GearPropType.tradeAvailable:
                    switch (value)
                    {
                        case 1: return "#cUse the Scissors of Karma to enable this item to be traded one time.#";
                        case 2: return "#cUse the Platinum Scissors of Karma to\n\renable this item to be traded one time.#";
                        default: return null;
                    }
                case GearPropType.accountShareTag:
                    switch (value)
                    {
                        case 1: return "#cUse the Sharing Tag to move an item to another character on the same account once.#";
                        default: return null;
                    }
                //case GearPropType.noPotential: return value == 0 ? null : "This item cannot gain Potential.";
                case GearPropType.fixedPotential: return value == 0 ? null : "Potential Reset Not Allowed";
                case GearPropType.superiorEqp: return value == 0 ? null : "Allows you to gain even higher stats with successful item enhancement.";
                case GearPropType.nActivatedSocket: return value == 0 ? null : "#cYou can mount a Nebulite on this item#";
                case GearPropType.jokerToSetItem: return value == 0 ? null : "\n#cThis lucky item counts towards any set,\n\rso long as you have at least 3 set pieces equipped!#";//\n\r#cThis lucky...
                case GearPropType.plusToSetItem: return value == 0 ? null : "#cWhen equipped, the item set will count as having equipped two.#";
                case GearPropType.abilityTimeLimited: return value == 0 ? null : "Limited Time Stats";
                case GearPropType.blockGoldHammer: return value == 0 ? null : "Golden Hammer cannot be used.";
                case GearPropType.colorvar: return value == 0 ? null : "#cThis item can be dyed using a Dye.#";
                case GearPropType.cantRepair: return value == 0 ? null : "Cannot be repaired";
                case GearPropType.noLookChange: return value == 0 ? null : "Cannot use Fusion Anvil";

                case GearPropType.incAllStat_incMHP25: return "All Stats: " + sign + value + ", MaxHP : " + sign + (value * 25);// check once Lv 250 set comes out in GMS
                case GearPropType.incAllStat_incMHP50_incMMP50: return "All Stats: " + sign + value + ", MaxHP / MaxMP : " + sign + (value * 50);
                case GearPropType.incMHP_incMMP: return "MaxHP / MaxMP: " + sign + value;
                case GearPropType.incMHPr_incMMPr: return "MaxHP / MaxMP: " + sign + value + "%";
                case GearPropType.incPAD_incMAD:
                case GearPropType.incAD: return "Attack Power && Magic ATT: " + sign + value;
                case GearPropType.incPDD_incMDD: return "Defense: " + sign + value;
                //case GearPropType.incACC_incEVA: return "ACC/AVO :" + sign + value;

                case GearPropType.incARC: return "ARC : " + sign + value;
                case GearPropType.incAUT: return "SAC : " + sign + value;
                default: return null;
            }
        }


        public static string GetGearPropDiffString(GearPropType propType, int value, int standardValue)
        {
            var propStr = GetGearPropString(propType, value);
            if (value > standardValue)
            {
                string subfix = null;
                switch (propType)
                {
                    case GearPropType.incSTR:
                    case GearPropType.incDEX:
                    case GearPropType.incINT:
                    case GearPropType.incLUK:
                    case GearPropType.incMHP:
                    case GearPropType.incMMP:
                    case GearPropType.incMDF:
                    case GearPropType.incARC:
                    case GearPropType.incAUT:
                    case GearPropType.incPAD:
                    case GearPropType.incMAD:
                    case GearPropType.incPDD:
                    case GearPropType.incMDD:
                    case GearPropType.incSpeed:
                    case GearPropType.incJump:
                        subfix = $"({standardValue} #$+{value - standardValue}#)"; break;

                    case GearPropType.bdR:
                    case GearPropType.incBDR:
                    case GearPropType.imdR:
                    case GearPropType.incIMDR:
                    case GearPropType.damR:
                    case GearPropType.incDAMr:
                    case GearPropType.statR:
                        subfix = $"({standardValue}% #$+{value - standardValue}%#)"; break;
                }
                propStr = "#$" + propStr + "# " + subfix;
            }
            return propStr;
        }

        /// <summary>
        /// 获取gearGrade所对应的字符串。
        /// </summary>
        /// <param Name="rank">表示装备的潜能等级GearGrade。</param>
        /// <returns></returns>
        public static string GetGearGradeString(GearGrade rank)
        {
            switch (rank)
            {
                //case GearGrade.C: return "C级(一般物品)";
                case GearGrade.B: return "(Rare Item)";
                case GearGrade.A: return "(Epic Item)";
                case GearGrade.S: return "(Unique Item)";
                case GearGrade.SS: return "(Legendary Item)";
                case GearGrade.Special: return "(Special Item)";
                default: return null;
            }
        }

        /// <summary>
        /// 获取gearType所对应的字符串。
        /// </summary>
        /// <param Name="Type">表示装备类型GearType。</param>
        /// <returns></returns>
        public static string GetGearTypeString(GearType type)
        {
            switch (type)
            {
                //case GearType.body: return "Avatar (Body)";
                case GearType.head: return "Avatar (Head)";
                case GearType.face:
                case GearType.face2: return "Face";
                case GearType.hair:
                case GearType.hair2:
                case GearType.hair3: return "Hair";
                case GearType.faceAccessory: return "FACE ACCESSORY";
                case GearType.eyeAccessory: return "EYE ACCESSORY";
                case GearType.earrings: return "EARRINGS";
                case GearType.pendant: return "PENDANT";
                case GearType.belt: return "BELT";
                case GearType.medal: return "MEDAL";
                case GearType.shoulderPad: return "SHOULDER";
                case GearType.cap: return "Hat";
                case GearType.cape: return "Cape";
                case GearType.coat: return "Top";
                case GearType.dragonMask: return "Dragon Hat";
                case GearType.dragonPendant: return "Dragon Pendant";
                case GearType.dragonWings: return "Dragon Wing Accessory";
                case GearType.dragonTail: return "Dragon Tail Accessory";
                case GearType.glove: return "GLOVES";
                case GearType.longcoat: return "Outfit";
                case GearType.machineEngine: return "Mechanic Engine";
                case GearType.machineArms: return "Mechanic Arm";
                case GearType.machineLegs: return "Mechanic Leg";
                case GearType.machineBody: return "Mechanic Frame";
                case GearType.machineTransistors: return "Mechanic Transistor";
                case GearType.pants: return "Bottom";
                case GearType.ring: return "RING";
                case GearType.shield: return "Shield";
                case GearType.shoes: return "Shoes";
                case GearType.shiningRod: return "Shining Rod";
                case GearType.soulShooter: return "Soul Shooter";
                case GearType.ohSword: return "One-handed Sword";
                case GearType.ohAxe: return "One-handed Axe";
                case GearType.ohBlunt: return "One-handed Blunt Weapon";
                case GearType.dagger: return "Dagger";
                case GearType.katara: return "Katara";
                case GearType.magicArrow: return "Magic Arrow";
                case GearType.card: return "Card";
                case GearType.box: return "Core";
                case GearType.orb: return "Orb";
                case GearType.novaMarrow: return "Dragon Essence";
                case GearType.soulBangle: return "Soul Ring";
                case GearType.mailin: return "Magnum";
                case GearType.cane: return "Cane";
                case GearType.wand: return "Wand";
                case GearType.staff: return "Staff";
                case GearType.thSword: return "Two-handed Sword";
                case GearType.thAxe: return "Two-handed Axe";
                case GearType.thBlunt: return "Two-handed Blunt Weapon";
                case GearType.spear: return "Spear";
                case GearType.polearm: return "Polearm";
                case GearType.bow: return "Bow";
                case GearType.crossbow: return "Crossbow";
                case GearType.throwingGlove: return "Claw";
                case GearType.knuckle: return "Knuckle";
                case GearType.gun: return "Gun";
                case GearType.android: return "ANDROID";
                case GearType.machineHeart: return "MECHANICAL HEART";
                case GearType.pickaxe: return "Mining Tool";
                case GearType.shovel: return "Herbalism Tool";
                case GearType.pocket: return "POCKET ITEM";
                case GearType.dualBow: return "Dual Bowguns";
                case GearType.handCannon: return "Hand Cannon";
                case GearType.badge: return "BADGE";
                case GearType.emblem: return "EMBLEM";
                case GearType.soulShield: return "Soul Shield";
                case GearType.demonShield: return "Demon Aegis";
                //case GearType.totem: return "Totem";
                case GearType.petEquip: return "Pet Equipment";
                case GearType.taming:
                case GearType.taming2:
                case GearType.taming3: 
                case GearType.tamingChair: return "TAMED MONSTER";
                case GearType.saddle: return "Saddle";
                case GearType.katana: return "Katana";
                case GearType.fan: return "Fan";
                case GearType.swordZB: return "Heavy Sword";
                case GearType.swordZL: return "Long Sword";
                case GearType.weapon: return "Weapon";
                case GearType.subWeapon: return "Secondary Weapon";
                case GearType.heroMedal: return "Medallions";
                case GearType.rosario: return "Rosary";
                case GearType.chain: return "Iron Chain";
                case GearType.book1:
                case GearType.book2:
                case GearType.book3: return "Magic Book";
                case GearType.bowMasterFeather: return "Arrow Fletching";
                case GearType.crossBowThimble: return "Bow Thimble";
                case GearType.shadowerSheath: return "Dagger Scabbard";
                case GearType.nightLordPoutch: return "Charm";
                case GearType.viperWristband: return "Wrist Band";
                case GearType.captainSight: return "Far Sight";
                case GearType.cannonGunPowder:
                case GearType.cannonGunPowder2: return "Powder Keg";
                case GearType.aranPendulum: return "Mass";
                case GearType.evanPaper: return "Document";
                case GearType.battlemageBall: return "Magic Marble";
                case GearType.wildHunterArrowHead: return "Arrowhead";
                case GearType.cygnusGem: return "Jewel";
                case GearType.controller: return "Controller";
                case GearType.foxPearl: return "Fox Marble";
                case GearType.chess: return "Chess Piece";
                case GearType.powerSource: return "Power Source";

                case GearType.energySword: return "Whip Blade";
                case GearType.desperado: return "Desperado";
                case GearType.magicStick: return "Beast Tamer Scepter";
                case GearType.whistle:
                case GearType.whistle2: return "Whistle";
                case GearType.boxingClaw: return "Fist";
                case GearType.kodachi:
                case GearType.kodachi2: return "Kodachi";
                case GearType.espLimiter: return "Psy-limiter";

                case GearType.GauntletBuster: return "Arm Cannon";
                case GearType.ExplosivePill: return "Charge";

                case GearType.chain2: return "Chain";
                case GearType.magicGauntlet: return "Lucent Gauntlet";
                case GearType.transmitter: return "Warp Forge";
                case GearType.magicWing: return "Lucent Wings";
                case GearType.pathOfAbyss: return "Abyssal Path";

                case GearType.relic: return "Relic";
                case GearType.ancientBow: return "Ancient Bow";

                case GearType.handFan: return "Ritual Fan";
                case GearType.fanTassel: return "Fan Tassel";

                case GearType.tuner: return "Bladecaster";
                case GearType.bracelet: return "Bladebinder";

                case GearType.breathShooter: return "Whispershot";
                case GearType.weaponBelt: return "Weapon Belt";

                case GearType.ornament: return "Ornament";

                case GearType.chakram: return "Chakram";
                case GearType.hexSeeker: return "Hex Seeker";

                case GearType.boxingCannon: return "拳封";//Mo Xuan weapon
                case GearType.boxingSky: return "拳天";//Mo Xuan weapon
                default: return null;
            }
        }

        /// <summary>
        /// 获取武器攻击速度所对应的字符串。
        /// </summary>
        /// <param Name="attackSpeed">表示武器的攻击速度，通常为2~9的数字。</param>
        /// <returns></returns>
        public static string GetAttackSpeedString(int attackSpeed)
        {
            string str;
            switch (attackSpeed)
            {
                case 2:
                case 3: str = "Very Fast"; break;
                case 4:
                case 5: str = "Fast"; break;
                case 6: str = "Normal"; break;
                case 7:
                case 8: str = "Slow"; break;
                case 9: str = "Very Slow"; break;
                default:
                    if (attackSpeed < 2) return "吃屎一样快";
                    else if (attackSpeed > 9) return "吃屎一样慢";
                    else return attackSpeed.ToString();
            }
            return str + " (Stage " + (10 - attackSpeed) + ")";
        }

        /// <summary>
        /// 获取套装装备类型的字符串。
        /// </summary>
        /// <param Name="Type">表示套装装备类型的GearType。</param>
        /// <returns></returns>
        public static string GetSetItemGearTypeString(GearType type)
        {
            return GetGearTypeString(type);
        }

        /// <summary>
        /// 获取装备额外职业要求说明的字符串。
        /// </summary>
        /// <param Name="Type">表示装备类型的GearType。</param>
        /// <returns></returns>
        public static string GetExtraJobReqString(GearType type)
        {
            switch (type)
            {
                //0xxx
                case GearType.heroMedal: return "Hero only";
                case GearType.rosario: return "Paladin only";
                case GearType.chain: return "Dark Knight only";
                case GearType.book1: return "Fire/Poison Magician only";
                case GearType.book2: return "Ice/Lightning Magician only";
                case GearType.book3: return "Bishop Magician only";
                case GearType.bowMasterFeather: return "Bow Master only";
                case GearType.crossBowThimble: return "Marksman only";
                case GearType.relic: return "Pathfinder only";
                case GearType.shadowerSheath: return "Shadower only";
                case GearType.nightLordPoutch: return "Night Lord only";
                case GearType.katara: return "Dual Blade only";
                case GearType.viperWristband: return "Buccaneer only";
                case GearType.captainSight: return "Corsair only";
                case GearType.cannonGunPowder:
                case GearType.cannonGunPowder2: return "Cannoneer only";
                case GearType.box:
                case GearType.boxingClaw: return "Jett only";

                //1xxx
                case GearType.cygnusGem: return "Cygnus Knights only";

                //2xxx
                case GearType.aranPendulum: return GetExtraJobReqString(21);
                case GearType.dragonMask:
                case GearType.dragonPendant:
                case GearType.dragonWings:
                case GearType.dragonTail:
                case GearType.evanPaper: return GetExtraJobReqString(22);
                case GearType.magicArrow: return GetExtraJobReqString(23);
                case GearType.card: return GetExtraJobReqString(24);
                case GearType.foxPearl: return GetExtraJobReqString(25);
                case GearType.orb:
                case GearType.shiningRod: return GetExtraJobReqString(27);

                //3xxx
                case GearType.demonShield: return GetExtraJobReqString(31);
                case GearType.desperado: return "Demon Avenger only";
                case GearType.battlemageBall: return "Battle Mage only";
                case GearType.wildHunterArrowHead: return "Wild Hunter only";
                case GearType.machineEngine:
                case GearType.machineArms:
                case GearType.machineLegs:
                case GearType.machineBody:
                case GearType.machineTransistors:
                case GearType.mailin: return "Mechanic only";
                case GearType.controller:
                case GearType.powerSource:
                case GearType.energySword: return GetExtraJobReqString(36);
                case GearType.GauntletBuster:
                case GearType.ExplosivePill: return GetExtraJobReqString(37);

                //4xxx
                case GearType.katana:
                case GearType.kodachi:
                case GearType.kodachi2: return GetExtraJobReqString(41);
                case GearType.fan: return "Kanna only"; //Haku only?

                //5xxx
                case GearType.soulShield: return "Mihile only";

                //6xxx
                case GearType.novaMarrow: return GetExtraJobReqString(61);
                case GearType.weaponBelt:
                case GearType.breathShooter: return GetExtraJobReqString(63);
                case GearType.chain2:
                case GearType.transmitter: return GetExtraJobReqString(64);
                case GearType.soulBangle:
                case GearType.soulShooter: return GetExtraJobReqString(65);

                //10xxx
                case GearType.swordZB:
                case GearType.swordZL: return GetExtraJobReqString(101);

                case GearType.whistle:
                case GearType.magicStick: return GetExtraJobReqString(112);

                case GearType.espLimiter:
                case GearType.chess: return GetExtraJobReqString(142);

                case GearType.magicGauntlet:
                case GearType.magicWing: return GetExtraJobReqString(152);

                case GearType.pathOfAbyss: return GetExtraJobReqString(155);
                case GearType.handFan:
                case GearType.fanTassel: return GetExtraJobReqString(164);

                case GearType.tuner:
                case GearType.bracelet: return GetExtraJobReqString(151);

                case GearType.boxingCannon:
                case GearType.boxingSky: return GetExtraJobReqString(175);

                case GearType.ornament: return GetExtraJobReqString(162);
                default: return null;
            }
        }

        /// <summary>
        /// 获取装备额外职业要求说明的字符串。
        /// </summary>
        /// <param Name="specJob">表示装备属性的reqSpecJob的值。</param>
        /// <returns></returns>
        public static string GetExtraJobReqString(int specJob)
        {
            switch (specJob)
            {
                case 21: return "Aran only";
                case 22: return "Evan only";
                case 23: return "Mercedes only";
                case 24: return "Phantom only";
                case 25: return "Shade only";
                case 27: return "Luminous only";
                case 31: return "Demon only";
                case 36: return "Xenon only";
                case 37: return "Blaster only";
                case 41: return "Hayato only";
                case 42: return "Kanna only";
                case 51: return "Mihile only";
                case 61: return "Kaiser only";
                case 63: return "Kain only";
                case 64: return "Cadena only";
                case 65: return "Angelic Buster only";
                case 101: return "Zero only";
                case 112: return "Beast Tamer only";
                case 142: return "Kinesis only";
                case 151: return "Adele only";
                case 152: return "Illium only";
                case 154: return "Khali only";
                case 155: return "Ark only";
                case 162: return "Lara only";
                case 164: return "Hoyoung only";
                case 175: return "Mo Xuan only";

                default: return null;
            }
        }

        public static string GetItemPropString(ItemPropType propType, int value)
        {
            switch (propType)
            {
                case ItemPropType.tradeBlock:
                    return GetGearPropString(GearPropType.tradeBlock, value);
                case ItemPropType.useTradeBlock:
                    return value == 0 ? null : "Cannot be traded after use";
                case ItemPropType.tradeAvailable:
                    return GetGearPropString(GearPropType.tradeAvailable, value);
                case ItemPropType.only:
                    return GetGearPropString(GearPropType.only, value);
                case ItemPropType.accountSharable:
                    return GetGearPropString(GearPropType.accountSharable, value);
                case ItemPropType.sharableOnce:
                    return GetGearPropString(GearPropType.sharableOnce, value);
                case ItemPropType.exchangeableOnce:
                    return value == 0 ? null : "Tradable once (untradable after using or trading)";
                case ItemPropType.quest:
                    return value == 0 ? null : "Quest Item";
                case ItemPropType.pquest:
                    return value == 0 ? null : "Party Quest Item";
                case ItemPropType.permanent:
                    return value == 0 ? null : "PERMANENT";//GMS PLACEHOLDER?
                case ItemPropType.multiPet://GMS string for: "Normal Pet (Cannot be used with other pets)" and "Multi Pet (Can use up to 3 pets at once)"
                    return value == 0 ? "" : "";
                default:
                    return null;
            }
        }

        public static string GetItemCoreSpecString(ItemCoreSpecType coreSpecType, int value, string desc)
        {
            bool hasCoda = false;
            if (desc?.Length > 0)
            {
                char lastCharacter = desc.Last();
                hasCoda = lastCharacter >= '가' && lastCharacter <= '힣' && (lastCharacter - '가') % 28 != 0;
            }
            switch (coreSpecType)
            {
                case ItemCoreSpecType.Ctrl_mobLv:
                    return value == 0 ? null : "Monster Level " + "+" + value;
                case ItemCoreSpecType.Ctrl_mobHPRate:
                    return value == 0 ? null : "Monster HP " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_mobRate:
                    return value == 0 ? null : "Monster Population " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_mobRateSpecial:
                    return value == 0 ? null : "Monster Population " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_change_Mob:
                    return desc == null ? null : "Change monster skins for " + desc;
                case ItemCoreSpecType.Ctrl_change_BGM:
                    return desc == null ? null : "Change music for " + desc;
                case ItemCoreSpecType.Ctrl_change_BackGrnd:
                    return desc == null ? null : "Change background image for " + desc;
                case ItemCoreSpecType.Ctrl_partyExp:
                    return value == 0 ? null : "Party EXP " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_partyExpSpecial:
                    return value == 0 ? null : "Party EXP " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_addMob:
                    return value == 0 || desc == null ? null : desc + ", Link " + value + " added to area";
                case ItemCoreSpecType.Ctrl_dropRate:
                    return value == 0 ? null : "Drop Rate " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_dropRateSpecial:
                    return value == 0 ? null : "Drop Rate " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_dropRate_Herb:
                    return value == 0 ? null : "Herb Drop Rate " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_dropRate_Mineral:
                    return value == 0 ? null : "Mineral Drop Rate " + "+" + value + "%";
                case ItemCoreSpecType.Ctrl_dropRareEquip:
                    return value == 0 ? null : "Rare Equipment Drop";
                case ItemCoreSpecType.Ctrl_reward:
                case ItemCoreSpecType.Ctrl_addMission:
                    return desc;
                default:
                    return null;
            }
        }

        public static string GetSkillReqAmount(int skillID, int reqAmount)
        {
            switch (skillID / 10000)
            {
                case 11200: return "[Required Bear Skill Point(s): " + reqAmount + "]";
                case 11210: return "[Required Leopard Skill Point(s): " + reqAmount + "]";
                case 11211: return "[Required Hawk Skill Point(s): " + reqAmount + "]";
                case 11212: return "[Required Cat Skill Point(s): " + reqAmount + "]";
                default: return "[Required ?? Skill Point(s): " + reqAmount + "]";
            }
        }

        public static string GetJobName(int jobCode)
        {
            switch (jobCode)
            {
                case 0: return "Beginner";
                case 100: return "Swordsman";
                case 110: return "Fighter";
                case 111: return "Crusader";
                case 112: return "Hero";
                case 120: return "Page";
                case 121: return "White Knight";
                case 122: return "Paladin";
                case 130: return "Spearman";
                case 131: return "Dragon Knight";
                case 132: return "Dark Knight";
                case 200: return "Magician";
                case 210: return "Wizard (Fire,Poison)";
                case 211: return "Mage (Fire, Poison)";
                case 212: return "Arch Mage (Fire,Poison)";
                case 220: return "Wizard (Ice,Lightning)";
                case 221: return "Mage (Ice,Lightning)";
                case 222: return "Arch Mage (Ice,Lightning)";
                case 230: return "Cleric";
                case 231: return "Priest";
                case 232: return "Bishop";
                case 300: return "Archer";
                case 301: return "Archer";
                case 310: return "Hunter";
                case 311: return "Ranger";
                case 312: return "Bowmaster";
                case 320: return "Crossbowman";
                case 321: return "Sniper";
                case 322: return "Marksman";
                case 330: return "Ancient Archer";
                case 331: return "Soulchaser";
                case 332: return "Pathfinder";
                case 333: return "Pathfinder (5)";
                case 400: return "Rogue";
                case 410: return "Assassin";
                case 411: return "Hermit";
                case 412: return "Night Lord";
                case 420: return "Thief";
                case 421: return "Chief Bandit";
                case 422: return "Shadower";
                case 430: return "Blade Recruit";
                case 431: return "Blade Acolyte";
                case 432: return "Blade Specialist";
                case 433: return "Blade Loard";
                case 434: return "Blade Master";
                case 500: return "Pirate";
                case 501: return "Pirate";
                case 508: return "Jett(1)";
                case 510: return "Brawler";
                case 511: return "Marauder";
                case 512: return "Buccaneer";
                case 520: return "Gunslinger";
                case 521: return "Outlaw";
                case 522: return "Captain";
                case 530: return "Cannoneer";
                case 531: return "Cannon Trooper";
                case 532: return "Cannon Master";
                case 570: return "Jett(2)";
                case 571: return "Jett(3)";
                case 572: return "Jett(4)";

                case 1000: return "Noblesse";
                case 1100: return "Dawn Warrior(1)";
                case 1110: return "Dawn Warrior(2)";
                case 1111: return "Dawn Warrior(3)";
                case 1112: return "Dawn Warrior(4)";
                case 1200: return "Blaze Wizard(1)";
                case 1210: return "Blaze Wizard(2)";
                case 1211: return "Blaze Wizard(3)";
                case 1212: return "Blaze Wizard(4)";
                case 1300: return "Wind Archer(1)";
                case 1310: return "Wind Archer(2)";
                case 1311: return "Wind Archer(3)";
                case 1312: return "Wind Archer(4)";
                case 1400: return "Night Walker(1)";
                case 1410: return "Night Walker(2)";
                case 1411: return "Night Walker(3)";
                case 1412: return "Night Walker(4)";
                case 1500: return "Thunder Breaker(1)";
                case 1510: return "Thunder Breaker(2)";
                case 1511: return "Thunder Breaker(3)";
                case 1512: return "Thunder Breaker(4)";

                case 2000: return "Legend";
                case 2001: return "Evan";
                case 2002: return "Mercedes";
                case 2003: return "Phantom";
                case 2004: return "Luminous";
                case 2005: return "Shade";
                case 2100: return "Aran(1)";
                case 2110: return "Aran(2)";
                case 2111: return "Aran(3)";
                case 2112: return "Aran(4)";
                case 2200:
                case 2210: return "Evan(1)";
                case 2211:
                case 2212:
                case 2213: return "Evan(2)";
                case 2214:
                case 2215:
                case 2216: return "Evan (3)";
                case 2217:
                case 2218: return "Evan(4)";
                case 2300: return "Mercedes(1)";
                case 2310: return "Mercedes(2)";
                case 2311: return "Mercedes(3)";
                case 2312: return "Mercedes(4)";
                case 2400: return "Phantom(1)";
                case 2410: return "Phantom(2)";
                case 2411: return "Phantom(3)";
                case 2412: return "Phantom(4)";
                case 2500: return "Shade(1)";
                case 2510: return "Shade(2)";
                case 2511: return "Shade(3)";
                case 2512: return "Shade(4)";
                case 2700: return "Luminous(1)";
                case 2710: return "Luminous(2)";
                case 2711: return "Luminous(3)";
                case 2712: return "Luminous(4)";


                case 3000: return "Citizen";
                case 3001: return "Demon";
                case 3100: return "Demon Slayer(1)";
                case 3110: return "Demon Slayer(2)";
                case 3111: return "Demon Slayer(3)";
                case 3112: return "Demon Slayer(4)";
                case 3101: return "Demon Avenger(1)";
                case 3120: return "Demon Avenger(2)";
                case 3121: return "Demon Avenger(3)";
                case 3122: return "Demon Avenger(4)";
                case 3200: return "Battle Mage(1)";
                case 3210: return "Battle Mage(2)";
                case 3211: return "Battle Mage(3)";
                case 3212: return "Battle Mage(4)";
                case 3300: return "Wild Hunter(1)";
                case 3310: return "Wild Hunter(2)";
                case 3311: return "Wild Hunter(3)";
                case 3312: return "Wild Hunter(4)";
                case 3500: return "Mechanic(1)";
                case 3510: return "Mechanic(2)";
                case 3511: return "Mechanic(3)";
                case 3512: return "Mechanic(4)";
                case 3002: return "Xenon";
                case 3600: return "Xenon(1)";
                case 3610: return "Xenon(2)";
                case 3611: return "Xenon(3)";
                case 3612: return "Xenon(4)";
                case 3700: return "Blaster(1)";
                case 3710: return "Blaster(2)";
                case 3711: return "Blaster(3)";
                case 3712: return "Blaster(4)";

                case 4001: return "Hayato";
                case 4002: return "Kanna";
                case 4100: return "Hayato(1)";
                case 4110: return "Hayato(2)";
                case 4111: return "Hayato(3)";
                case 4112: return "Hayato(4)";
                case 4200: return "Kanna(1)";
                case 4210: return "Kanna(2)";
                case 4211: return "Kanna(3)";
                case 4212: return "Kanna(4)";


                case 5000: return "Mihile";
                case 5100: return "Mihile(1)";
                case 5110: return "Mihile(2)";
                case 5111: return "Mihile(3)";
                case 5112: return "Mihile(4)";


                case 6000: return "Kaiser";
                case 6001: return "Angelic Buster";
                case 6002: return "Cadena";
                case 6003: return "Kain";
                case 6100: return "Kaiser(1)";
                case 6110: return "Kaiser(2)";
                case 6111: return "Kaiser(3)";
                case 6112: return "Kaiser(4)";
                case 6300: return "Kain(1)";
                case 6310: return "Kain(2)";
                case 6311: return "Kain(3)";
                case 6312: return "Kain(4)";
                case 6400: return "Cadena(1)";
                case 6410: return "Cadena(2)";
                case 6411: return "Cadena(3)";
                case 6412: return "Cadena(4)";
                case 6500: return "Angelic Buster(1)";
                case 6510: return "Angelic Buster(2)";
                case 6511: return "Angelic Buster(3)";
                case 6512: return "Angelic Buster(4)";

                case 10000: return "Zero";
                case 10100: return "Zero(1)";
                case 10110: return "Zero(2)";
                case 10111: return "Zero(3)";
                case 10112: return "Zero(4)";

                case 11000: return "Chase";
                case 11200: return "Beast Tamer(1)";
                case 11210: return "Beast Tamer(2)";
                case 11211: return "Beast Tamer(3)";
                case 11212: return "Beast Tamer(4)";

                case 13000: return "Pink Bean";
                case 13001: return "Yetihood";
                case 13100: return "Pink Bean";
                case 13500: return "Yeti";

                case 14000: return "Kinesis";
                case 14200: return "Kinesis(1)";
                case 14210: return "Kinesis(2)";
                case 14211: return "Kinesis(3)";
                case 14212: return "Kinesis(4)";
                case 14213: return "Kinesis(5)";

                case 15000: return "Illium";
                case 15001: return "Ark";
                case 15002: return "Adele";
                case 15003: return "Khali";
                case 15100: return "Adele(1)";
                case 15110: return "Adele(2)";
                case 15111: return "Adele(3)";
                case 15112: return "Adele(4)";
                case 15200: return "Illium(1)";
                case 15210: return "Illium(2)";
                case 15211: return "Illium(3)";
                case 15212: return "Illium(4)";
                case 15400: return "Khali(1)";
                case 15410: return "Khali(2)";
                case 15411: return "Khali(3)";
                case 15412: return "Khali(4)";
                case 15500: return "Ark(1)";
                case 15510: return "Ark(2)";
                case 15511: return "Ark(3)";
                case 15512: return "Ark(4)";

                case 16000: return "Anima Thief";
                case 16001: return "Lara";
                case 16200: return "Lara(1)";
                case 16210: return "Lara(2)";
                case 16211: return "Lara(3)";
                case 16212: return "Lara(4)";
                case 16400: return "Hoyoung(1)";
                case 16410: return "Hoyoung(2)";
                case 16411: return "Hoyoung(3)";
                case 16412: return "Hoyoung(4)";
            }
            return null;
        }
    }
}
