using System.Windows.Forms;

namespace MapleNecrocer;

public enum GridType
{
    Normal,
    Icon,
    Dye
}
public class BaseDataGridView : DataGridView
{
    public BaseDataGridView(int IDWidth, int NameWidth, bool IconGrid=false)
    {
        //  Dock = System.Windows.Forms.DockStyle.Fill;
        ColumnHeadersVisible = false;
        RowHeadersVisible = false;
        AllowUserToAddRows = false;
        SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.ShowCellToolTips = false;
        DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        this.MultiSelect = false;
        this.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        
        this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        this.AllowUserToResizeRows = false;
        this.AllowUserToResizeColumns = false;
        this.ScrollBars= ScrollBars.Vertical;
        this.BackgroundColor = Color.White;
        RowTemplate.Height = 35;
        //  DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9);
        var ID = new DataGridViewTextBoxColumn();
        ID.DataPropertyName = "ID";
        ID.HeaderText = "ID";
        ID.Name = "propID";
        ID.ReadOnly = true;
        ID.Width = IDWidth;
        //
        var Icon = new DataGridViewImageColumn();
        Icon.DataPropertyName = "Icon";
        Icon.HeaderText = "圖示";
        Icon.Name = "propBitmap";
        Icon.ReadOnly = true;
        Icon.Width = 50;
        
        //
        var Name = new DataGridViewTextBoxColumn();
        Name.DataPropertyName = "NameProperty";
        Name.HeaderText = "名稱";
        Name.Name = "propName";
        Name.ReadOnly = true;
        Name.Width = NameWidth;
        if (IconGrid)
        {
            this.RowTemplate.Height = 35;
            DefaultCellStyle.Font = new Font("Tahoma", 16, GraphicsUnit.Pixel);
            Columns.AddRange(ID, Icon, Name);
            
        }
        else
        {
            this.RowTemplate.Height = 18;
            CellBorderStyle = DataGridViewCellBorderStyle.None;
            DefaultCellStyle.Font = new Font("Arial", 13, GraphicsUnit.Pixel);
            Columns.AddRange(ID, Name);
        }
       

    }

   
}

public class DataGridViewEx : BaseDataGridView
{
    public DataGridViewEx(int IDWidth, int NameWidth, int Left, int Top, int Width, int Height, bool IconGrid, Control Parent) : base(IDWidth, NameWidth, IconGrid)
    {
        this.Left = Left;
        this.Top = Top;
        this.Width = Width;
        this.Height = Height;
        this.Parent = Parent;
       

        SearchGrid = new(IDWidth, NameWidth, IconGrid);
        SearchGrid.Left = Left;
        SearchGrid.Top = Top;
        SearchGrid.Width = Width;
        SearchGrid.Height = Height;
        SearchGrid.Parent = Parent;
        SearchGrid.Visible = false;
    }
    public BaseDataGridView SearchGrid;

    string Trim(string s)
    {

        return s.Trim(' ');
    }
    public void Search(string Text)
    {

        foreach (DataGridViewRow i in SearchGrid.Rows)
        {
            if (i != null)
                i.Dispose();
        }

        var SearchStr = Trim(Text);
        if (SearchStr == "")
        {
            this.Visible = true;
            SearchGrid.Visible = false;
        }
        else
        {
            SearchGrid.Rows.Clear();
            var Row = new DataGridViewRow();

            for (int i = 0; i < this.RowCount; i++)
            {
                for (int j = 0; j < this.Columns.Count; j++)
                {
                    if (this.Rows[i].Cells[j].Value is string)
                    {
                        if (this.Rows[i].Cells[j].Value.ToString().IndexOf(SearchStr, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Row = (DataGridViewRow)this.Rows[i].Clone();
                            for (int j2 = 0; j2 < this.Columns.Count; j2++)
                                Row.Cells[j2].Value = this.Rows[i].Cells[j2].Value;
                            SearchGrid.Rows.Add(Row);
                            break;
                        }
                    }
                }
            }
            this.Visible = false;
            SearchGrid.Visible = true;
            SearchGrid.Refresh();
        }

    }


}