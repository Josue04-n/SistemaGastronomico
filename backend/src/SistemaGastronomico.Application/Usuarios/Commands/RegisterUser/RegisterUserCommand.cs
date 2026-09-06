namespace SistemaGastronomico.Application.Usuarios.Commands.RegisterUser;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;
using SistemaGastronomico.Domain.Entities;
using SistemaGastronomico.Domain.Enums;
using ValidationException = SistemaGastronomico.Application.Common.Exceptions.ValidationException;

public record RegisterUserCommand(
    string NombreCompleto,
    string Email,
    string Password,
    string? Telefono,
    RolUsuario Rol = RolUsuario.DuenoLocal
) : IRequest<AuthResponseDto>;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(v => v.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es requerido.")
            .MaximumLength(150).WithMessage("El nombre completo no debe exceder los 150 caracteres.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido.")
            .EmailAddress().WithMessage("El correo electrónico no es válido.")
            .MaximumLength(150).WithMessage("El correo electrónico no debe exceder los 150 caracteres.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
    }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email == request.Email.ToLower().Trim(), cancellationToken);

        if (emailExiste)
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("Email", "El correo electrónico ya se encuentra registrado.")
            });
        }

        var usuario = new Usuario
        {
            NombreCompleto = request.NombreCompleto.Trim(),
            Email = request.Email.ToLower().Trim(),
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Telefono = request.Telefono?.Trim(),
            Rol = request.Rol,
            IsActive = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(usuario);

        return new AuthResponseDto(
            usuario.Id,
            usuario.NombreCompleto,
            usuario.Email,
            usuario.Rol.ToString(),
            token
        );
    }
}
