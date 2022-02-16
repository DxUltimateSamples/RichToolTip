namespace DxUltimate.RichToolTipDemo
{
    using DevExpress.Utils;

    using DxUltimate.RichToolTip.Items;

    public partial class Demo1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Demo1()
        {
            this.InitializeComponent();
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

            var richItem = new RichToolTipItem();

            richItem.Text = this.richEditControl1.RtfText;
            tip.Items.Add(richItem);

            var info = new ToolTipControlInfo(this.simpleButton1, title);

            info.SuperTip = tip;
            info.ImmediateToolTip = true;

            return info;
        }

        private void Demo1_Load(object sender, System.EventArgs e)
        {
            this.richEditControl1.RtfText = Properties.Resources.DemoText;
        }
    }
}