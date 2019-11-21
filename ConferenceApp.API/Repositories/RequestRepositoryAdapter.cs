using System;
using System.Collections.Generic;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.Models;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;

namespace ConferenceApp.API.Repositories
{
    public class RequestRepositoryAdapter : IRequestRepositoryAdapter
    {
        private readonly IRequestRepository _requestRepository;


        public RequestRepositoryAdapter(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        
        public void Insert( RequestViewModel model )
        {
            var requestModel = new RequestModel
            {
                User    = model.User,
                Reports = model.Reports
            };
            
            _requestRepository.Insert( requestModel );
        }
        
        
        public void Update( RequestViewModel model )
        {
            var requestModel = new RequestModel
            {
                User    = model.User,
                Reports = model.Reports
            };
            
            _requestRepository.Update(requestModel);
        }

        
        public void Delete( Guid requestId )
        {
            _requestRepository.Delete(requestId);
        }

        
        public RequestViewModel Get( Guid id )
        {
            throw new NotImplementedException();
        }


        public IEnumerable<RequestViewModel> Get( Func<RequestViewModel, bool> filter )
        {
            throw new NotImplementedException();
        }


        public IEnumerable<RequestViewModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}