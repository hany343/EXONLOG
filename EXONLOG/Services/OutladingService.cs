using EXONLOG.Data;
using EXONLOG.Model.Outbound;
using System;

namespace EXONLOG.Services
{
    public class OutloadingService
    {
        private readonly EXONContext _context;

        public OutloadingService(EXONContext context)
        {
            _context = context;
        }

        public async Task SaveOutloadingAsync(OutLading outloading)
        {
            _context.OutLadings.Add(outloading);
            await _context.SaveChangesAsync();
        }
    }

}
