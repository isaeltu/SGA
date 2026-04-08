using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SGA.Persistence.Context;
using System.Net;
using System.Net.Mail;

namespace SGA.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private const string MasterAdminEmail = "adm@isael.com";
    private const string MasterOtpRecipient = "isaelcapellanlite@gmail.com";
    private const string MasterOtpCacheKey = "MASTER_ADMIN_OTP";

    private readonly ApplicationDbContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        ApplicationDbContext dbContext,
        IMemoryCache memoryCache,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("portal-login")]
    public async Task<ActionResult<PortalLoginResponse>> PortalLogin([FromBody] PortalLoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest("Email is required.");
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var person = await _dbContext.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => EF.Property<string>(p, nameof(SGA.Domain.Entities.Users.Person.Email)) == normalizedEmail,
                cancellationToken)
            .ConfigureAwait(false);

        if (person is null)
        {
            return NotFound("No existe un usuario con ese correo.");
        }

        var personId = person.Id;

        var isAdmin = await _dbContext.Administrators.AsNoTracking().AnyAsync(x => x.PersonId == personId, cancellationToken).ConfigureAwait(false);
        var isOperator = await _dbContext.Operators.AsNoTracking().AnyAsync(x => x.PersonId == personId, cancellationToken).ConfigureAwait(false);
        var isDriver = await _dbContext.Drivers.AsNoTracking().AnyAsync(x => x.PersonId == personId, cancellationToken).ConfigureAwait(false);
        var isStudent = await _dbContext.Students.AsNoTracking().AnyAsync(x => x.PersonId == personId, cancellationToken).ConfigureAwait(false);

        var response = new PortalLoginResponse(
            person.Id,
            person.InstitutionId,
            person.FirstName ?? string.Empty,
            person.LastName ?? string.Empty,
            normalizedEmail,
            isAdmin,
            isOperator,
            isDriver,
            isStudent || !isAdmin,
            false);

        return Ok(response);
    }

    [HttpPost("master/request-otp")]
    public async Task<IActionResult> RequestMasterOtp([FromBody] MasterOtpRequest request, CancellationToken cancellationToken)
    {
        if (!string.Equals(request.Email?.Trim(), MasterAdminEmail, StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized("Este acceso solo aplica para el administrador master.");
        }

        var otp = Random.Shared.Next(100000, 999999).ToString();
        _memoryCache.Set(MasterOtpCacheKey, otp, TimeSpan.FromMinutes(5));

        await SendOtpByEmailAsync(otp, cancellationToken).ConfigureAwait(false);
        return Ok(new { message = "Codigo enviado al correo de seguridad del master admin." });
    }

    [HttpPost("master/verify-otp")]
    public IActionResult VerifyMasterOtp([FromBody] MasterOtpVerifyRequest request)
    {
        if (!string.Equals(request.Email?.Trim(), MasterAdminEmail, StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized("Este acceso solo aplica para el administrador master.");
        }

        if (!_memoryCache.TryGetValue(MasterOtpCacheKey, out string? expectedOtp) || string.IsNullOrWhiteSpace(expectedOtp))
        {
            return BadRequest("El codigo expiro o no fue generado. Solicita uno nuevo.");
        }

        if (!string.Equals(expectedOtp, request.Code?.Trim(), StringComparison.Ordinal))
        {
            return Unauthorized("Codigo invalido.");
        }

        _memoryCache.Remove(MasterOtpCacheKey);

        var profile = new PortalLoginResponse(
            0,
            0,
            "Master",
            "Admin",
            MasterAdminEmail,
            true,
            false,
            false,
            true,
            true);

        return Ok(profile);
    }

    private async Task SendOtpByEmailAsync(string otp, CancellationToken cancellationToken)
    {
        var host = _configuration["Smtp:Host"];
        var portRaw = _configuration["Smtp:Port"];
        var user = _configuration["Smtp:User"];
        var pass = _configuration["Smtp:Password"];
        var from = _configuration["Smtp:From"];

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(portRaw) ||
            !int.TryParse(portRaw, out var port) ||
            string.IsNullOrWhiteSpace(user) ||
            string.IsNullOrWhiteSpace(pass) ||
            string.IsNullOrWhiteSpace(from))
        {
            _logger.LogWarning("SMTP is not fully configured. OTP for master admin cannot be delivered by email.");
            throw new InvalidOperationException("SMTP no esta configurado en la API. Configura Smtp en appsettings para enviar el codigo.");
        }

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(user, pass)
        };

        using var message = new MailMessage(from, MasterOtpRecipient)
        {
            Subject = "Codigo OTP - Master Admin SGA",
            Body = $"Tu codigo de verificacion de 6 digitos es: {otp}. Expira en 5 minutos."
        };

        cancellationToken.ThrowIfCancellationRequested();
        await client.SendMailAsync(message).ConfigureAwait(false);
    }

    public sealed record PortalLoginRequest(string Email);
    public sealed record MasterOtpRequest(string Email);
    public sealed record MasterOtpVerifyRequest(string Email, string Code);

    public sealed record PortalLoginResponse(
        int PersonId,
        int InstitutionId,
        string FirstName,
        string LastName,
        string Email,
        bool IsAdmin,
        bool IsOperator,
        bool IsDriver,
        bool IsClientOrStudent,
        bool IsMasterAdmin);
}