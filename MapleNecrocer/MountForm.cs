using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WzComparerR2.CharaSim;

namespace MapleNecrocer;

public partial class MountForm : Form
{
    public MountForm()
    {
        InitializeComponent();
        Instance = this;
    }
    public static MountForm Instance;
    public DataGridViewEx MountListGrid;


    void CellClick(BaseDataGridView DataGrid, DataGridViewCellEventArgs e)
    {
        //if (Morph.IsUse)
        // return;
        var ID = DataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
        TamingMob.Delete();

        MapleChair.Delete();
        ItemEffect.Delete(EffectType.Chair);

        TamingMob.IsChairTaming = false;
        TamingMob.Create(ID);
        TamingMob.IsUse = true;

    }

    private void MountForm_Shown(object sender, EventArgs e)
    {
        this.FormClosing += (s, e1) =>
        {
            this.Hide();
            e1.Cancel = true;
        };
        MountListGrid = new(90, 174, 0, 0, 220, 800, true, tabControl1.TabPages[0]);
        MountListGrid.Dock = DockStyle.Fill;
        MountListGrid.SearchGrid.Dock = DockStyle.Fill;

        var Graphic = MountListGrid.CreateGraphics();
        var Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
        Graphic.DrawString("Loading...", Font, Brushes.Black, 10, 50);
        MountListGrid.CellClick += (s, e) =>
        {
            CellClick(MountListGrid, e);
        };

        MountListGrid.SearchGrid.CellClick += (s, e) =>
        {
            CellClick(MountListGrid.SearchGrid, e);
        };


        string MountName = "";
        Bitmap Bmp = null;
        Win32.SendMessage(MountListGrid.Handle, false);
        foreach (var Img in Wz.GetNodeA("Character/TamingMob").Nodes)
        {
            if (Img.Text.LeftStr(4) == "0191" || Img.Text.LeftStr(4) == "0198")
                continue;
            string ID = Img.ImgID();
            if (Wz.GetNode("String/Eqp.img/Eqp/Taming/" + ID.IntID()) != null)
                MountName = Wz.GetNode("String/Eqp.img/Eqp/Taming/" + ID.IntID()).GetStr("name");
            else
                MountName = "";
            var Entry = Wz.GetNode("Character/TamingMob/" + Img.Text + "/info/icon");
            if (Entry != null)
                Bmp = Entry.ExtractPng();
            MountListGrid.Rows.Add(ID, Bmp, MountName);
        }


        Dictionary<string, string> Dict = new();

        for (int i = 11; i <= 31; i++)
        {
            if (Wz.GetNode("Skill/" + "80011" + i.ToString() + ".img") != null)
            {
                foreach (var Iter in Wz.GetNode("Skill/80011" + i.ToString() + ".img/skill").Nodes)
                {
                    if (Iter.Nodes["vehicleID"] != null)
                        Dict.AddOrReplace("0" + Iter.Nodes["vehicleID"].ToStr(), Iter.Text);
                }
            }
        }

        for (int i = 0; i <= 9; i++)
        {
            if (Wz.GetNode("Skill/" + "8000" + i.ToString() + ".img") != null)
            {
                foreach (var Iter in Wz.GetNode("Skill/8000" + i.ToString() + ".img/skill").Nodes)
                {
                    if (Iter.Nodes["vehicleID"] != null)
                        Dict.AddOrReplace("0" + Iter.Nodes["vehicleID"].ToStr(), Iter.Text);
                }
            }
        }

        Dict.AddOrReplace("01932026", "80001015");
        Dict.AddOrReplace("01932064", "80001060");
        Dict.AddOrReplace("01932186", "80001060");
        Dict.AddOrReplace("01932003", "80001024");
        Dict.AddOrReplace("01932004", "80001024");
        Dict.AddOrReplace("01932065", "80001082");
        Dict.AddOrReplace("01932093", "80001082");
        Dict.AddOrReplace("01932066", "80001116");
        Dict.AddOrReplace("01932187", "80001312");
        Dict.AddOrReplace("01932188", "80001313");
        Dict.AddOrReplace("01932189", "80001314");
        Dict.AddOrReplace("01932190", "80001315");
        Dict.AddOrReplace("01932007", "80001004");
        Dict.AddOrReplace("01932019", "80001004");
        Dict.AddOrReplace("01932027", "80001016");
        Dict.AddOrReplace("01992002", "80001016");
        Dict.AddOrReplace("01932028", "80001017");
        Dict.AddOrReplace("01932034", "80001450");
        Dict.AddOrReplace("01932226", "80001450");
        Dict.AddOrReplace("01932284", "80001450");
        Dict.AddOrReplace("01992003", "80001066");
        Dict.AddOrReplace("01932177", "80001067");
        Dict.AddOrReplace("01992004", "80001067");
        Dict.AddOrReplace("01932013", "80001009");
        Dict.AddOrReplace("01932009", "80001006");
        Dict.AddOrReplace("01932008", "80001005");
        Dict.AddOrReplace("01932495", "80001005");
        Dict.AddOrReplace("01932191", "80001316");
        Dict.AddOrReplace("01932192", "80001317");
        Dict.AddOrReplace("01932193", "80001075");
        Dict.AddOrReplace("01992012", "80001075");
        Dict.AddOrReplace("01932014", "80001010");
        Dict.AddOrReplace("01932258", "80001077");
        Dict.AddOrReplace("01992014", "80001077");
        Dict.AddOrReplace("01932078", "80001118");
        Dict.AddOrReplace("01932194", "80001118");
        Dict.AddOrReplace("01932332", "80001533");
        Dict.AddOrReplace("01932102", "80001346");
        Dict.AddOrReplace("01932206", "80001346");
        Dict.AddOrReplace("01932002", "80001449");
        Dict.AddOrReplace("01932118", "80001449");
        Dict.AddOrReplace("01932225", "80001449");
        Dict.AddOrReplace("01932285", "80001449");
        Dict.AddOrReplace("01992006", "80001069");
        Dict.AddOrReplace("01992010", "80001008");
        Dict.AddOrReplace("01992007", "80001007");
        Dict.AddOrReplace("01932250", "80001074");
        Dict.AddOrReplace("01992011", "80001074");
        Dict.AddOrReplace("01932012", "80001074");
        Dict.AddOrReplace("01932011", "80001007");
        Dict.AddOrReplace("01932018", "80001046");
        Dict.AddOrReplace("01932023", "80001013");
        Dict.AddOrReplace("01932025", "80001014");
        Dict.AddOrReplace("01932029", "80001030");
        Dict.AddOrReplace("01932054", "80001030");
        Dict.AddOrReplace("01932062", "80001030");
        Dict.AddOrReplace("01932035", "80001019");
        Dict.AddOrReplace("01932227", "80001019");
        Dict.AddOrReplace("01932283", "80001019");
        Dict.AddOrReplace("01932038", "80001031");
        Dict.AddOrReplace("01932055", "80001031");
        Dict.AddOrReplace("01932172", "80001031");
        Dict.AddOrReplace("01992013", "80001031");
        Dict.AddOrReplace("01932041", "80001020");
        Dict.AddOrReplace("01932043", "80001021");
        Dict.AddOrReplace("01932044", "80001022");
        Dict.AddOrReplace("01932045", "80001054");
        Dict.AddOrReplace("01932058", "80001084");
        Dict.AddOrReplace("01932095", "80001175");
        Dict.AddOrReplace("01932122", "80001175");
        Dict.AddOrReplace("01932046", "80001044");
        Dict.AddOrReplace("01932059", "80001044");
        Dict.AddOrReplace("01932090", "80001044");
        Dict.AddOrReplace("01932197", "80001044");
        Dict.AddOrReplace("01932047", "80001056");
        Dict.AddOrReplace("01932061", "80001056");
        Dict.AddOrReplace("01932094", "80001056");
        Dict.AddOrReplace("01932048", "80001023");
        Dict.AddOrReplace("01932245", "80001023");
        Dict.AddOrReplace("01932049", "80001027");
        Dict.AddOrReplace("01932050", "80001028");
        Dict.AddOrReplace("01932053", "80001029");
        Dict.AddOrReplace("01932088", "80001029");
        Dict.AddOrReplace("01992008", "80001029");
        Dict.AddOrReplace("01932056", "80001032");
        Dict.AddOrReplace("01932080", "80001032");
        Dict.AddOrReplace("01932196", "80001032");
        Dict.AddOrReplace("01932057", "80001033");
        Dict.AddOrReplace("01932060", "80001413");
        Dict.AddOrReplace("01932219", "80001413");
        Dict.AddOrReplace("01932071", "80001061");
        Dict.AddOrReplace("01932072", "80001117");
        Dict.AddOrReplace("01932081", "80001062");
        Dict.AddOrReplace("01932083", "80001057");
        Dict.AddOrReplace("01932089", "80001039");
        Dict.AddOrReplace("01932092", "80001121");
        Dict.AddOrReplace("01932096", "80001090");
        Dict.AddOrReplace("01932097", "80001112");
        Dict.AddOrReplace("01932255", "80001112");
        Dict.AddOrReplace("01932098", "80001113");
        Dict.AddOrReplace("01932320", "80001113");
        Dict.AddOrReplace("01932099", "80001114");
        Dict.AddOrReplace("01932105", "80001124");
        Dict.AddOrReplace("01932109", "80001131");
        Dict.AddOrReplace("01932211", "80001131");
        Dict.AddOrReplace("01932110", "80001137");
        Dict.AddOrReplace("01932112", "80001142");
        Dict.AddOrReplace("01932208", "80001142");
        Dict.AddOrReplace("01932113", "80001144");
        Dict.AddOrReplace("01932136", "80001144");
        Dict.AddOrReplace("01932114", "80001148");
        Dict.AddOrReplace("01932137", "80001148");
        Dict.AddOrReplace("01932115", "80001149");
        Dict.AddOrReplace("01932138", "80001149");
        Dict.AddOrReplace("01932116", "80001156");
        Dict.AddOrReplace("01932117", "80001157");
        Dict.AddOrReplace("01932120", "80001173");
        Dict.AddOrReplace("01932121", "80001174");
        Dict.AddOrReplace("01932123", "80001179");
        Dict.AddOrReplace("01932124", "80001180");
        Dict.AddOrReplace("01932131", "80001183");
        Dict.AddOrReplace("01932127", "80001184");
        Dict.AddOrReplace("01932132", "80001184");
        Dict.AddOrReplace("01932128", "80001185");
        Dict.AddOrReplace("01932133", "80001185");
        Dict.AddOrReplace("01932129", "80001186");
        Dict.AddOrReplace("01932134", "80001186");
        Dict.AddOrReplace("01932130", "80001187");
        Dict.AddOrReplace("01932135", "80001187");
        Dict.AddOrReplace("01932139", "80001196");
        Dict.AddOrReplace("01932140", "80001197");
        Dict.AddOrReplace("01932145", "80001197");
        Dict.AddOrReplace("01932143", "80001220");
        Dict.AddOrReplace("01932146", "80001223");
        Dict.AddOrReplace("01932144", "80001221");
        Dict.AddOrReplace("01932147", "80001221");
        Dict.AddOrReplace("01932148", "80001228");
        Dict.AddOrReplace("01932149", "80001228");
        Dict.AddOrReplace("01932150", "80001229");
        Dict.AddOrReplace("01932151", "80001230");
        Dict.AddOrReplace("01932152", "80001241");
        Dict.AddOrReplace("01932349", "80001241");
        Dict.AddOrReplace("01932153", "80001237");
        Dict.AddOrReplace("01992036", "80001237");
        Dict.AddOrReplace("01932154", "80001240");
        Dict.AddOrReplace("01932156", "80001243");
        Dict.AddOrReplace("01932157", "80001244");
        Dict.AddOrReplace("01932158", "80001245");
        Dict.AddOrReplace("01932159", "80001246");
        Dict.AddOrReplace("01932348", "80001246");
        Dict.AddOrReplace("01992026", "80001246");
        Dict.AddOrReplace("01992050", "80001246");
        Dict.AddOrReplace("01932161", "80001257");
        Dict.AddOrReplace("01992035", "80001257");
        Dict.AddOrReplace("01932162", "80001258");
        Dict.AddOrReplace("01992032", "80001258");
        Dict.AddOrReplace("01932163", "80001260");
        Dict.AddOrReplace("01932222", "80001260");
        Dict.AddOrReplace("01932164", "80001261");
        Dict.AddOrReplace("01992027", "80001261");
        Dict.AddOrReplace("01932165", "80001277");
        Dict.AddOrReplace("01932166", "80001277");
        Dict.AddOrReplace("01932167", "80001285");
        Dict.AddOrReplace("01932168", "80001285");
        Dict.AddOrReplace("01932169", "80001285");
        Dict.AddOrReplace("01932174", "80001285");
        Dict.AddOrReplace("01932170", "80001289");
        Dict.AddOrReplace("01932175", "80001289");
        Dict.AddOrReplace("01932171", "80001290");
        Dict.AddOrReplace("01932176", "80001290");
        Dict.AddOrReplace("01932173", "80001292");
        Dict.AddOrReplace("01932178", "80001068");
        Dict.AddOrReplace("01932231", "80001068");
        Dict.AddOrReplace("01992005", "80001068");
        Dict.AddOrReplace("01932195", "80001320");
        Dict.AddOrReplace("01932198", "80001327");
        Dict.AddOrReplace("01932199", "80001331");
        Dict.AddOrReplace("01932203", "80001331");
        Dict.AddOrReplace("01932200", "80001336");
        Dict.AddOrReplace("01932201", "80001338");
        Dict.AddOrReplace("01932202", "80001343");
        Dict.AddOrReplace("01932204", "80001345");
        Dict.AddOrReplace("01932205", "80001333");
        Dict.AddOrReplace("01932207", "80001347");
        Dict.AddOrReplace("01932212", "80001355");
        Dict.AddOrReplace("01932214", "80001387");
        Dict.AddOrReplace("01932216", "80001398");
        Dict.AddOrReplace("01932217", "80001329");
        Dict.AddOrReplace("01992029", "80001329");
        Dict.AddOrReplace("01932218", "80001411");
        Dict.AddOrReplace("01932220", "80001419");
        Dict.AddOrReplace("01932221", "80001421");
        Dict.AddOrReplace("01932223", "80001404");
        Dict.AddOrReplace("01932224", "80001435");
        Dict.AddOrReplace("01932228", "80001453");
        Dict.AddOrReplace("01932230", "80001454");
        Dict.AddOrReplace("01932232", "80001002");
        Dict.AddOrReplace("01932235", "80001484");
        Dict.AddOrReplace("01932236", "80001531");
        Dict.AddOrReplace("01932253", "80001531");
        Dict.AddOrReplace("01932237", "80001440");
        Dict.AddOrReplace("01932238", "80001441");
        Dict.AddOrReplace("01932239", "80001442");
        Dict.AddOrReplace("01932240", "80001443");
        Dict.AddOrReplace("01932241", "80001444");
        Dict.AddOrReplace("01932242", "80001445");
        Dict.AddOrReplace("01932243", "80001447");
        Dict.AddOrReplace("01932244", "80001508");
        Dict.AddOrReplace("01932247", "80001490");
        Dict.AddOrReplace("01932248", "80001491");
        Dict.AddOrReplace("01932249", "80001492");
        Dict.AddOrReplace("01932262", "80001492");
        Dict.AddOrReplace("01992043", "80001492");
        Dict.AddOrReplace("01932251", "80001505");
        Dict.AddOrReplace("01932252", "80001517");
        Dict.AddOrReplace("01932254", "80001549");
        Dict.AddOrReplace("01932256", "80001552");
        Dict.AddOrReplace("01932259", "80001557");
        Dict.AddOrReplace("01932261", "80001584");
        Dict.AddOrReplace("01932276", "80001584");
        Dict.AddOrReplace("01932263", "80001561");
        Dict.AddOrReplace("01932264", "80001562");
        Dict.AddOrReplace("01932265", "80001563");
        Dict.AddOrReplace("01932266", "80001564");
        Dict.AddOrReplace("01932267", "80001565");
        Dict.AddOrReplace("01932268", "80001566");
        Dict.AddOrReplace("01932269", "80001567");
        Dict.AddOrReplace("01932270", "80001568");
        Dict.AddOrReplace("01932271", "80001569");
        Dict.AddOrReplace("01932272", "80001570");
        Dict.AddOrReplace("01932273", "80001571");
        Dict.AddOrReplace("01932274", "80001572");
        Dict.AddOrReplace("01932275", "80001582");
        Dict.AddOrReplace("01932310", "80001582");
        Dict.AddOrReplace("01932333", "80001582");
        Dict.AddOrReplace("01932278", "80011111");
        Dict.AddOrReplace("01932279", "80001586");
        Dict.AddOrReplace("01932280", "80011127");
        Dict.AddOrReplace("01932281", "80001776");
        Dict.AddOrReplace("01932324", "80001776");
        Dict.AddOrReplace("01932282", "80011129");
        Dict.AddOrReplace("01932286", "80002257");
        Dict.AddOrReplace("01932429", "80002257");
        Dict.AddOrReplace("01932287", "80011135");
        Dict.AddOrReplace("01932288", "80001778");
        Dict.AddOrReplace("01932325", "80001778");
        Dict.AddOrReplace("01932289", "80001766");
        Dict.AddOrReplace("01932321", "80001766");
        Dict.AddOrReplace("01932294", "80001630");
        Dict.AddOrReplace("01932296", "80001630");
        Dict.AddOrReplace("01932300", "80001630");
        Dict.AddOrReplace("01932295", "80001631");
        Dict.AddOrReplace("01932298", "80001631");
        Dict.AddOrReplace("01932301", "80001631");
        Dict.AddOrReplace("01932297", "80001592");
        Dict.AddOrReplace("01932357", "80001592");
        Dict.AddOrReplace("01932304", "80001701");
        Dict.AddOrReplace("01932305", "80001703");
        Dict.AddOrReplace("01932306", "80001707");
        Dict.AddOrReplace("01932307", "80001708");
        Dict.AddOrReplace("01932311", "80001713");
        Dict.AddOrReplace("01932314", "80001507");
        Dict.AddOrReplace("01932319", "80001763");
        Dict.AddOrReplace("01932323", "80001775");
        Dict.AddOrReplace("01932334", "80001784");
        Dict.AddOrReplace("01932335", "80001786");
        Dict.AddOrReplace("01932336", "80001790");
        Dict.AddOrReplace("01932337", "80001792");
        Dict.AddOrReplace("01932338", "80001796");
        Dict.AddOrReplace("01932367", "80001796");
        Dict.AddOrReplace("01932341", "80001811");
        Dict.AddOrReplace("01932342", "80001813");
        Dict.AddOrReplace("01932460", "80001813");
        Dict.AddOrReplace("01932343", "80001866");
        Dict.AddOrReplace("01932344", "80001867");
        Dict.AddOrReplace("01932345", "80001868");
        Dict.AddOrReplace("01932346", "80011256");
        Dict.AddOrReplace("01932347", "80001870");
        Dict.AddOrReplace("01932353", "80001918");
        Dict.AddOrReplace("01932354", "80001920");
        Dict.AddOrReplace("01932358", "80001934");
        Dict.AddOrReplace("01932359", "80001935");
        Dict.AddOrReplace("01932360", "80001921");
        Dict.AddOrReplace("01932361", "80001923");
        Dict.AddOrReplace("01932362", "80001931");
        Dict.AddOrReplace("01932363", "80001932");
        Dict.AddOrReplace("01932367", "80001942");
        Dict.AddOrReplace("01932368", "80001950");
        Dict.AddOrReplace("01932371", "80001950");
        Dict.AddOrReplace("01932369", "80001951");
        Dict.AddOrReplace("01932372", "80001951");
        Dict.AddOrReplace("01932370", "80001952");
        Dict.AddOrReplace("01932373", "80001952");
        Dict.AddOrReplace("01932374", "80001956");
        Dict.AddOrReplace("01932375", "80001958");
        Dict.AddOrReplace("01932377", "80001975");
        Dict.AddOrReplace("01932378", "80001977");
        Dict.AddOrReplace("01932379", "80001980");
        Dict.AddOrReplace("01932380", "80001982");
        Dict.AddOrReplace("01932381", "80001986");
        Dict.AddOrReplace("01932382", "80001986");
        Dict.AddOrReplace("01932383", "80001988");
        Dict.AddOrReplace("01932526", "80001988");
        Dict.AddOrReplace("01932384", "80001989");
        Dict.AddOrReplace("01932385", "80001990");
        Dict.AddOrReplace("01932386", "80001991");
        Dict.AddOrReplace("01932387", "80001991");
        Dict.AddOrReplace("01932388", "80001993");
        Dict.AddOrReplace("01932389", "80002221");
        Dict.AddOrReplace("01932402", "80002221");
        Dict.AddOrReplace("01932390", "80011359");
        Dict.AddOrReplace("01932404", "80011359");
        Dict.AddOrReplace("01932391", "80001997");
        Dict.AddOrReplace("01932392", "80001995");
        Dict.AddOrReplace("01932393", "80002200");
        Dict.AddOrReplace("01932394", "80002201");
        Dict.AddOrReplace("01932395", "80002202");
        Dict.AddOrReplace("01932407", "80002202");
        Dict.AddOrReplace("01932590", "80002202");
        Dict.AddOrReplace("01932396", "80011395");
        Dict.AddOrReplace("01932397", "80011397");
        Dict.AddOrReplace("01932398", "80002219");
        Dict.AddOrReplace("01932399", "80002204");
        Dict.AddOrReplace("01932408", "80002204");
        Dict.AddOrReplace("01932400", "80002205");
        Dict.AddOrReplace("01932401", "80002220");
        Dict.AddOrReplace("01932403", "80002222");
        Dict.AddOrReplace("01932404", "80011405");
        Dict.AddOrReplace("01932405", "80002223");
        Dict.AddOrReplace("01932406", "80002225");
        Dict.AddOrReplace("01932409", "80002229");
        Dict.AddOrReplace("01932410", "80002233");
        Dict.AddOrReplace("01932411", "80002234");
        Dict.AddOrReplace("01932412", "80002235");
        Dict.AddOrReplace("01932413", "80011421");
        Dict.AddOrReplace("01932414", "80002236");
        Dict.AddOrReplace("01932415", "80002238");
        Dict.AddOrReplace("01932416", "80011424");
        Dict.AddOrReplace("01932418", "80002240");
        Dict.AddOrReplace("01932461", "80002240");
        Dict.AddOrReplace("01932419", "80002242");
        Dict.AddOrReplace("01932420", "80002244");
        Dict.AddOrReplace("01932421", "80002248");
        Dict.AddOrReplace("01932422", "80002250");
        Dict.AddOrReplace("01932423", "80002252");
        Dict.AddOrReplace("01932424", "80002252");
        Dict.AddOrReplace("01932425", "80011431");
        Dict.AddOrReplace("01932426", "80002270");
        Dict.AddOrReplace("01932427", "80002259");
        Dict.AddOrReplace("01932428", "80002258");
        Dict.AddOrReplace("01932430", "80002260");
        Dict.AddOrReplace("01932431", "80002262");
        Dict.AddOrReplace("01932432", "80002265");
        Dict.AddOrReplace("01932433", "80011438");
        Dict.AddOrReplace("01932434", "80002266");
        Dict.AddOrReplace("01932435", "80011443");
        Dict.AddOrReplace("01932436", "80011445");
        Dict.AddOrReplace("01932437", "80011447");
        Dict.AddOrReplace("01932438", "80002272");
        Dict.AddOrReplace("01932439", "80002271");
        Dict.AddOrReplace("01932440", "80011436");
        Dict.AddOrReplace("01932441", "80002276");
        Dict.AddOrReplace("01932442", "80002278");
        Dict.AddOrReplace("01932443", "80011451");
        Dict.AddOrReplace("01932444", "80011453");
        Dict.AddOrReplace("01932445", "80002287");
        Dict.AddOrReplace("01932446", "80002289");
        Dict.AddOrReplace("01932447", "80002315");
        Dict.AddOrReplace("01932448", "80002294");
        Dict.AddOrReplace("01932449", "80002296");
        Dict.AddOrReplace("01932450", "80002299");
        Dict.AddOrReplace("01932451", "80002203");
        Dict.AddOrReplace("01932452", "80002302");
        Dict.AddOrReplace("01932453", "80002304");
        Dict.AddOrReplace("01932454", "80002305");
        Dict.AddOrReplace("01932455", "80002307");
        Dict.AddOrReplace("01932456", "80011486");
        Dict.AddOrReplace("01932457", "80011488");
        Dict.AddOrReplace("01932458", "80002309");
        Dict.AddOrReplace("01932459", "80002313");
        Dict.AddOrReplace("01932462", "80002317");
        Dict.AddOrReplace("01932463", "80002318");
        Dict.AddOrReplace("01932464", "80002319");
        Dict.AddOrReplace("01932465", "80002321");
        Dict.AddOrReplace("01932466", "80011500");
        Dict.AddOrReplace("01932467", "80011502");
        Dict.AddOrReplace("01932468", "80002335");
        Dict.AddOrReplace("01932469", "80011506");
        Dict.AddOrReplace("01932470", "80002345");
        Dict.AddOrReplace("01932471", "80002347");
        Dict.AddOrReplace("01932472", "80002349");
        Dict.AddOrReplace("01932473", "80002349");
        Dict.AddOrReplace("01932474", "80002356");
        Dict.AddOrReplace("01932475", "80002358");
        Dict.AddOrReplace("01932476", "80002351");
        Dict.AddOrReplace("01932478", "80002351");
        Dict.AddOrReplace("01932477", "80002352");
        Dict.AddOrReplace("01932480", "80002352");
        Dict.AddOrReplace("01932479", "80002361");
        Dict.AddOrReplace("01932481", "80011531");
        Dict.AddOrReplace("01942382", "80011533");
        Dict.AddOrReplace("01932483", "80002367");
        Dict.AddOrReplace("01932484", "80011524");
        Dict.AddOrReplace("01942385", "80011535");
        Dict.AddOrReplace("01932486", "80002369");
        Dict.AddOrReplace("01942387", "80011546");
        Dict.AddOrReplace("01932488", "80002372");
        Dict.AddOrReplace("01932492", "80002372");
        Dict.AddOrReplace("01932489", "80002373");
        Dict.AddOrReplace("01932493", "80002373");
        Dict.AddOrReplace("01932490", "80002374");
        Dict.AddOrReplace("01932494", "80002374");
        Dict.AddOrReplace("01932491", "80002375");
        Dict.AddOrReplace("01932496", "80011541");
        Dict.AddOrReplace("01932497", "80002392");
        Dict.AddOrReplace("01932498", "80002400");
        Dict.AddOrReplace("01932499", "80002402");
        Dict.AddOrReplace("01932500", "80002417");
        Dict.AddOrReplace("01932501", "80002418");
        Dict.AddOrReplace("01932572", "80002418");
        Dict.AddOrReplace("01932502", "80011551");
        Dict.AddOrReplace("01932503", "80011554");
        Dict.AddOrReplace("01932530", "80011554");
        Dict.AddOrReplace("01932539", "80011554");
        Dict.AddOrReplace("01932504", "80002425");
        Dict.AddOrReplace("01932505", "80002425");
        Dict.AddOrReplace("01932506", "80002427");
        Dict.AddOrReplace("01932507", "80002429");
        Dict.AddOrReplace("01932511", "80002429");
        Dict.AddOrReplace("01932508", "80002431");
        Dict.AddOrReplace("01932509", "80011571");
        Dict.AddOrReplace("01932510", "80002433");
        Dict.AddOrReplace("01932511", "80002435");
        Dict.AddOrReplace("01932512", "80011580");
        Dict.AddOrReplace("01932513", "80002437");
        Dict.AddOrReplace("01932514", "80002439");
        Dict.AddOrReplace("01932515", "80002443");
        Dict.AddOrReplace("01932516", "80002441");
        Dict.AddOrReplace("01932517", "80002446");
        Dict.AddOrReplace("01932518", "80002447");
        Dict.AddOrReplace("01932519", "80011639");
        Dict.AddOrReplace("01932520", "80011642");
        Dict.AddOrReplace("01932521", "80002448");
        Dict.AddOrReplace("01932522", "80002450");
        Dict.AddOrReplace("01932523", "80011646");
        Dict.AddOrReplace("01932524", "80002454");
        Dict.AddOrReplace("01932525", "80002424");
        Dict.AddOrReplace("01932527", "80002546");
        Dict.AddOrReplace("01932528", "80002547");
        Dict.AddOrReplace("01932529", "80011698");
        Dict.AddOrReplace("01932535", "80002569");
        Dict.AddOrReplace("01932536", "80002569");
        Dict.AddOrReplace("01932537", "80002571");
        Dict.AddOrReplace("01932538", "80002573");
        Dict.AddOrReplace("01932540", "80002622");
        Dict.AddOrReplace("01932541", "80011712");
        Dict.AddOrReplace("01932542", "80002585");
        Dict.AddOrReplace("01932543", "80002628");
        Dict.AddOrReplace("01932544", "80002630");
        Dict.AddOrReplace("01932545", "80011706");
        Dict.AddOrReplace("01932546", "80002625");
        Dict.AddOrReplace("01932548", "80002625");
        Dict.AddOrReplace("01932549", "80002625");
        Dict.AddOrReplace("01932547", "80011721");
        Dict.AddOrReplace("01932550", "80002648");
        Dict.AddOrReplace("01932551", "80002650");
        Dict.AddOrReplace("01932552", "80002594");
        Dict.AddOrReplace("01932553", "80002595");
        Dict.AddOrReplace("01932554", "80011733");
        Dict.AddOrReplace("01932555", "80011737");
        Dict.AddOrReplace("01932556", "80002654");
        Dict.AddOrReplace("01932557", "80002655");
        Dict.AddOrReplace("01932559", "80011743");
        Dict.AddOrReplace("01932560", "80002659");
        Dict.AddOrReplace("01932562", "80002663");
        Dict.AddOrReplace("01932563", "80002665");
        Dict.AddOrReplace("01932564", "80002667");
        Dict.AddOrReplace("01932565", "80011758");
        Dict.AddOrReplace("01932566", "80011760");
        Dict.AddOrReplace("01932567", "80002668");
        Dict.AddOrReplace("01932569", "80002668");
        Dict.AddOrReplace("01932570", "80011784");
        Dict.AddOrReplace("01932571", "80002698");
        Dict.AddOrReplace("01932572", "80002699");
        Dict.AddOrReplace("01932573", "80002702");
        Dict.AddOrReplace("01932574", "80011773");
        Dict.AddOrReplace("01932575", "80011775");
        Dict.AddOrReplace("01932576", "80011777");
        Dict.AddOrReplace("01932577", "80011779");
        Dict.AddOrReplace("01932578", "80011798");
        Dict.AddOrReplace("01932579", "80011791");
        Dict.AddOrReplace("01932580", "80002712");
        Dict.AddOrReplace("01932581", "80002713");
        Dict.AddOrReplace("01932582", "80011806");
        Dict.AddOrReplace("01932583", "80011808");
        Dict.AddOrReplace("01932587", "80011819");
        Dict.AddOrReplace("01932588", "80011820");
        Dict.AddOrReplace("01932599", "80011825");
        Dict.AddOrReplace("01932600", "80011844");
        Dict.AddOrReplace("01932610", "80011850");
        Dict.AddOrReplace("01939000", "80001617");
        Dict.AddOrReplace("01939001", "80001619");
        Dict.AddOrReplace("01939002", "80001621");
        Dict.AddOrReplace("01939003", "80001623");
        Dict.AddOrReplace("01939004", "80001625");
        Dict.AddOrReplace("01939005", "80001627");
        Dict.AddOrReplace("01939006", "80001629");
        Dict.AddOrReplace("01939011", "80011732");
        Dict.AddOrReplace("01992015", "80001120");
        Dict.AddOrReplace("01992030", "80001330");
        Dict.AddOrReplace("01992031", "80001403");
        Dict.AddOrReplace("01932322", "80001769");
        Dict.AddOrReplace("01932416", "80001769");
        Dict.AddOrReplace("01932331", "80011238");
        Dict.AddOrReplace("01932558", "80002714");
        Dict.AddOrReplace("01932561", "80002661");
        Dict.AddOrReplace("01932584", "80002715");
        Dict.AddOrReplace("01932585", "80002717");
        Dict.AddOrReplace("01932586", "80002717");
        Dict.AddOrReplace("01932589", "80002735");
        Dict.AddOrReplace("01932590", "80002738");
        Dict.AddOrReplace("01932591", "80002736");
        Dict.AddOrReplace("01932592", "80011821");
        Dict.AddOrReplace("01932594", "80002740");
        Dict.AddOrReplace("01932595", "80002747");
        Dict.AddOrReplace("01932596", "80002752");
        Dict.AddOrReplace("01932597", "80002754");
        Dict.AddOrReplace("01932598", "80002756");
        Dict.AddOrReplace("01932601", "80002742");
        Dict.AddOrReplace("01932603", "80002742");
        Dict.AddOrReplace("01932603", "80002744");
        Dict.AddOrReplace("01932602", "80002743");
        Dict.AddOrReplace("01932612", "80002824");
        Dict.AddOrReplace("01932614", "80002843");
        Dict.AddOrReplace("01932615", "80002831");
        Dict.AddOrReplace("01932616", "80002845");
        Dict.AddOrReplace("01932617", "80002846");
        Dict.AddOrReplace("01932618", "80002853");
        Dict.AddOrReplace("01932619", "80002854");
        Dict.AddOrReplace("01992028", "80001296");


        for (int i = 0; i < MountListGrid.Rows.Count; i++)
        {
            string ID = MountListGrid.Rows[i].Cells[0].Value.ToString();
            if (Dict.ContainsKey(ID) && Wz.GetNode("String/Skill.img/" + Dict[ID]) != null)
                MountListGrid.Rows[i].Cells[2].Value = Wz.GetNode("String/Skill.img/" + Dict[ID]).GetStr("name");
        }

        Win32.SendMessage(MountListGrid.Handle, true);
        MountListGrid.Refresh();

        for (int i = 0; i < MountListGrid.Rows.Count; i++)
        {
            MountListGrid.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            MountListGrid.Rows[i].Cells[2].Style.Alignment = DataGridViewContentAlignment.TopLeft;
        }


    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        MountListGrid.Search(textBox1.Text);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        TamingMob.Delete();
        ItemEffect.Delete(EffectType.Chair);
        MapleChair.Delete();
        Game.Player.ResetAction = true;
        Game.Player.NewAction = Game.Player.StandType;
    }
}
