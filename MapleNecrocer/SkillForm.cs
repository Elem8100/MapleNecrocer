using DevComponents.DotNetBar.Controls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.CharaSim;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Input = Microsoft.Xna.Framework.Input.Keys;
namespace MapleNecrocer;

public partial class SkillForm : Form
{
    public SkillForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static SkillForm Instance;
    public DataGridViewEx SkillListGrid, UseListGrid;
    int SelectRow;
    
   
    
    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var Rec = DataGrid.GetCellDisplayRectangle(1, e.RowIndex, true);
        comboBox1.Top = Rec.Top + 7;
        comboBox1.Left = Rec.Left + 239;
        SelectRow = e.RowIndex;
        comboBox1.SelectedIndex = -1;
        comboBox1.Visible = true;
    }

    private void SkillForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        var Skill = new Skill(EngineFunc.SpriteEngine);
        Skill.Tag = 1;

        SkillListGrid = new(100, 185, 0, 0, 220, 400, true, panel1);
        SkillListGrid.Dock = DockStyle.Fill;
        SkillListGrid.SearchGrid.Dock = DockStyle.Fill;
        SkillListGrid.RowTemplate.Height = 40;
        var Str = new DataGridViewTextBoxColumn();
        SkillListGrid.Columns.AddRange(Str);
        SkillListGrid.Columns[3].Width = 75;
        comboBox1.Parent = SkillListGrid;

        UseListGrid = new(100, 185, 0, 0, 220, 400, true, panel2);
        UseListGrid.Dock = DockStyle.Fill;
        UseListGrid.SearchGrid.Dock = DockStyle.Fill;
        UseListGrid.RowTemplate.Height = 40;
        var Str2 = new DataGridViewTextBoxColumn();
        UseListGrid.Columns.AddRange(Str2);
        UseListGrid.Columns[3].Width = 75;


        SkillListGrid.CellClick += (s, e) =>
        {
            CellClick(SkillListGrid, e);
        };

        SkillListGrid.Scroll += (s, e) =>
        {
            comboBox1.Visible = false;
        };

        SkillListGrid.SetToolTipEvent(WzType.Skill, this);
        var Graphic = SkillListGrid.CreateGraphics();
       
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        Win32.SendMessage(SkillListGrid.Handle, false);
        foreach (var Img in Wz.GetNodes("Skill"))
        {
            if (!Char.IsNumber(Img.Text[0]))
                continue;
            if (Img.Text[0] == '0')
                continue;
            if (!Wz.HasNode("Skill/" + Img.Text + "/skill"))
                continue;
            foreach (var ID in Wz.GetNodes("Skill/" + Img.Text + "/skill"))
            {
                if (ID.Text[0] == '0')
                    continue;
                if (!ID.HasNode("hit"))
                    continue;
                if (!ID.HasNode("common"))
                    continue;
                if (!ID.HasNode("common/lt"))
                    continue;
                if (!ID.HasNode("effect"))
                    continue;
                Bitmap Bmp = Wz.GetBmp("Skill/" + Skill.GetJobImg(ID.Text) + ".img/skill/" + ID.Text + "/icon");
                string SkillName = "";
                if (Wz.HasNode("String/Skill.img/" + ID.Text))
                    SkillName = Wz.GetStr("String/Skill.img/" + ID.Text + "/name");
                SkillListGrid.Rows.Add(ID.Text, Bmp, SkillName);

            }

        }
        Win32.SendMessage(SkillListGrid.Handle, true);
        SkillListGrid.Refresh();

        for (int i = 0; i < SkillListGrid.Rows.Count; i++)
        {
            SkillListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SkillListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }

    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (comboBox1.SelectedIndex == -1)
            return;
        UseListGrid.Rows.Add("", null, "", "");

        for (int i = UseListGrid.RowCount - 1; i >= 0; i--)
        {
            if (UseListGrid.Rows[i].Cells[0].Value.ToString() == SkillListGrid.Rows[SelectRow].Cells[0].Value.ToString())
                UseListGrid.Rows.RemoveAt(i);
            if (UseListGrid.Rows[i].Cells[3].Value.ToString() == comboBox1.Text)
                UseListGrid.Rows.RemoveAt(i);
        }

        for (int i = 0; i < UseListGrid.Columns.Count; i++)
            UseListGrid.Rows[UseListGrid.RowCount - 1].Cells[i].Value = SkillListGrid.Rows[SelectRow].Cells[i].Value;
        UseListGrid.Rows[UseListGrid.RowCount - 1].Cells[3].Value = comboBox1.Text;

        Skill.HotKeyList.Clear();
        for (int i = 0; i < UseListGrid.Rows.Count; i++)
        {
            string Char = UseListGrid.Rows[i].Cells[3].Value.ToString();
            string ID = UseListGrid.Rows[i].Cells[0].Value.ToString();
            Skill.HotKeyList.AddOrReplace((Input)Enum.Parse(typeof(Input), Char, true), ID);
        }

        string SkillID = SkillListGrid.Rows[SelectRow].Cells[0].Value.ToString();
        if (!Skill.LoadedList.Contains(SkillID))
            Skill.Load(SkillID);
        Skill.LoadedList.Add(SkillID);
        // comboBox1.Visible = false;

    }

    private void SkillForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }
}
