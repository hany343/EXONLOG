﻿using Microsoft.EntityFrameworkCore;
using EXONLOG.Model.Stocks;
using EXONLOG.Data;

namespace EXONLOG.Services
{
    public class MaterialTypeService
    {
        private readonly EXONContext _context;

        public MaterialTypeService(EXONContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddMaterialTypeAsync(MaterialType materialType)
        {
            _context.MaterialTypes.Add(materialType);
            await _context.SaveChangesAsync();
        }
        public async Task<List<MaterialType>> GetMaterialTypesAsync()
        {
            return await _context.MaterialTypes.ToListAsync();
        }
    }
}
