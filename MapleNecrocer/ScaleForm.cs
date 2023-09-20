using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapleNecrocer;

public partial class ScaleForm : Form
{
    public ScaleForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ScaleForm Instance;
    public static int ScaleX, ScaleY;

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        RenderFormDraw.ScreenMode = ScreenMode.Scale;
        var Split = comboBox1.Text.Split("->");

        var SrcStr = Split[0].Split('X');
        Map.DisplaySize.X = SrcStr[0].Trim(' ').ToInt();
        Map.DisplaySize.Y = SrcStr[1].Trim(' ').ToInt();

        var DestStr = Split[1].Split('X');
        ScaleX = DestStr[0].Trim(' ').ToInt();
        ScaleY = DestStr[1].Trim(' ').ToInt();
        RenderFormDraw.ScreenMode = ScreenMode.Scale;

        bool Result;
        Result = MainForm.MoveWindow(MainForm.Instance.Handle, MainForm.Instance.Left, MainForm.Instance.Top, ScaleX + 283, ScaleY + 140, true);
        //this.Width = Map.DisplaySize.X + 283;
        //this.Height = Map.DisplaySize.Y + 124;

        Result = MainForm.MoveWindow(MainForm.RenderForm.Handle, MainForm.RenderForm.Left, MainForm.RenderForm.Top, ScaleX, ScaleY, true);
        // RenderForm.Width = Map.DisplaySize.X;
        //RenderForm.Height = Map.DisplaySize.Y;
        RenderForm.RenderFormDraw.Width = ScaleX;
        RenderForm.RenderFormDraw.Height = ScaleY;
        RenderForm.RenderFormDraw.Parent = MainForm.RenderForm;
        EngineFunc.SpriteEngine.VisibleWidth = Map.DisplaySize.X + 200;
        EngineFunc.SpriteEngine.VisibleHeight = Map.DisplaySize.Y + 200;
        Map.ResetPos = true;
        MainForm.Instance.CenterToScreen2();


    }

    private void ScaleForm_Load(object sender, EventArgs e)
    {

        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
    }

    private void ScaleForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!comboBox1.Focused)
            ActiveControl = null;


    }
}
