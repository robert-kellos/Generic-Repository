using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using SampleArch.Model;

namespace SampleArch.Validation.Validators
{
    public class CountryValidator : AbstractValidator<Country>
    {
        private static CountryValidator _instance;
        public static CountryValidator Instance => _instance ?? (_instance = new CountryValidator());

        protected CountryValidator()
        {
            Task.Factory.StartNew(() => {
                RuleSet("Country", () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage($"Invalid [{"PropertyName"}]");
                    RuleFor(x => x.Name).NotEmpty().WithMessage($"[{"PropertyName"}] cannot be empty");
                    RuleFor(x => x.Name).NotNull().WithMessage($"[{"PropertyName"}] cannot be null");
                    RuleFor(x => x.Name)
                        .Length(2, 150)
                        .WithMessage($"[{"PropertyName"}] must be greater than {"MinLength"} and less than {"MaxLength"}");
                    //min 2, max 150

                    RuleFor(x => x.Persons).SetCollectionValidator(x => new PersonValidator());
                });
            }).ConfigureAwait(true);
        }

        public static string Test()
        {
            var result = string.Empty;

            var obj = new Country();
            //var validator = new CountryValidator();
            //var results = validator.Validate(obj);
            //var result = validator.Validate(obj, ruleSet: "Country");
            var results = Instance.Validate(obj);

            var validationSucceeded = results.IsValid;
            var failures = results.Errors;

            if (!validationSucceeded)
            {
                result = string.Join(", ", failures);
            }

            return result;
        }
    }
}
