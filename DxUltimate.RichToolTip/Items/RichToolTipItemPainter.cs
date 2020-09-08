namespace DxUltimate.RichToolTip.Items
{
    using System.Drawing;

    using DevExpress.Utils.Drawing;
    using DevExpress.Utils.ViewInfo;
    using DevExpress.XtraEditors.Drawing;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraEditors.ViewInfo;
    using DevExpress.XtraRichEdit;

    /// <summary>
    /// Painter for <see cref="RichToolTipItem"/>.
    /// </summary>
    /// <seealso cref="DevExpress.Utils.ViewInfo.BaseToolTipItemPainter" />
    public class RichToolTipItemPainter : BaseToolTipItemPainter
    {
        private static RepositoryItemRichTextEdit richTextRepoItem;

        /// <summary>Initializes a new instance of the <see cref="RichToolTipItemPainter"/> class.</summary>
        public RichToolTipItemPainter()
        {
            if (richTextRepoItem == null)
            {
                richTextRepoItem = new RepositoryItemRichTextEdit { DocumentFormat = DocumentFormat.Rtf, HorizontalIndent = 0 };
            }
        }

        /// <summary>Draws the object.</summary>
        /// <param name="e">The e.</param>
        public override void DrawObject(ObjectInfoArgs e)
        {
            var infoArgs = e as RichToolTipItemInfoArgs;

            this.DrawText(infoArgs);
        }

        /// <summary>Draws the text.</summary>
        /// <param name="e">The e.</param>
        public virtual void DrawText(RichToolTipItemInfoArgs e)
        {
            var rtfBounds = e.ViewInfo.TextBounds;

            var vi = new RichTextEditViewInfo(richTextRepoItem);

            UpdateRichTextEditViewInfo(e.Cache.Graphics, e.ViewInfo.Text, vi, rtfBounds);

            RichTextEditPainter.DrawRTF(vi, e.Cache);
        }

        /// <summary>Calculates the size of the rich text.</summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="text">The text.</param>
        /// <param name="maxSize">The maximum size.</param>
        /// <param name="itemViewInfo">The item view information.</param>
        /// <returns>A <see cref="Size" />.</returns>
        public Size CalcRichTextSize(Graphics graphics, string text, Size maxSize, RichToolTipItemViewInfo itemViewInfo)
        {
            const int delta = 20;

            var vi = new RichTextEditViewInfo(richTextRepoItem);
            vi.LoadText(text);

            var viewer = vi.GetViewer();

            var height = viewer.GetEditorHeight(graphics, maxSize.Width, maxSize.Height, 1.0f);
            var width = maxSize.Width;

            while (height < maxSize.Height && width > delta && viewer.GetEditorHeight(graphics, width - delta, maxSize.Height, 1.0f) == height)
            {
                width -= delta;
            }

            return new Size(width + delta, height);
        }

        private static void UpdateRichTextEditViewInfo(Graphics graphics, string rtfText, RichTextEditViewInfo vi, Rectangle bounds)
        {
            vi.LoadText(rtfText);
            vi.Bounds = bounds;
            vi.CalcViewInfo(graphics);
        }
    }
}