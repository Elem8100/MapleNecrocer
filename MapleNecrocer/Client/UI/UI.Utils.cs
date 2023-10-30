using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.UI.Forms;
using MapleNecrocer;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace GameUI;
public class UIControlManager : ControlManager
{
    public UIControlManager()
    {

    }
}

public class UI
{
    public static UIControlManager ControlManager = new();
    public static UIForm RefForm;

    public static Dictionary<string, UIForm> Form = new();
    public static Dictionary<string, UIButton> Button = new();
    public static void CreateDefault()
    {


    }

    public static void CrateForm(string UIPath, int X = 0, int Y = 0, bool IsMoveable = true, bool visible = true)
    {
        var _Form = new UIForm();
        // Form.HitBox=new Microsoft.Xna.Framework.Rectangle(0,0,500,500);
        var UINode = Wz.GetNode(UIPath);
        Wz.DumpData(UINode, Wz.UIData, Wz.UIImageLib);
        _Form.Location = new Vector2(X, Y);
        _Form.UIPath = UIPath;
        var UIPng = UINode.ExtractPng();
        _Form.Size = new Vector2(UIPng.Width, UIPng.Height);
        _Form.IsMovable = IsMoveable;
        _Form.IsVisible = visible;

        RefForm = _Form;
        ControlManager.Controls.Add(_Form);
        Form.Add(UIPath, _Form);
    }
    public static void CreateButton(string UIPath, int X = 0, int Y = 0, Action ClickEvent = null)
    {
        var Button = new UIButton();
        var UINode = Wz.GetNode(UIPath);
        if (!Wz.UIData.ContainsKey(UINode.FullPathToFile2()))
            Wz.DumpData(UINode, Wz.UIData, Wz.UIImageLib);
        Button.UIPath = UIPath;

        var Origin = UINode.GetVector("normal/0/ogigin");
        Button.Location = new Vector2(X + -Origin.X, Y + -Origin.Y);
        var UIPng = UINode.GetBmp("normal/0");
        Button.Size = new Vector2(UIPng.Width, UIPng.Height);
        Button.Clicked += (s, e) => { ClickEvent(); };
        RefForm.Controls.Add(Button);
    }

    public static void CreateButton(string ButtonName, string UIPath, int X = 0, int Y = 0, Action ClickEvent = null)
    {
        var _Button = new UIButton();
        var UINode = Wz.GetNode(UIPath);
        if (!Wz.UIData.ContainsKey(UINode.FullPathToFile2()))
            Wz.DumpData(UINode, Wz.UIData, Wz.UIImageLib);
        _Button.UIPath = UIPath;

        var Origin = UINode.GetVector("normal/0/ogigin");
        _Button.Location = new Vector2(X + -Origin.X, Y + -Origin.Y);
        var UIPng = UINode.GetBmp("normal/0");
        _Button.Size = new Vector2(UIPng.Width, UIPng.Height);
        _Button.Clicked += (s, e) => { ClickEvent(); };
        RefForm.Controls.Add(_Button);
        Button.Add(ButtonName, _Button);
    }

}
