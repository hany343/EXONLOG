using EXONLOG.Data;
using EXONLOG.Model.Outbound;
using System;

namespace EXONLOG.Services
{
    public class OutladingService
    {
        private readonly EXONContext _context;

        public OutladingService(EXONContext context)
        {
            _context = context;
        }

        public async Task SaveOutladingAsync(OutLading outloading)
        {
            _context.OutLadings.Add(outloading);
            await _context.SaveChangesAsync();
        }
    }

}
