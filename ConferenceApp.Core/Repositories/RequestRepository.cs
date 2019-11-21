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
            var userId = _userRepository.InsertWithId( user );

            // 2. Добавить заявку.
            var request = new Request { Id = Guid.NewGuid(), OwnerId = userId };
            _db.Insert( request );

            // 3. Добавить доклады.
            var reports = model.Reports;
            _reportRepository.InsertRange( reports );
        }


        /// <summary>
        /// Удалить заявку.
        /// </summary>
        public void Delete(Guid userId)
        {
            var request = _db.Requests.FirstOrDefault(x => x.OwnerId == userId);

            // 1. Удалить доклады.
            _reportRepository.DeleteRange( request.Id );

            // 2. Удалить заявку.
            _db.Requests.Delete( x => x.Id == userId );

            // 3. Удалить пользователя.
            _userRepository.Delete( request.OwnerId );
        }


        /// <summary>
        /// Обновить информацию по заявке.
        /// </summary>
        public void Update(RequestModel request)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Получить заявку по id владельца.
        /// </summary>
        public RequestModel Get(Guid userId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Получить заявки по критерию.
        /// </summary>
        public IEnumerable<RequestModel> Get(Func<RequestModel, bool> filter)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Получить все заявки.
        /// </summary>
        public IEnumerable<RequestModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}