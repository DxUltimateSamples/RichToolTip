﻿namespace DxUltimate.RichToolTip.Helpers
{
    using System.Drawing;
    using System.Linq;

    using DevExpress.Utils;
    using DevExpress.XtraRichEdit;
    using DevExpress.XtraRichEdit.API.Native;

    /// <summary>
    /// Builds a table as RTF string.
    /// </summary>
    public static class RichTextTableBuilder
    {
        /// <summary>Creates an RTF table from a 2-dimensional string array.</summary>
        /// <param name="cellText">The cell text.</param>
        /// <param name="appearance">The appearance.</param>
        /// <returns>A table as RTF string.</returns>
        public static string CreateTable(string[,] cellText, AppearanceObject appearance)
        {
            var rowCount = cellText.GetLength(0);
            var colCount = cellText.GetLength(1);

            using (var srv = new RichEditDocumentServer())
            {
                Document doc = srv.Document;
                doc.DefaultTableProperties.TableBorders.Top.LineStyle = TableBorderLineStyle.None;
                doc.DefaultTableProperties.TableBorders.Bottom.LineStyle = TableBorderLineStyle.None;
                doc.DefaultTableProperties.TableBorders.Left.LineStyle = TableBorderLineStyle.None;
                doc.DefaultTableProperties.TableBorders.Right.LineStyle = TableBorderLineStyle.None;

                doc.DefaultCharacterProperties.ForeColor = appearance?.GetForeColor();
                
                var font = appearance?.GetFont();

                doc.DefaultCharacterProperties.FontName = font?.Name;
                doc.DefaultCharacterProperties.FontSize = (float?)(font?.Size * 0.94);
                doc.DefaultCharacterProperties.Bold  = font?.Bold;
                doc.DefaultCharacterProperties.Italic  = font?.Italic;
                doc.DefaultCharacterProperties.Underline = (font?.Underline ?? false) ? UnderlineType.Single : UnderlineType.None;
                doc.DefaultCharacterProperties.Strikeout = (font?.Strikeout ?? false) ? StrikeoutType.Single : StrikeoutType.None;

                var tableStyles = doc.TableStyles.Where(e => e.Name != "Normal Table").ToList();
                tableStyles.ForEach(doc.TableStyles.Delete);

                Table tbl = doc.Tables.Create(doc.Range.Start, rowCount, colCount, AutoFitBehaviorType.AutoFitToContents);
                tbl.TableBackgroundColor = Color.Empty;
                tbl.Indent = 0;
                tbl.TableLayout = TableLayoutType.Autofit;

                try
                {
                    tbl.BeginUpdate();

                    for (int i = 0; i < rowCount; i++)
                    {
                        TableRow row = tbl.Rows[i];
                        for (int j = 0; j < colCount; j++)
                        {
                            var cell = row.Cells[j];
                            cell.TopPadding = 2;
                            cell.LeftPadding = 5;
                            cell.BottomPadding = 12;
                            doc.InsertSingleLineText(cell.Range.Start, cellText[i, j]);
                            cell.PreferredWidthType = WidthType.Auto;
                        }
                    }

                    ////// Center the table header.
                    ////foreach (Paragraph p in document.Paragraphs.Get(tbl.FirstRow.Range))
                    ////{
                    ////    p.Alignment = ParagraphAlignment.Center;
                    ////}
                }
                finally
                {
                    tbl.EndUpdate();
                }

                ////var charSettings = doc.BeginUpdateCharacters(doc.Range);
                ////charSettings.Reset(CharacterPropertiesMask.ForeColor | CharacterPropertiesMask.FontName | CharacterPropertiesMask.FontSize);
                ////doc.EndUpdateCharacters(charSettings);
                tbl.BeginUpdate();
                tbl.SetPreferredWidth(0, WidthType.Auto);
                tbl.EndUpdate();

                ////doc.AppendText("This document is generated by Word Processing Document API");
                ////CharacterProperties cp = doc.BeginUpdateCharacters(doc.Paragraphs[0].Range);
                ////cp.ForeColor = System.Drawing.Color.FromArgb(0x83, 0x92, 0x96);
                ////cp.Italic = true;
                ////doc.EndUpdateCharacters(cp);
                ////ParagraphProperties pp = doc.BeginUpdateParagraphs(doc.Paragraphs[0].Range);
                ////pp.Alignment = DevExpress.XtraRichEdit.API.Native.ParagraphAlignment.Right;
                ////doc.EndUpdateParagraphs(pp);
                return doc.RtfText;
            }
        }
    }
}