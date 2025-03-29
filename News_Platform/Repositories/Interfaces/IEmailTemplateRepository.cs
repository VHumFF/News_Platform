using System.Collections.Generic;
using System.Threading.Tasks;
using News_Platform.Models;

namespace News_Platform.Repositories.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<Dictionary<string, EmailTemplate>> GetAllTemplatesAsync();
    }
}
