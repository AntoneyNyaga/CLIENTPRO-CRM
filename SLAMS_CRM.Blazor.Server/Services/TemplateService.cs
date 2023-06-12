using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Blazor.Server.Services
{
    public class TemplateService
    {
        public string LoadTemplate(string templateFilePath, Dictionary<string, string> placeholders)
        {
            string templateContent = File.ReadAllText(templateFilePath);

            foreach (var placeholder in placeholders)
            {
                templateContent = templateContent.Replace(placeholder.Key, placeholder.Value);
            }

            return templateContent;
        }
    }
}