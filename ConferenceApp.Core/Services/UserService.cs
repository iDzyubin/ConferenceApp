using AutoMapper;
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

        public (UserModel user, bool isSuccess) TryToSignIn( string email, string password )
        {
            var user = _userRepository.GetByEmail(email);
            if( user == null )
            {
                return (null, false);
            }

            if( user.Password != password )
            {
                return (null, false);
            }

            var model = _mapper.Map<UserModel>(user);
            return (model, true);
        }
    }
}