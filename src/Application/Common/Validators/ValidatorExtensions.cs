using System;
using Application.Common.Models;
using FluentValidation;

namespace Application.Common.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> ValidObjectId<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .Matches(@"^[a-f\d]{24}$")
                .WithMessage("This is not a valid object id.");

            return options;
        }

        public static IRuleBuilder<T, PageParameters> ValidPageParameters<T>(
            this IRuleBuilder<T, PageParameters> ruleBuilder,
             Type EntityType
        )
        {
            var options = ruleBuilder
                .NotEmpty()
                .Must((x => SortByMustBeInEntityProperties(x.SortBy, EntityType)))
                .WithMessage("These are not valid page parameters.");

            return options;
        }

        private static bool SortByMustBeInEntityProperties(string sortBy, Type EntityType)
        {
            var propertyInfos = EntityType.GetProperties();

            foreach (var propInfo in propertyInfos)
            {
                if (string.Equals(sortBy, propInfo.Name, StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}