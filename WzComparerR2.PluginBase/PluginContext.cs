using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WzComparerR2.WzLib;
using WzComparerR2.Common;
//using DevComponents.DotNetBar;
//using WzComparerR2.Controls;

namespace WzComparerR2.PluginBase
{
    public class PluginContext
    {
        internal PluginContext(PluginContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        private PluginContextProvider contextProvider;

       // public Form MainForm
       // {
           // get { return this.contextProvider.MainForm; }
       // }

      //  public DotNetBarManager DotNetBarManager
       // {
          //  get { return this.contextProvider.DotNetBarManager; }
       // }

      //  public Wz_Node SelectedNode1
       // {
         //   get { return this.contextProvider.SelectedNode1; }
       // }

       // public Wz_Node SelectedNode2
      //  {
        //    get { return this.contextProvider.SelectedNode2; }
      //  }

      //  public Wz_Node SelectedNode3
      //  {
         //   get { return this.contextProvider.SelectedNode3; }
       // }

      //  public SuperTabItem SelectedTab
      //  {
        //    get { return this.SuperTabControl1.SelectedTab; }
       // }

       // public event EventHandler<WzNodeEventArgs> SelectedNode1Changed
       // {
         //   add { contextProvider.SelectedNode1Changed += value; }
          //  remove { contextProvider.SelectedNode1Changed -= value; }
      //  }

      //  public event EventHandler<WzNodeEventArgs> SelectedNode2Changed
      //  {
       //     add { contextProvider.SelectedNode2Changed += value; }
         //   remove { contextProvider.SelectedNode2Changed -= value; }
       // }

      //  public event EventHandler<WzNodeEventArgs> SelectedNode3Changed
      //  {
         //   add { contextProvider.SelectedNode3Changed += value; }
          //  remove { contextProvider.SelectedNode3Changed -= value; }
      //  }

        public event EventHandler<WzStructureEventArgs> WzOpened
        {
            add { contextProvider.WzOpened += value; }
            remove { contextProvider.WzOpened-= value; }
        }

      //  public event EventHandler<WzStructureEventArgs> WzClosing
     //   {
         //   add { contextProvider.WzClosing += value; }
           // remove { contextProvider.WzClosing -= value; }
      //  }

      //  public StringLinker DefaultStringLinker
        //{
         //   get { return this.contextProvider.DefaultStringLinker; }
       // }

       // public AlphaForm DefaultTooltipWindow
       // {
         //   get { return this.contextProvider.DefaultTooltipWindow; }
       // }

      //  private SuperTabControl SuperTabControl1
       // {
          //  get
          //  {
               // var controls = this.contextProvider.MainForm.Controls.Find("superTabControl1", true);
             //   SuperTabControl tabControl = controls.Length > 0 ? (controls[0] as SuperTabControl) : null;
             //   return tabControl;
           // }
       // }

       

       

      

       
    }
}
