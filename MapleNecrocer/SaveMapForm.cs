using Microsoft.Xna.Framework.Graphics;
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

public partial class SaveMapForm : Form
{
    public SaveMapForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static SaveMapForm Instance;

    private void button1_Click(object sender, EventArgs e)
    {

        int MapWidth = Map.Info["MapWidth"];
        int MapHeight = 0;
        if (Map.Info.ContainsKey("VRLeft"))
            MapHeight = Map.Bottom - Map.Top;
        else
            MapHeight = Map.SaveMapBottom - Map.Top;

        EngineFunc.SpriteEngine.Camera.X = Map.Left;
        EngineFunc.SpriteEngine.Camera.Y = Map.Top;
        EngineFunc.SpriteEngine.VisibleWidth = MapWidth;
        EngineFunc.SpriteEngine.VisibleHeight = MapHeight;

        Map.SaveMap = true;
        RenderTarget2D SaveTexture = null;
        EngineFunc.SpriteEngine.Move(1);
      
        EngineFunc.Canvas.DrawTarget(ref SaveTexture, MapWidth, MapHeight, () =>
        {
           EngineFunc.SpriteEngine.Draw();
        });
       
        EngineFunc.SpriteEngine.Move(1);
        Map.SaveMap = false;
        Map.ResetPos = true;

        EngineFunc.SpriteEngine.VisibleWidth = Map.DisplaySize.X;
        EngineFunc.SpriteEngine.VisibleHeight = Map.DisplaySize.Y;
        string MapName = "";
        if (MainForm.Instance.MapNames.ContainsKey(Map.ID))
            MapName = MainForm.Instance.MapNames[Map.ID];
        MapName = MapName.Replace('<', '(');
        MapName = MapName.Replace('>', ')');
        Stream stream = File.OpenWrite(System.Environment.CurrentDirectory + "\\" + Map.ID + '-' + MapName + ".png");
        SaveTexture.SaveAsPng(stream, MapWidth, MapHeight);
        stream.Dispose();
        SaveTexture.Dispose();
    }

    private void SaveMapForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
    }

    private void SaveMapForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
    }
}
