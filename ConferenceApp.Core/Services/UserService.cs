using AutoMapper;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;

namespace ConferenceApp.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService( IUserRepository userRepository, IMapper mapper )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public bool TryToSignIn( string email, string password )
        {
            var user = _userRepository.GetByEmail(email);
            if( user == null 
                || user.UserStatus == UserStatus.Unconfirmed 
                || user.Password != password )
            {
                return false;
            }

            return true;
        }
    }
}