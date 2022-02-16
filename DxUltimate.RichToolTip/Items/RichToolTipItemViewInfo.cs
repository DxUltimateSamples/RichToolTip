namespace DxUltimate.RichToolTip.Items
{
    using System;
    using System.Drawing;

    using DevExpress.LookAndFeel;
    using DevExpress.Skins;
    using DevExpress.Utils;
    using DevExpress.Utils.Drawing;
    using DevExpress.Utils.Text;
    using DevExpress.Utils.ViewInfo;

    public class RichToolTipItemViewInfo : BaseToolTipItemViewInfo
    {
        private Rectangle contentBounds;
        private int lastAvailableWidth;
        private Size? lastSize;

        private string lastText;
        private Rectangle textBounds;
        private StringInfo textInfo;

        public RichToolTipItemViewInfo(RichToolTipItem item)
            : base(item)
        {
            this.contentBounds = this.textBounds = Rectangle.Empty;
        }

        public Rectangle ContentBounds
        {
            get
            {
                return this.contentBounds;
            }
        }

        public override AppearanceDefault DefaultAppearance
        {
            get
            {
                return this.LookAndFeel.ActiveStyle == ActiveLookAndFeelStyle.Skin && this.GetElement() != null
                           ? this.GetElement().GetAppearanceDefault(this.LookAndFeel)
                           : base.DefaultAppearance;
            }
        }

        public new RichToolTipItem Item
        {
            get
            {
                return base.Item as RichToolTipItem;
            }
        }

        public override UserLookAndFeel LookAndFeel
        {
            get
            {
                return this.Item.LookAndFeel;
            }
        }

        public string Text
        {
            get
            {
                return this.Item.Text;
            }
        }

        public Rectangle TextBounds
        {
            get
            {
                return this.textBounds;
            }

            set
            {
                this.textBounds = value;
            }
        }

        public StringInfo TextInfo
        {
            get
            {
                if (this.textInfo == null)
                {
                    this.textInfo = new StringInfo();
                }

                return this.textInfo;
            }

            set
            {
                this.textInfo = value;
            }
        }

        protected virtual int LeftContentIndent
        {
            get
            {
                return this.Item.LeftIndent;
            }
        }

        public AppearanceObject GetPaintAppearance()
        {
            this.UpdatePaintAppearance();
            return this.PaintAppearance;
        }

        protected internal virtual int CalcAvailableContentWidth()
        {
            return this.Item.MaxWidth;
        }

        protected internal virtual int CalcAvailableTextWidth()
        {
            int val1 = this.CalcAvailableContentWidth() - this.LeftContentIndent;
            return Math.Max(val1, 1);
        }

        protected internal virtual void CalcContentBounds()
        {
            this.contentBounds = this.Bounds;
        }

        protected internal virtual Size CalcTextSize()
        {
            var availableWidth = this.CalcAvailableTextWidth();

            if (this.lastSize.HasValue && this.lastAvailableWidth == availableWidth && this.lastText == this.Text)
            {
                return this.lastSize.Value;
            }

            var result = this.Item.Painter.CalcRichTextSize(this.GInfo.Graphics, this.Text, new Size(availableWidth, this.Item.MaxHeight), this);
            this.lastSize = result;
            this.lastAvailableWidth = availableWidth;
            this.lastText = this.Text;

            return result;
        }

        protected internal virtual void LayoutText()
        {
            this.TextBounds = this.GetAvailableTextBounds();

            ////this.TextInfo = this.CalcHtmlText(this.TextBounds);
        }

        protected override Size CalcActualContentSizeCore()
        {
            this.textBounds.Size = this.CalcTextSize();
            Size empty = Size.Empty;
            empty.Height = this.textBounds.Height;
            empty.Width = this.textBounds.Width + this.LeftContentIndent;
            return empty;
        }

        protected override void CalcViewInfoCore()
        {
            this.CalcContentBounds();
            this.CalcActualContentSize();
            this.LayoutItem();
        }

        protected SkinElement GetElement()
        {
            return this.GetElement(this.GetElementName()) == null ? this.GetElement(CommonSkins.SkinToolTipItem) : this.GetElement(this.GetElementName());
        }

        protected virtual string GetElementName()
        {
            return CommonSkins.SkinToolTipItem;
        }

        protected override BaseToolTipItemInfoArgs GetInfoArgs(GraphicsCache cache)
        {
            return new RichToolTipItemInfoArgs(cache, this);
        }

        protected override void LayoutItem()
        {
            this.LayoutText();
        }

        private Rectangle GetAvailableTextBounds()
        {
            int x = this.ContentBounds.X + this.LeftContentIndent;
            int y = this.ContentBounds.Y;
            int width = this.ContentBounds.Width - this.LeftContentIndent;
            int height = this.ContentBounds.Height;

            return new Rectangle(x, y, width, height);
        }

        private SkinElement GetElement(string name)
        {
            return CommonSkins.GetSkin(this.LookAndFeel)[name];
        }
    }
}