namespace SistemaGastronomico.Application.Usuarios.Commands.LoginUser;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaGastronomico.Application.Common.Interfaces;
using ValidationException = SistemaGastronomico.Application.Common.Exceptions.ValidationException;

public record LoginUserCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido.")
            .EmailAddress().WithMessage("El correo electrónico no es válido.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("La contraseña es requerida.");
    }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.ToLower().Trim();
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive, cancellationToken);

        if (usuario == null || !_passwordHasher.VerifyPassword(request.Password, usuario.PasswordHash))
        {
            throw new ValidationException(new[]
            {
                new FluentValidation.Results.ValidationFailure("Credenciales", "El correo electrónico o la contraseña ingresados son incorrectos.")
            });
        }

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
