using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using ConferenceApp.Core.Models;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUserRepository _userRepository;
        private readonly MainDb _db;


        public RequestRepository
        (
            IReportRepository reportRepository,
            IUserRepository userRepository,
            MainDb db
        )
        {
            _reportRepository = reportRepository;
            _userRepository = userRepository;
            _db = db;
        }


        /// <summary>
        /// Добавить заявку.
        /// </summary>
        public void Insert( RequestModel model )
        {
            // 1. Добавить пользователя.
            var user = model.User;
            var userId = _userRepository.InsertWithId(user);

            // 2. Добавить заявку.
            var request = new Request {Id = Guid.NewGuid(), OwnerId = userId};
            _db.Insert(request);

            // 3. Добавить доклады.
            var reports = model.Reports;
            _reportRepository.InsertRange(reports);
        }


        /// <summary>
        /// Удалить заявку.
        /// </summary>
        public void Delete( Guid userId )
        {
            var request = _db.Requests.FirstOrDefault(x => x.OwnerId == userId);
            if( request == null )
            {
                return;
            }

            // 1. Удалить доклады.
            _reportRepository.DeleteRange(request.Id);

            // 2. Удалить заявку.
            _db.Requests.Delete(x => x.Id == userId);

            // 3. Удалить пользователя.
            _userRepository.Delete(request.OwnerId);
        }


        public void ChangeStatus( Guid requestId, RequestStatus status )
        {
            _db.Requests
                .Where(x => x.Id == requestId)
                .Set(x => x.Status, status)
                .Update();
        }


        /// <summary>
        /// Получить заявку по id владельца.
        /// </summary>
        public RequestModel Get( Guid requestId )
        {
            var request = _db.Requests.FirstOrDefault(x => x.Id == requestId);
            if( request == null )
            {
                return null;
            }

            var model = new RequestModel
            {
                User = _userRepository
                    .Get(x => x.Id == request.OwnerId)
                    .FirstOrDefault(),
                Reports = _reportRepository
                    .GetReportsByRequest(request.Id)
                    .ToList()
            };
            return model;
        }


        /// <summary>
        /// Получить все заявки.
        /// </summary>
        public IEnumerable<RequestModel> GetAll()
        {
            var requests = _db.Requests.ToList();
            var model = requests.Select(request => new RequestModel
                {
                    User = _userRepository
                        .Get(x => x.Id == request.OwnerId)
                        .FirstOrDefault(),
                    Reports = _reportRepository
                        .GetReportsByRequest(request.Id)
                        .ToList()
                })
                .ToList();
            return model;
        }
    }
}