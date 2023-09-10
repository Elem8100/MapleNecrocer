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

public partial class WorldMapForm : Form
{
    public WorldMapForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static WorldMapForm Instance;
    private void WorldMapForm_Shown(object sender, EventArgs e)
    {


    }

    private void WorldMapForm_Load(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        MainForm.Instance.WorldMapListGrid.ColumnCount = 2;
        MainForm.Instance.WorldMapListGrid.Columns[0].Width = 203;
        MainForm.Instance.WorldMapListGrid.Columns[1].Width = 0;
        /*
        var StreetNameDict = new Dictionary<string, string>();
        foreach (var Iter in Wz.GetNodes("String/Map.img"))
        {
            foreach (var Iter2 in Iter.Nodes)
            {
                string ID = Iter2.Text.PadLeft(9, '0');
                var StreetName = Iter2.GetStr("streetName");
                if (!StreetNameDict.ContainsKey(ID))
                    StreetNameDict.Add(ID, StreetName);
            }
        }
      
        foreach (var img in Wz.GetNodes("Map/WorldMap"))
        {
            foreach (var Iter in Wz.GetNodes("Map/WorldMap/" + img.Text))
            {
                if (Iter.Text!="MapList")
                    continue;
                if(Iter.HasNode("0/mapNo/0"))
                {
                    if(StreetNameDict.ContainsKey(Iter.GetStr("0/mapNo/0")))
                      //  MainForm.Instance.WorldMapListGrid.Rows.Add(img.Text);
                        MainForm.Instance.WorldMapListGrid.Rows.Add(StreetNameDict[Iter.GetStr("0/mapNo/0")] );
                }
            }
        }
        */
        Dictionary<string, string> Dict = new();
        Dict.Add("BWorldMap.img", "Maple World");
        Dict.Add("GWorldMap.img", "Grandis");
        Dict.Add("MWorldMap.img", "Mirror World");
        Dict.Add("SWorldMap.img", "Star Planet");
        Dict.Add("WorldMap.img", "Maple World");
        Dict.Add("WorldMap000.img", "Maple Island");
        Dict.Add("WorldMap010.img", "Victoria Island");
        Dict.Add("WorldMap0101.img", "Kerning Tower");
        Dict.Add("WorldMap011.img", "Nautilus");
        Dict.Add("WorldMap012.img", "Sleepywood");
        Dict.Add("WorldMap0121.img", "Dark World Tree");
        Dict.Add("WorldMap015.img", "WorldMap015 ");
        Dict.Add("WorldMap016.img", "Shinsoo International School");
        Dict.Add("WorldMap017.img", "Ellinel Fairy Academy");
        Dict.Add("WorldMap018.img", "Gold Beach");
        Dict.Add("WorldMap019.img", "Mushroom Castle");
        Dict.Add("WorldMap020.img", "E1 Nath Mts.");
        Dict.Add("WorldMap021.img", "Dead Mine");
        Dict.Add("WorldMap022.img", "Lion King Castle");
        Dict.Add("WorldMap030.img", "Ludus Lake");
        Dict.Add("WorldMap031.img", "Clocktower Bottom Floor");
        Dict.Add("WorldMap032.img", "Ellin Forest ");
        Dict.Add("WorldMap033.img", "Fantastic Theme World");
        Dict.Add("WorldMap034.img", "Korean Folk Town");
        Dict.Add("WorldMap035.img", "Omega Sector");
        Dict.Add("WorldMap040.img", "Aqua Road");
        Dict.Add("WorldMap041.img", "Twisted Aqua Road");
        Dict.Add("WorldMap050.img", "Minar Forest");
        Dict.Add("WorldMap051.img", "Neo City ");
        Dict.Add("WorldMap052.img", "Stone Colossus");
        Dict.Add("WorldMap060.img", "Mu Lung Garden");
        Dict.Add("WorldMap061.img", "Golden Temple ");
        Dict.Add("WorldMap070.img", "Nihal Desert");
        Dict.Add("WorldMap071.img", "Ancient City Azwan");
        Dict.Add("WorldMap072.img", "Nihal Desert Trade Zone");
        Dict.Add("WorldMap080.img", "Temple of Time");
        Dict.Add("WorldMap081.img", "Gate of Future");
        Dict.Add("WorldMap082.img", "Arcane River");
        Dict.Add("WorldMap0821.img", "Vanishing Journey");
        Dict.Add("WorldMap0822.img", "Chu Chu Island");
        Dict.Add("WorldMap08221.img", "Lonely Chu Chu Island");
        Dict.Add("WorldMap0823.img", "Lachelein, the Dreaming City");
        Dict.Add("WorldMap0824.img", "Arcana");
        Dict.Add("WorldMap0825.img", "Morass, Swamp of Memory");
        Dict.Add("WorldMap0826.img", "Esfera ");
        Dict.Add("WorldMap090.img", "Ereve");
        Dict.Add("WorldMap100.img", "Rien");
        Dict.Add("WorldMap101.img", "Riena Strait");
        Dict.Add("WorldMap110.img", "Edelstein");
        Dict.Add("WorldMap111.img", "Scrapyard");
        Dict.Add("WorldMap120.img", "Crystal Garden");
        Dict.Add("WorldMap130.img", "Pantheon");
        Dict.Add("WorldMap140.img", "Heliseum");
        Dict.Add("WorldMap141.img", "Tyrant Castle");
        Dict.Add("WorldMap143.img", "Masteria");
        Dict.Add("WorldMap152.img", "皇陵之巔");
        Dict.Add("WorldMap153.img", "Commerci");
        Dict.Add("WorldMap154.img", "Arboren");
        Dict.Add("WorldMap155.img", "Meso Gear");
        Dict.Add("WorldMap160.img", "Zipangu");
        Dict.Add("WorldMap161.img", "Mushroom Shrine");
        Dict.Add("WorldMap163.img", "Ninja Castle");
        Dict.Add("WorldMap164.img", "未來 ");
        Dict.Add("WorldMap167.img", "東京 ");
        Dict.Add("WorldMap169.img", "Blackgate City");
        Dict.Add("WorldMap170.img", "Kritias");
        Dict.Add("WorldMap171.img", "Momijigaoka");
        Dict.Add("WorldMap172.img", "Spring Valley");
        Dict.Add("WorldMap173.img", "The Afterlands");
        Dict.Add("WorldMap174.img", "Eluna");
        Dict.Add("WorldMap175.img", "Beautyroid");
        Dict.Add("WorldMap176.img", "Abrup Basin");
        Dict.Add("WorldMap180.img", "Fox point Village");
        Dict.Add("WorldMap181.img", "Fox Valley");
        Dict.Add("WorldMap190.img", "Savage Terminal");
        Dict.Add("WorldMap191.img", "Sanctuary ");
        Dict.Add("WorldMap200.img", "Verdel");
        Dict.Add("WorldMap210.img", "Cheong-woon");
        Dict.Add("WorldMap220.img", " Ristonia");
        foreach (var img in Wz.GetNodes("Map/WorldMap"))
        {
            if (Dict.ContainsKey(img.Text))
                MainForm.Instance.WorldMapListGrid.Rows.Add(Dict[img.Text], img.Text);
            else
                MainForm.Instance.WorldMapListGrid.Rows.Add(img.Text, img.Text);
        }


    }
}
