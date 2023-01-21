using FluentValidation;
using WeatherApi.Models;

namespace WeatherApi.Validators
{
    public class GeoPointValidator : AbstractValidator<GeoPoint>
    {
        public GeoPointValidator()
        {
            RuleFor(x => x.lat)
                .NotEmpty()
                .InclusiveBetween(-90.0, 90.0);

            RuleFor(x => x.lon)
                .NotEmpty()
                .InclusiveBetween(-180.0, 180.0);
        }
    }
}
