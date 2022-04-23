using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Requests;
using Flutter.Backend.Service.Settings;
using static Flutter.Backend.Common.Constains.MessageResConstain;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Flutter.Backend.Service.Services
{
    public class SendMailService : GenericErrorTextService, ISendMailService
    {
        private readonly IMailSettings _mailSettings;

        public SendMailService(
            IMailSettings mailSettings,
            IMessageService messageService) : base(messageService)
        {
            _mailSettings = mailSettings;
        }

        public async Task<AppActionResultMessage<string>> SendMailRegisterAsync(MailRequest request)
        {
            var email = new MimeMessage();
            var result = new AppActionResultMessage<string>();

            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;

            var builder = new BodyBuilder();

            builder.HtmlBody = request.Body;

            email.Body = builder.ToMessageBody();
            using var smpt = new SmtpClient();
            smpt.Connect(_mailSettings.Host,_mailSettings.Port, SecureSocketOptions.StartTls);
            smpt.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            var sendMailResult = await smpt.SendAsync(email);
            if (sendMailResult == null)
            {
                return await BuildError(result, ERR_MSG_SEND_MAIL_FAILED);
            }
            smpt.Disconnect(true);

            return await BuildResult(result, MSG_SEND_MAIL_SUCCESSFULLY);
        }
    }
}
