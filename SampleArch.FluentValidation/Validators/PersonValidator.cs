using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using SampleArch.Model;

namespace SampleArch.Validation.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        private static PersonValidator _instance;
        public static PersonValidator Instance => _instance ?? (_instance = new PersonValidator());

        public PersonValidator()
        {
            Task.Factory.StartNew(() =>
            {
                RuleSet("Person", () =>
                {
                    RuleFor(x => x.Id).NotEqual(0).WithMessage($"Invalid [{"PropertyName"}]");
                    RuleFor(x => x.Name).NotEmpty().WithMessage($"[{"PropertyName"}] cannot be empty");
                    RuleFor(x => x.Name).NotNull().WithMessage($"[{"PropertyName"}] cannot be null");
                    RuleFor(x => x.Name)
                        .Length(2, 150)
                        .WithMessage(
                            $"[{"PropertyName"}] must be greater than {"MinLength"} and less than {"MaxLength"}");
                    //min 2, max 150

                    //RuleFor(x => x.Country).SetValidator(x => new CountryValidator()).When(x => x != null);
                    RuleFor(x => x.Address).NotEmpty().WithMessage($"[{"PropertyName"}] cannot be empty");
                    RuleFor(x => x.Address).NotNull().WithMessage($"[{"PropertyName"}] cannot be null");
                    RuleFor(x => x.Address)
                        .Length(2, 250)
                        .WithMessage(
                            $"[{"PropertyName"}] must be greater than {"MinLength"} and less than {"MaxLength"}");
                    //min 2, max 250

                    RuleFor(x => x.Phone)
                        .Length(6, 12)
                        .WithMessage(
                            $"[{"PropertyName"}] must be greater than {"MinLength"} and less than {"MaxLength"}")
                        .When(x => x.Phone != null); //min 6, max 12
                    RuleFor(x => x.State)
                        .Length(2, 25)
                        .WithMessage(
                            $"[{"PropertyName"}] must be greater than {"MinLength"} and less than {"MaxLength"}")
                        .When(x => x.State != null); //min 2, max 25
                });


            }).ConfigureAwait(true);
        }

        //public static string Test<T>() where T: new()
        public static string Test()
        {
            var result = string.Empty;

            var obj = new Person();
            //var validator = new PersonValidator();
            //var results = validator.Validate(obj);
            //var result = validator.Validate(person, ruleSet: "Person");
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
