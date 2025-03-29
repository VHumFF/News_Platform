using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News_Platform.Models;
using News_Platform.Data;
using News_Platform.Repositories.Interfaces;

namespace News_Platform.Repositories.Implementations
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly AppDbContext _context;
        private Dictionary<string, EmailTemplate> _emailTemplateCache;

        public EmailTemplateRepository(AppDbContext context)
        {
            _context = context;
            _emailTemplateCache = new Dictionary<string, EmailTemplate>();
        }

        public async Task<Dictionary<string, EmailTemplate>> GetAllTemplatesAsync()
        {
            if (_emailTemplateCache.Count == 0)
            {
                var templates = await _context.EmailTemplates.ToListAsync();
                _emailTemplateCache = templates.ToDictionary(t => t.TemplateName, t => t);
            }
            return _emailTemplateCache;
        }
    }
}
