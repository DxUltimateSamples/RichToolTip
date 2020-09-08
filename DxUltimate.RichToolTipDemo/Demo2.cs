namespace DxUltimate.RichToolTipDemo
{
    using System.Collections.Generic;
    using System.Drawing;

    using DevExpress.Utils;

    using DxUltimate.RichToolTip.Helpers;
    using DxUltimate.RichToolTip.Items;

    public partial class Demo2 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private List<DemoItem> list;

        public Demo2()
        {
            this.InitializeComponent();

            this.list = new List<DemoItem>()
                            {
                                new DemoItem("CPU:", "Intel Core i7", ""),
                                new DemoItem("RAM:", "16 GB", "(optional 32 GB)"),
                                new DemoItem("HD:", "Samsung SSD 512 GB", ""),
                                new DemoItem("Graphics:", "Nvidia RTX4000", "(optional RTX4000)"),
                            };

            this.demoItemBindingSource1.DataSource = this.list;
        }

        private void defaultToolTipController1_DefaultController_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl == this.simpleButton1)
            {
                e.Info = this.CreateToolTipInfo();
                e.Info.ToolTipAnchor = ToolTipAnchor.Object;
                e.Info.ToolTipLocation = ToolTipLocation.RightTop;
            }
        }

        /// <summary>Creates the tool tip information.</summary>
        /// <returns>A <see cref="ToolTipControlInfo"/> object.</returns>
        public virtual ToolTipControlInfo CreateToolTipInfo()
        {
            var title = "DxUltimate - RichToolTip Demo";
            var tip = new SuperToolTip();
            tip.Items.AddTitle(title);
            tip.Items.AddSeparator();

            var descItem = tip.Items.Add("System specifications");
            descItem.ImageOptions.SvgImage = null;
            if (descItem.ImageOptions.SvgImage != null)
            {
                descItem.ImageToTextDistance = 8;
                descItem.Appearance.TextOptions.VAlignment = VertAlignment.Center;
                descItem.ImageOptions.SvgImageSize = new Size(32, 32);
            }

            var cells = new string[this.list.Count,3];

            for (int i = 0; i < this.list.Count; i++)
            {
                cells[i, 0] = this.list[i].Col1;
                cells[i, 1] = this.list[i].Col2;
                cells[i, 2] = this.list[i].Col3;
            }

            var richItem = new RichToolTipItem();
            richItem.MaxWidth = 600;
            richItem.Appearance.Assign(this.defaultToolTipController1.DefaultController.Appearance);

            richItem.LeftIndent = 8;
            var appearance = richItem.GetPaintAppearance();
            var rtf = RichTextTableBuilder.CreateTable(cells, appearance);

            richItem.Text = rtf;

            tip.Items.Add(richItem);
            tip.MaxWidth = 600;

            var info = new ToolTipControlInfo(this.simpleButton1, title);

            info.SuperTip = tip;
            info.ImmediateToolTip = true;

            return info;
        }

        private void gridControl1_Click(object sender, System.EventArgs e)
        {

        }

        public class DemoItem
        {
            /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
            public DemoItem(string col1, string col2, string col3)
            {
                this.Col1 = col1;
                this.Col2 = col2;
                this.Col3 = col3;
            }

            public string Col1 { get; set; }
            public string Col2 { get; set; }
            public string Col3 { get; set; }
        }
    }
}