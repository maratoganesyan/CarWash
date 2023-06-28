using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;


namespace CarWash.Tools.ReportsGeneration
{
    internal static class ToolsForGeneration
    {
        public static void ReplaceWord(string Original, string NewText, Document WordDocument) => 
            ReplaceWord(Original, NewText, WordDocument, true);
        private static Word.Range ReplaceWord(string Original, string NewText, Document WordDocument, bool Returnable = false)
        {
            Word.Range Range = WordDocument.Content;
            Range.Find.ClearFormatting();
            Range.Find.Execute(FindText: Original, ReplaceWith: NewText);
            return Range;
        }
        public static void ReplaceWordWithUnderline(string Original, string NewText, Document WordDocument)
        {
            Word.Range Range = ReplaceWord(Original, NewText, WordDocument, true);
            Range.Font.Underline = WdUnderline.wdUnderlineSingle;
        }
        public static void ReplaceWordWithBold(string Original, string NewText, Document WordDocument)
        {
            Word.Range Range = ReplaceWord(Original, NewText, WordDocument, true);
            Range.Font.Bold = 1;
        }
        public static void AutoSizeColumn(Table MyTable, int ColumnNumber)
        {
            MyTable.AllowAutoFit = true;
            Column SomeColumn = MyTable.Columns[ColumnNumber];
            SomeColumn.AutoFit(); // force fit sizing
            float ColAutoWidth = SomeColumn.Width; // store autofit width
            MyTable.AutoFitBehavior(WdAutoFitBehavior.wdAutoFitWindow); // fill page width
            SomeColumn.SetWidth(ColAutoWidth, WdRulerStyle.wdAdjustFirstColumn); // reset width keeping right table margin
        }

    }
}
