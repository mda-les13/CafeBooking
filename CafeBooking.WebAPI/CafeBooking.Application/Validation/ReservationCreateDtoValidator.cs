using FluentValidation;
using CafeBooking.BusinessLogic.DTOs;

namespace CafeBooking.BusinessLogic.Validation
{
    public class ReservationCreateDtoValidator : AbstractValidator<ReservationCreateDto>
    {
        public ReservationCreateDtoValidator()
        {
            RuleFor(r => r.CustomerName)
                .NotEmpty().WithMessage("Имя клиента обязательно")
                .MaximumLength(100).WithMessage("Имя не может превышать 100 символов");

            RuleFor(r => r.PhoneNumber)
                .NotEmpty().WithMessage("Номер телефона обязателен")
                .Matches(@"^\+?\d{1,3}[-.\s]?\(?\d{1,3}\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}$")
                .WithMessage("Некорректный формат номера телефона");

            RuleFor(r => r.StartDateTime)
                .GreaterThan(DateTime.Now).WithMessage("Дата начала должна быть в будущем");

            RuleFor(r => r.DurationHours)
                .InclusiveBetween(1, 24).WithMessage("Длительность бронирования должна быть от 1 до 24 часов");

            RuleFor(r => r.TableId)
                .GreaterThan(0).WithMessage("Некорректный идентификатор стола");
        }
    }
}
