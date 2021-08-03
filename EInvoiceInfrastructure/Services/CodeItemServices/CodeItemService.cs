using AutoMapper;
using EInvoiceCore.Entities;
using EInvoiceInfrastructure.EFRepository;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceInfrastructure.Services.CodeItemServices
{
    public class CodeItemService : ICodeItemService
    {
        #region Fields
        private readonly IRepository<CodeItem> _codeItemRepository;

        #endregion
        #region Constructor
        public CodeItemService( IRepository<CodeItem> codeItemRepository)
        {
            _codeItemRepository = codeItemRepository;
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<CodeItem>> AddDataFromExcelFile(IFormFile excelFile)
        {
            try
            {
                var codeItemList = new List<CodeItem>();
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var countRow = worksheet.Dimension.Rows;
                        for (int row = 2; row <= countRow; row++)
                        {
                            codeItemList.Add(new CodeItem
                            {
                                InternalCode = Convert.ToInt32(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                EgCode = Convert.ToInt32(worksheet.Cells[row, 2].Value.ToString().Trim())

                            });
                        }
                    }
                }
                await _codeItemRepository.Create(codeItemList);
                await _codeItemRepository.Save();
                return codeItemList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<IEnumerable<CodeItem>> GetAll()
        {
            try
            {
                var codeItems = await _codeItemRepository.GetAll();
                return codeItems;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion
    }
}
