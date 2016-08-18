using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MyFrame.Infrastructure.Extension;

namespace WebApp.Core.Extensions.Validation
{
    public class ValidDateTimeAttribute : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dt = value.ConvertTo<DateTime>();
                if (dt <= new DateTime(1753, 1, 1, 12, 0, 0))
                {
                    //return new ValidationResult("是非法日期", new string[] { validationContext.DisplayName });
                    return new ValidationResult(string.Empty);
                }
            }
            return null;//null表示验证成功
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "validdatetime",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };

            yield return rule;
        }
    }
}
