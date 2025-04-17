using System;
using System.ComponentModel.DataAnnotations;

namespace News_Platform.Models
{
    public class EmailTemplate
    {
        [Key]
        public int TemplateID { get; set; }

        public string TemplateName { get; set; }
        public string Subject { get; set; }
        public string Template { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
