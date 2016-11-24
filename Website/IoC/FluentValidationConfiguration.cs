using FluentValidation.Mvc;

namespace Website.IoC
{
    public static class FluentValidationConfiguration
    {
        public static void Configure()
        {
            FluentValidationModelValidatorProvider.Configure();
        }
    }
}