using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using Vector2 = Microsoft.Xna.Framework.Vector2;


namespace MapleNecrocer;

internal class ItemDrop : JumperSprite
{
    public ItemDrop(Sprite Parent) : base(Parent)
    {
        ImageLib = Wz.EquipImageLib;
        DoJump = true;
        JumpSpeed = 0.3f;
        JumpHeight = 9;
        MaxFallSpeed = 8;
        Random Random = new();
        CosY = Random.Next(100);
        SpriteSheetMode= SpriteSheetMode.NoneSingle;
        DoCenter = true;
    }
    static float Value;
    Foothold FH;
    int Number;
    string Style;
    int TimeCount;
    int Frame;
    int FTime;
    public float MoveX;
    public float CosY;
    public string ParentPath;
    public string ID;

    static string GetItemDir(string ID)
    {
        switch (ID.ToInt() / 1000000)
        {
            case 5: return "Cash";
            case 2: return "Consume";
            case 4: return "Etc";
            case 3: return "Install";
            case 9: return "Special";
        }
        return "";
    }
    static void AddItem(string ID)
    {
        if (ID.LeftStr(2) != "01")
        {
            Wz_Node Entry = Wz.GetNode("Item/" + GetItemDir(ID) + '/' + ID.LeftStr(4) + ".img/" + ID);
            Equip.DataS.Add(ID + "/drop", "0");
            Wz.DumpData(Entry, Wz.EquipData, Wz.EquipImageLib);
        }
    }
    public static void Drop(int AX, int AY, int Num, List<string> DropList)
    {
        Value = Num / 2 + 1;
        Random Random = new();
        for (int I = 0; I <= Num; I++)
        {
            if (I < 0)
                Value = Value + 1;
            else
                Value = Value - 1;
            var ItemDrop = new ItemDrop(EngineFunc.SpriteEngine);

            int Rand = Random.Next(DropList.Count);
            if (!Equip.DataS.ContainsKey(DropList[Rand] + "/drop"))
                AddItem(DropList[Rand]);
            ItemDrop.ID = DropList[Rand];
            string Dir = GetItemDir(ItemDrop.ID);
            string IconPath = "Item/" + Dir + '/' + ItemDrop.ID.LeftStr(4) + ".img/" + ItemDrop.ID + "/info/iconRaw";
            if (ItemDrop.ID.LeftStr(4) != "0900")
                ItemDrop.ImageNode = Wz.EquipData[IconPath];
            ItemDrop.Angle = Random.Next(100) * 0.0628f;
            if (ItemDrop.ID.LeftStr(4) == "0900")
            {
                ItemDrop.ParentPath = "Item/Special/0900.img/" + ItemDrop.ID + "/iconRaw";
                IconPath = ItemDrop.ParentPath + "/0";
                ItemDrop.ImageNode = Wz.EquipData[IconPath];
            }
            ItemDrop.X = AX;
            ItemDrop.Y = AY;
            ItemDrop.Z = Game.Player.Z;
            ItemDrop.MoveX = Value * 0.5f;
            ItemDrop.Offset.X = -ItemDrop.ImageNode.GetVector("origin").X;
            ItemDrop.Offset.Y = -ItemDrop.ImageNode.GetVector("origin").Y + 5;
        }
    }

    public static void Drop(int AX, int AY, int Num, string ID)
    {
        Value = Num / 2 + 1;
        Random Random = new();
        for (int I = 0; I <= Num; I++)
        {
            if (I < 0)
                Value = Value + 1;
            else
                Value = Value - 1;
            var ItemDrop = new ItemDrop(EngineFunc.SpriteEngine);
            if (!Equip.DataS.ContainsKey(ID + "/drop"))
                AddItem(ID);
            string Dir = GetItemDir(ID);
            string IconPath = "Item/" + Dir + '/' + ID.LeftStr(4) + ".img/" + ID + "/info/iconRaw";
            ItemDrop.ID=ID;             
            if (ItemDrop.ID.LeftStr(4) != "0900")
                ItemDrop.ImageNode = Wz.EquipData[IconPath];
            ItemDrop.Angle = Random.Next(100) * 0.0628f;
            if (ItemDrop.ID.LeftStr(4) == "0900")
            {
                ItemDrop.ParentPath = "Item/Special/0900.img/" + ID + "/iconRaw";
                IconPath = ItemDrop.ParentPath + "/0";
                ItemDrop.ImageNode = Wz.EquipData[IconPath];
            }
            ItemDrop.X = AX;
            ItemDrop.Y = AY;
            ItemDrop.Z = Game.Player.Z;
            ItemDrop.MoveX = Value * 0.5f;
            ItemDrop.Offset.X = -ItemDrop.ImageNode.GetVector("origin").X;
            ItemDrop.Offset.Y = -ItemDrop.ImageNode.GetVector("origin").Y + 15;
        }
    }
    public override void DoMove(float Delta)
    {  
        base.DoMove(Delta);
        X += MoveX;
        if (JumpState != JumpState.jsNone)
            Angle += 0.5f;
        if (JumpState == JumpState.jsFalling)
        {
            Foothold BelowFH = null;
            Vector2 Below = FootholdTree.Instance.FindBelow(new Vector2(X, Y - VelocityY - 2), ref BelowFH);
            if (Y >= Below.Y - 10)
            {
                Y = Below.Y;
                JumpState = JumpState.jsNone;
                Angle=0;
                Offset.X = -12;
                FH = BelowFH;
                Z = FH.Z * 100000 + 6000;
            }
        }

        if (ID.LeftStr(4) == "0900")
        {
            ImageNode = Wz.EquipData[ParentPath + '/' + Frame];
            FTime += 17;
            if (FTime > 100)
            {
                Frame += 1;
                if (Frame > 3)
                    Frame = 0;
                FTime = 0;
            }
            Offset.X = -ImageNode.GetVector("origin").X;
            Offset.Y = -ImageNode.GetVector("origin").Y + 3;
        }

        if (JumpState == JumpState.jsNone)
        {
            MoveX = 0;
            CosY += 0.055f;
            Y = Y - (float)Math.Cos(CosY) * 0.15f;
        }

        TimeCount += 1;
        if (TimeCount > 1000)
            Alpha -= 7;
        if (Alpha < 10)
            Dead();
    }


}

