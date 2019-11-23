using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.API.Interfaces;
using ConferenceApp.API.ViewModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;

namespace ConferenceApp.API.Repositories
{
    public class RequestRepositoryAdapter : IRequestRepositoryAdapter
    {
        private readonly IRequestRepository _requestRepository;


        public RequestRepositoryAdapter( IRequestRepository requestRepository )
        {
            _requestRepository = requestRepository;
        }

        
        public void Insert( RequestViewModel model )
        {
            var requestModel = new RequestModel
            {
                User = model.User,
                Reports = model.Reports
            };

            _requestRepository.Insert(requestModel);
        }


        public void Update( RequestViewModel model )
        {
            var requestModel = new RequestModel
            {
                User = model.User,
                Reports = model.Reports
            };

            _requestRepository.Update(requestModel);
        }


        public void Delete( Guid requestId )
        {
            _requestRepository.Delete(requestId);
        }


        public RequestViewModel Get( Guid requestId )
        {
            var request = _requestRepository.Get(requestId);
            if( request == null )
            {
                return null;
            }
            var model = new RequestViewModel
            {
                User = request.User,
                Reports = request.Reports
            };
            return model;
        }


        public IEnumerable<RequestViewModel> Get( Func<RequestViewModel, bool> filter )
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RequestViewModel> GetAll()
        {
            var requests = _requestRepository.GetAll();
            var model = requests.Select(request => new RequestViewModel
            {
                User = request.User, 
                Reports = request.Reports
            }).ToList();
            return model;
        }
    }
}