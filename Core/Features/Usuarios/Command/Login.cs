using System.ComponentModel.DataAnnotations;
using Core.Domain.Exceptions;
using Core.Domain.Services;
using Core.Infraestructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Usuarios.Command;

public record Login : IRequest<LoginResponse>
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Contrasena { get; set; }
}

public class LoginCommandHandler : IRequestHandler<Login, LoginResponse>
{
    private readonly IAuthService _authService;
    private readonly ChatContext _context;
    
    public LoginCommandHandler(IAuthService authService, ChatContext chat)
    {
        _authService = authService;
        _context = chat;
    }
    
    public async Task<LoginResponse> Handle(Login request, CancellationToken cancellationToken)
    {
        //Valida que no esten vacias
        if(string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Contrasena))
            throw new BadRequestException("Usuario y contraseña son obligatorios");
        
        //Si cumple con las validaciones se procede a autenticar
        var token = await _authService.AuthenticateAsync(request.UserName, request.Contrasena);
        
        if(token == null)
            throw new NotFoundException("Usuario no encontrado");

        var name = await _context.Usuarios.FirstOrDefaultAsync(x => x.UserName == request.UserName);
        
        //Respuesta
        return new LoginResponse()
        {
            Token = token,
            nombreUser = name.UserName
        };
    }
}

public record LoginResponse
{
    public string Token { get; set; }
    
    public string nombreUser { get; set; }
}