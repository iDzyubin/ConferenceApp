using System;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService( IUserRepository userRepository )
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Попытка войти в систему.
        /// </summary>
        public async Task<(User user, bool result)> TryToSignInAsync( string email, string password )
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if( user == null 
                || user.UserStatus == UserStatus.Unconfirmed 
                || user.Password != password )
            {
                return (null, false);
            }

            return (user, true);
        }

        /// <summary>
        /// Подтверждение аккаунта.
        /// </summary>
        public async Task ConfirmAccountAsync( string code )
        {
            var user = _userRepository
                           .Get( u => u.ConfirmCode == code ).FirstOrDefault() 
                            ?? throw new Exception( "Ссылка недействительна" );
            switch( user.UserStatus )
            {
                case UserStatus.Unconfirmed:
                    user.UserStatus = UserStatus.Confirmed;
                    await _userRepository.Confirm( user.Id );
                    break;
                case UserStatus.Confirmed:
                    throw new Exception( "Аккаунт был подтвержден ранее" );
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}