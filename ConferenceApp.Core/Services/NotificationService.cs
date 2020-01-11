using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace ConferenceApp.Core.Services
{
    public class SmtpSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string BaseUrl { get; set; }
    }
    
    public class NotificationService
    {
        private readonly SmtpSettings _settings;
        private readonly string _conferenceName = "\"Современные тенденции развития и применения(ПИ-2020)\"";
        
        public NotificationService( SmtpSettings settings )
        {
            _settings = settings;
        }
        
        public async Task SendAccountConfirmationAsync( string email, string confirmUrl )
        {
            var content = new StringBuilder();
            
            content.AppendLine( $"Ваш адрес был указан при регистрации на конференции {_conferenceName} кафедры программной инженерии ЮЗГУ." );
            content.AppendLine( "\n" );
            content.AppendLine( "Для завершения регистрации перейдите по указанной ссылке:" );
            content.AppendLine( _settings.BaseUrl.TrimEnd( '/' ) + confirmUrl );
            await SendMessageAsync( email, $"Конференция {_conferenceName}", content.ToString() );
        }
        
        
        private async Task SendMessageAsync( string receiver, string subject, string content )
        {
            var message = new MimeMessage();
            message.From.Add( new MailboxAddress( $"Конференция {_conferenceName}", _settings.Address ) );
            message.To.Add( new MailboxAddress( receiver ) );
            message.Subject = subject;
            message.Body = new TextPart( "plain" ) { Text = content };

            using var client = new SmtpClient();
            await client.ConnectAsync( _settings.HostName, _settings.Port, _settings.EnableSsl );
            await client.AuthenticateAsync( _settings.Login, _settings.Password );

            await client.SendAsync( message );
            await client.DisconnectAsync( true );
        }
    }
}