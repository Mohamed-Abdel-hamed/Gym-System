using ClosedXML.Excel;

namespace Gym.Api.Extensions;

public static class ExcelSheetExtension
{
    public static void AddHeader(this IXLWorksheet sheet, string[]cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            sheet.Cell(1, i + 1).SetValue(cells[i]);
        }

        var header = sheet.Range(1, 1, 1, cells.Length);

        header.Style.Fill.BackgroundColor = XLColor.Black;

        header.Style.Font.FontColor = XLColor.White;

        header.Style.Font.SetBold();
    }
    public static void Format(this IXLWorksheet sheet)
    {

        sheet.ColumnsUsed().AdjustToContents();

        sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
    }
}
