namespace DxUltimate.RichToolTip.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using System.Security;

    using DevExpress.LookAndFeel;
    using DevExpress.Skins;
    using DevExpress.Utils;
    using DevExpress.Utils.Drawing;
    using DevExpress.Utils.ViewInfo;

    /// <summary>
    /// Implements a ToolTipItem for displaying RichText.
    /// </summary>
    /// <seealso cref="DevExpress.Utils.BaseToolTipItem" />
    /// <seealso cref="System.Runtime.Serialization.ISerializable" />
    /// <seealso cref="System.IDisposable" />
    [TypeConverter(typeof(ToolTipTypeConverter))]
    [Serializable]
    public class RichToolTipItem : BaseToolTipItem, ISerializable, IDisposable
    {
        private bool isDisposed;
        private int leftIndent;
        private string text;
        private int maxHeight;

        /// <summary>Initializes a new instance of the <see cref="RichToolTipItem"/> class.</summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        public RichToolTipItem(SerializationInfo info, StreamingContext context)
            : this()
        {
            this.Deserialize(info, context);
        }

        /// <summary>Initializes a new instance of the <see cref="RichToolTipItem"/> class.</summary>
        public RichToolTipItem()
        {
            this.leftIndent = 0;
            this.maxHeight = 500;
            this.text = string.Empty;
        }

        /// <summary>Gets a value indicating whether the ToolTipItem is empty.</summary>
        /// <value>
        ///   <b>true</b>, if ToolTipItem content is empty; otherwise <b>false</b>.
        /// </value>
        public override bool IsEmpty
        {
            get
            {
                return base.IsEmpty && string.IsNullOrEmpty(this.Text);
            }
        }

        /// <summary>Gets or sets the indent from the left edge of a ToolTipItem.</summary>
        /// <value>The indent from the left edge of a ToolTipItem, in pixels.</value>
        [DefaultValue(0)]
        public int LeftIndent
        {
            get
            {
                return this.leftIndent;
            }

            set
            {
                this.leftIndent = Math.Max(value, 0);
            }
        }

        /// <summary>Gets or sets the maximum width.</summary>
        /// <value>The maximum width.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override int MaxWidth
        {
            get
            {
                return this.Container != null ? this.ScaleSize(this.Container.MaxWidth) - this.Container.Padding.Size.Width : base.MaxWidth;
            }

            set
            {
                base.MaxWidth = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the tooltip item text.</para>
        /// </summary>
        /// <value>Specifies the text in a ToolTipItem.</value>
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (this.Text == value)
                {
                    return;
                }

                this.text = value;
                this.AdjustSize();
            }
        }

        /// <summary>Gets or sets the maximum height.</summary>
        /// <value>The maximum height.</value>
        [DefaultValue(500)]
        public int MaxHeight
        {
            get
            {
                return this.maxHeight;
            }

            set
            {
                if (this.maxHeight == value)
                {
                    return;
                }

                this.maxHeight = value;
                this.AdjustSize();
            }
        }

        /// <summary>Gets the look and feel.</summary>
        /// <value>The look and feel.</value>
        internal new UserLookAndFeel LookAndFeel
        {
            get
            {
                return this.Container == null ? UserLookAndFeel.Default : this.Container.LookAndFeel;
            }
        }

        /// <summary>Gets the painter.</summary>
        /// <value>The painter.</value>
        protected internal new RichToolTipItemPainter Painter
        {
            get
            {
                return base.Painter as RichToolTipItemPainter;
            }
        }

        /// <summary>Gets the view information.</summary>
        /// <value>The view information.</value>
        protected internal new RichToolTipItemViewInfo ViewInfo
        {
            get
            {
                return base.ViewInfo as RichToolTipItemViewInfo;
            }
        }

        /// <summary>Gets the paint appearance.</summary>
        /// <returns>An <see cref="AppearanceObject"/>.</returns>
        public AppearanceObject GetPaintAppearance()
        {
            return this.ViewInfo.GetPaintAppearance();
        }

        /// <summary>Releases all unmanaged resources used by the ToolTipItem.</summary>
        public override void Dispose()
        {
            base.Dispose();
            this.Dispose(true);
        }

        /// <summary>Gets the object data.</summary>
        /// <param name="si">The si.</param>
        /// <param name="context">The context.</param>
        [SecurityCritical]
        void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
        {
            this.Serialize(si, context);
        }

        /// <summary>Assigns item properties.</summary>
        /// <param name="item">The item.</param>
        protected override void AssignCore(BaseToolTipItem item)
        {
            base.AssignCore(item);
            if (item is RichToolTipItem toolTipItem)
            {
                this.text = toolTipItem.Text;
                this.leftIndent = toolTipItem.LeftIndent;
                this.maxHeight = toolTipItem.MaxHeight;
            }

            this.MaxWidth = item.MaxWidth;
            this.Appearance.Assign(item.Appearance);
        }

        /// <summary>Creates a new instance.</summary>
        /// <returns>A new <see cref="BaseToolTipItem"/> descendant.</returns>
        protected override BaseToolTipItem CreateInstance()
        {
            return new RichToolTipItem();
        }

        /// <summary>Creates the painter.</summary>
        /// <returns>An ObjectPainter.</returns>
        protected override ObjectPainter CreatePainter()
        {
            return new RichToolTipItemPainter();
        }

        /// <summary>Creates the view information.</summary>
        /// <returns>A <see cref="BaseToolTipItemViewInfo"/> object.</returns>
        protected override BaseToolTipItemViewInfo CreateViewInfo()
        {
            return new RichToolTipItemViewInfo(this);
        }

        /// <summary>Deserializes the specified information.</summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected void Deserialize(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry serializationEntry in info)
            {
                switch (serializationEntry.Name)
                {
                    case "LeftIndent":
                        this.LeftIndent = info.GetInt32(serializationEntry.Name);
                        continue;
                    case "MaxHeight":
                        this.MaxHeight = info.GetInt32(serializationEntry.Name);
                        continue;
                    case "MaxWidth":
                        this.MaxWidth = info.GetInt32(serializationEntry.Name);
                        continue;
                    case "Text":
                        this.Text = info.GetString(serializationEntry.Name);
                        continue;
                    default:
                        continue;
                }
            }
        }

        /// <summary>Serializes the specified information.</summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        [SecurityCritical]
        protected virtual void Serialize(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MaxHeight", this.MaxHeight);
            info.AddValue("LeftIndent", this.LeftIndent);
            info.AddValue("MaxWidth", this.MaxWidth);
            info.AddValue("Text", this.Text);
        }

        private int ScaleSize(int size)
        {
            return (int)(size * (double)DpiProvider.Default.DpiScaleFactor);
        }

        private void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            this.isDisposed = true;
        }
    }
}