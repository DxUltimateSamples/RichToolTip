namespace DxUltimate.RichToolTip.Items
{
    using DevExpress.Utils.Drawing;
    using DevExpress.Utils.ViewInfo;

    public class RichToolTipItemInfoArgs : BaseToolTipItemInfoArgs
    {
        public RichToolTipItemInfoArgs(GraphicsCache cache, RichToolTipItemViewInfo viewInfo)
            : base(cache, viewInfo, viewInfo.Bounds)
        {
        }

        public new RichToolTipItemViewInfo ViewInfo
        {
            get
            {
                return base.ViewInfo as RichToolTipItemViewInfo;
            }
        }
    }
}