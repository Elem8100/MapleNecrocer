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

public partial class ChatRingForm : Form
{
    public ChatRingForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static ChatRingForm Instance;
    public DataGridViewEx ChatRingListGrid;

    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        if (ID == "01112243")
            return;
        ChatRingBalloon.Remove();
        var chatRingBalloon = new ChatRingBalloon(EngineFunc.SpriteEngine);
        //  chatRingBalloon.Msg = richTextBox1.Text;
        if (Wz.Region == "GMS")
        {
            chatRingBalloon.Msg = richTextBox1.Text;
            //Npc.Msgs.Add(Str1);
        }
        else
        {
            string Str1 = chatRingBalloon.Msg = richTextBox1.Text;
            Str1 = Str1.Replace(" ", "");
            chatRingBalloon.Msg = Npc.ReString(Str1);
        }

        int TagNum = Wz.GetInt("Character/Ring/" + ID + ".img" + "/info/chatBalloon");
        chatRingBalloon.SetStyle(TagNum);
        chatRingBalloon.ReDraw();

    }
    private void ChatRingForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };

        ChatRingListGrid = new(90, 179, 0, 0, 220, 400, true, panel1);
        ChatRingListGrid.Dock = DockStyle.Fill;
        ChatRingListGrid.SearchGrid.Dock = DockStyle.Fill;
        ChatRingListGrid.RowTemplate.Height = 40;

        var Graphic = ChatRingListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        ChatRingListGrid.CellClick += (s, e) =>
        {
            CellClick(ChatRingListGrid, e);
        };

        ChatRingListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(ChatRingListGrid.SearchGrid, e);
        };
        ChatRingListGrid.SetToolTipEvent(WzType.Character, this);

        foreach (var Img in Wz.GetNodes("Character/Ring"))
        {
            if (Img.Text.LeftStr(6) != "011122" && Img.Text.LeftStr(6) != "011150"  &&
                Img.Text.LeftStr(6) != "011152" && Img.Text.LeftStr(6) != "011154")
                continue;

            if (!Wz.HasNode("Character/Ring/" + Img.Text + "/info/chatBalloon"))
                continue;
            int TagNum = Wz.GetInt("Character/Ring/" + Img.Text + "/info/chatBalloon");

            if (!Wz.HasNode("UI/ChatBalloon.img/" + TagNum))
                continue;

            string ID = Img.ImgID();
            string ChatRingName = "";

            if (Wz.HasNode("String/Eqp.img/Eqp/Ring/" + ID.ToInt()))
                ChatRingName = Wz.GetStr("String/Eqp.img/Eqp/Ring/" + ID.ToInt() + "/name");

            Bitmap Icon = null;
            if (Wz.HasNode("Character/Ring/" + ID + ".img/info/icon"))
                Icon = Wz.GetBmp("Character/Ring/" + ID + ".img/info/icon");
            ChatRingListGrid.Rows.Add(ID, Icon, ChatRingName);

        }


    }

    private void button1_Click(object sender, EventArgs e)
    {
        ChatRingBalloon.Remove();
    }

    private void ChatRingForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        MainForm.Instance.ToolTipView.Visible = false;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        ChatRingListGrid.Search(textBox1.Text);
    }
}
