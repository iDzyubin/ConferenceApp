using System;
using System.Collections.Generic;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.Models;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;

namespace ConferenceApp.API.Repositories
{
    public class RequestRepositoryAdapter : IRequestRepositoryAdapter
    {
        private readonly IReportRepositoryAdapter _reportRepositoryAdapter;
        private readonly IRequestRepository _requestRepository;
        private readonly IUserRepository _userRepository;


        public RequestRepositoryAdapter
        (
            IReportRepositoryAdapter reportRepositoryAdapter,
            IRequestRepository requestRepository,
            IUserRepository userRepository
        )
        {
            _reportRepositoryAdapter = reportRepositoryAdapter;
            _requestRepository = requestRepository;
            _userRepository = userRepository;
        }

        
        public void Insert( RequestModel model )
        {
            // 1. Добавить пользователя.
            var userId = _userRepository.InsertWithId( model.User );

            // 2. Добавить заявку.
            _requestRepository.Insert( new Request { OwnerId = userId } );

            // 3. Добавить доклады.
            _reportRepositoryAdapter.InsertRange( model.Reports );
        }

        
        public void Delete( Guid requestId )
        {
            var request = _requestRepository.Get( requestId );

            // 1. Удалить доклады.
            _reportRepositoryAdapter.DeleteRange( requestId );

            // 2. Удалить заявку.
            _requestRepository.Delete( requestId );

            // 3. Удалить пользователя.
            _userRepository.Delete( request.OwnerId );
        }

        
        public void Update( RequestModel item )
        {
            throw new NotImplementedException();
        }

        
        public RequestModel Get( Guid id )
        {
            throw new NotImplementedException();
        }


        public IEnumerable<RequestModel> Get( Func<RequestModel, bool> filter )
        {
            throw new NotImplementedException();
        }


        public IEnumerable<RequestModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}