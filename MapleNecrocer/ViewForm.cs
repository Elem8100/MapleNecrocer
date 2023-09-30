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

public partial class ViewForm : Form
{
    public ViewForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ViewForm Instance;

    private void ViewForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

    }

    private void ViewForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Alt)
            e.Handled = true;
        if (!textBox1.Focused && !textBox2.Focused)
            ActiveControl = null;
    }

    private void Tile_CheckedChanged(object sender, EventArgs e)
    {
        switch (((CheckBox)sender).Name)
        {
            case "Tile": Map.ShowTile = !Map.ShowTile; break;
            case "Obj": Map.ShowObj = !Map.ShowObj; break;
            case "Back": Map.ShowBack = !Map.ShowBack; break;
            case "Front": Map.ShowFront = !Map.ShowFront; break;
            case "Npc": Map.ShowNpc = !Map.ShowNpc; break;
            case "NpcName": Map.ShowNpcName = !Map.ShowNpcName; break;
            case "NpcChat": Map.ShowNpcChat = !Map.ShowNpcChat; break;
            case "Mob": Map.ShowMob = !Map.ShowMob; break;
            case "MobName": Map.ShowMobName = !Map.ShowMobName; break;
            case "ID": Map.ShowID = !Map.ShowID; break;
            case "Portal": Map.ShowPortal = !Map.ShowPortal; break;
            case "BgmName": Map.ShowBgmName = !Map.ShowBgmName; break;
            case "Foothold": Map.ShowFootholds = !Map.ShowFootholds; break;
            case "Player": Map.ShowPlayer = !Map.ShowPlayer; break;

        }

    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (LabelRingTag.Instance != null)
        {
            LabelRingTag.Instance.MedalName = textBox1.Text;
            LabelRingTag.Instance.InitData();
            LabelRingTag.ReDraw();
        }
        Game.Player.Name = textBox1.Text;
        NameTag.ReDraw = true;

    }
}
