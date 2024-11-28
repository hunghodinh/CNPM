using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var smtpClient = new SmtpClient
        {
            Host = _configuration["EmailSettings:Host"],
            Port = int.Parse(_configuration["EmailSettings:Port"]),
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]
            ),
            EnableSsl = true, // Bật SSL để bảo mật
        };

        // Tạo đối tượng MailMessage
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["EmailSettings:From"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        // Thêm địa chỉ người nhận
        mailMessage.To.Add(to);

        // Thêm địa chỉ "Reply-To"
        mailMessage.ReplyToList.Add(new MailAddress("support@example.com"));

        // Gửi email
        await smtpClient.SendMailAsync(mailMessage);
    }
}



