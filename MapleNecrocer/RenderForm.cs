using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MonoGame.Forms.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using WzComparerR2.Rendering;
using WzComparerR2.MapRender2;
using WzComparerR2.Animation;
using Spine;
using WzComparerR2.WzLib;
using WzComparerR2.PluginBase;
using System.Reflection;
namespace MapleNecrocer;

public partial class RenderForm : Form
{
    public RenderForm()
    {
        InitializeComponent();
        RenderFormDraw = new();
        RenderFormDraw.Width = 1024;
        RenderFormDraw.Height = 768;
        RenderFormDraw.Parent = this;

    }
    public static RenderFormDraw RenderFormDraw;


    private void RenderForm_Load(object sender, EventArgs e)
    {

    }

}


