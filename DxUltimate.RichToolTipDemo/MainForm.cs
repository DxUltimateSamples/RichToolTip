namespace DxUltimate.RichToolTipDemo
{
    using DevExpress.Data.Utils;

    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public MainForm()
        {
            this.InitializeComponent();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            var frm = new Demo1();
            frm.MdiParent = this;
            frm.Show();

            var frm2 = new Demo2();
            frm2.MdiParent = this;
            frm2.Show();

            this.xtraTabbedMdiManager1.SelectedPage = this.xtraTabbedMdiManager1.Pages[frm];
        }

        private void barButtonLaunchWebsite_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SafeProcess.Open("https://github.com/DxUltimateSamples");
        }
    }
}
