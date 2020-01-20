using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceApp.Core.DataModels;
using ConferenceApp.Core.Interfaces;
using LinqToDB;

namespace ConferenceApp.Core.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly MainDb _db;

        public SectionRepository( MainDb db ) => _db = db;

        public async Task<Guid> InsertAsync( Section item )
        {
            item.Id = Guid.NewGuid();
            await _db.InsertAsync(item);
            return item.Id;
        }

        public async Task UpdateAsync( Section item )
            => await _db.UpdateAsync(item);

        public async Task DeleteAsync( Guid id )
            => await _db.Sections.DeleteAsync(x => x.Id == id);

        public async Task<Section> GetAsync( Guid id )
            => await _db.Sections.FirstOrDefaultAsync(x => x.Id == id);

        public List<Section> Get( Func<Section, bool> filter ) 
            => _db.Sections.Where(filter).ToList();

        public async Task<List<Section>> GetAllAsync() 
            => await _db.Sections.ToListAsync();

        public async Task<ReportsInSection> GetByReport( Guid reportId )
            => await _db.ReportsInSections.FirstOrDefaultAsync(x => x.ReportId == reportId);

        public async Task<bool> IsExistAsync( Guid id ) 
            => await GetAsync(id) != null;
    }
}