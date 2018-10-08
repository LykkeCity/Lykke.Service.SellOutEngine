using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.SellOutEngine.Client.Models.Instruments;

namespace Lykke.Service.SellOutEngine.Validators
{
    [UsedImplicitly]
    public class InstrumentModelValidator : AbstractValidator<InstrumentModel>
    {
        public InstrumentModelValidator()
        {
            RuleFor(o => o.AssetPairId)
                .NotEmpty()
                .WithMessage("Asset pair id required");
            
            RuleFor(o => o.QuoteSource)
                .NotEmpty()
                .WithMessage("Quote source required");

            RuleFor(o => o.Markup)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Markup should be greater than or equal to zero")
                .LessThanOrEqualTo(1)
                .WithMessage("Markup should be less than or equal to one");
            
            RuleFor(o => o.Levels)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Levels should be greater than or equal to one");
            
            RuleFor(o => o.MinSpread)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Min spread should be greater than or equal to zero");
            
            RuleFor(o => o.MaxSpread)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max spread should be greater than or equal to zero")
                .Must((model, value) => model.MinSpread < value)
                .WithMessage("Max spread should be greater than min spread");
            
            RuleFor(o => o.Mode)
                .Must((model, value) => value != InstrumentMode.None)
                .WithMessage("Mode should be specified");
        }
    }
}
