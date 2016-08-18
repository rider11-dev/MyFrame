using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Core.Extensions.Validation
{
    /// <summary>
    /// 对比两属性值是否不相等
    /// </summary>
    public class NotEqualToAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 要比对的字段名称
        /// </summary>
        public string AnotherProperty { get; set; }

        public NotEqualToAttribute(string anotherProperty)
        {
            AnotherProperty = anotherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //从验证上下文中可以获取我们想要的的属性
            var property = validationContext.ObjectType.GetProperty(AnotherProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "{0}不存在", AnotherProperty));
            }
            //获取属性值
            var anotherValue = property.GetValue(validationContext.ObjectInstance, null);
            if (object.Equals(value, anotherValue))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "notequalto",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["another"] = AnotherProperty;

            yield return rule;
        }
    }
}