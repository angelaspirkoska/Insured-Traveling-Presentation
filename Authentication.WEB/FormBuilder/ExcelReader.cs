using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using System.IO;
using System.Reflection;
using InsuredTraveling.FormBuilder;
using HtmlTags;

namespace InsuredTraveling.FormBuilder
{
    public static class ExcelReader
    {
        public static IHtmlString ReadExcel(string path)
        {
         
            ExcelPackage pck = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = pck.Workbook.Worksheets["Item_Details"];
            ExcelWorksheet worksheetListDetails = pck.Workbook.Worksheets["Lists"];
            var formBuilder = new FormBuilder()
               .SetName("my-form")
               .SetAction("/index")
               .SetMethod("post");

            List<TagInfo> tagInfoExcel = new List<TagInfo>();
            Dictionary<string, string> attributes;
            List<string> listValues;
            int itemIdIndex = -1;
            int nameCaptionIndex = -1;
            int fieldTypeIndex = -1;
            int listIdIndex = -1;
            int fieldListIndex = -1;
            int defaultValueIndex = -1;
            int requiredIndex = -1;
            int ratingIndicatorIndex = -1;
            int fieldSizeIndex = -1;
            int classesIndex = -1;
            int cssIndex = -1;

            for (int i = 1; worksheet.Cells[1, i].Value != null; i++)
            {
                var metaData = worksheet.Cells[1, i].Value.ToString();
                switch (metaData)
                {
                    case "Field_ID": itemIdIndex=i; break;
                    case "Name_Caption": nameCaptionIndex = i; break;
                    case "Field_type": fieldTypeIndex = i; break;
                    case "list_id": listIdIndex = i; break;
                    case "Field_list": fieldListIndex = i; break;
                    case "Default": defaultValueIndex = i; break;
                    case "Required": requiredIndex = i; break;
                    case "Rating_indicator": ratingIndicatorIndex = i; break;
                    case "Field_size": fieldSizeIndex = i; break;
                    case "Classes": classesIndex = i; break;
                    case "Css": cssIndex = i; break;
                    default: return null;
                }
            }

            for (int col = 2; worksheet.Cells[col, 1].Value != null ; col++)
            {
                attributes = new Dictionary<string, string>();
                listValues = new List<string>();

                if (worksheet.Cells[col, nameCaptionIndex].Value != null)
                {
                    attributes.Add("name", worksheet.Cells[col, nameCaptionIndex].Value.ToString());
                }

                if (worksheet.Cells[col, requiredIndex].Value.ToString() == "1")
                {
                    attributes.Add("required", "true");
                }

                if (worksheet.Cells[col, fieldSizeIndex].Value != null)
                {
                    attributes.Add("field_size", worksheet.Cells[col, fieldSizeIndex].Value.ToString());
                }

                if (worksheet.Cells[col, classesIndex].Value != null)
                {
                    attributes.Add("class", worksheet.Cells[col, classesIndex].Value.ToString());
                }
                if (worksheet.Cells[col, cssIndex].Value != null)
                {
                    attributes.Add("css", worksheet.Cells[col, cssIndex].Value.ToString());
                }

                if (worksheet.Cells[col, defaultValueIndex].Value.ToString() != "n/a")
                {
                    attributes.Add("default", worksheet.Cells[col, defaultValueIndex].Value.ToString());
                }

                var listId = worksheet.Cells[col, listIdIndex].Value;
                var type = worksheet.Cells[col, fieldTypeIndex].Value.ToString();

                if (listId!=null && !listId.ToString().Equals("0") && type.Equals("dropdown"))
                {
                    for(int column = 2; worksheetListDetails.Cells[column, 1].Value != null; column++)
                    {
                        var listIdDetails = worksheetListDetails.Cells[column, 1].Value;
                        if (listIdDetails != null && listIdDetails.Equals(listId))
                        {
                            var listValue = worksheetListDetails.Cells[column, 3].Value.ToString();
                            listValues.Add(listValue);
                        }
                    }
                }

                TagInfo tagInfo =
                    new TagInfo
                    {
                        Id = worksheet.Cells[col, itemIdIndex].Value.ToString(),
                        Name = worksheet.Cells[col, nameCaptionIndex].Value.ToString(),
                        Type = worksheet.Cells[col, fieldTypeIndex].Value.ToString(),
                        Attributes = attributes,
                        ListItems = listValues,
                    };

                tagInfoExcel.Add(tagInfo);
            }


            foreach (var excelRow in tagInfoExcel)
            {
                var wrapper = TagFactory.GenerateWrappedTagFor(excelRow);
                formBuilder.AddElement(wrapper);
            }


            var result = formBuilder.ToHtmlString();
            return new HtmlString(result);

        }
    }
}