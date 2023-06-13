namespace SLAMS_CRM.Blazor.Server.Services
{
    public class TemplateService
    {
        public string LoadTemplate(string templateFilePath, Dictionary<string, string> placeholders)
        {
            string templateContent = ReadTemplateFromFile(templateFilePath);

            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace(placeholder.Key, placeholder.Value);
            }

            return templateContent;
        }

        private string ReadTemplateFromFile(string templateFilePath)
        {
            // You can customize this method to read the template file from a different location or with specific options
            return File.ReadAllText(templateFilePath);
        }
    }
}
