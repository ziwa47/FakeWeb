using System.Collections.Generic;
using System.Linq;
using CoreDAL.Dto;
using CoreDAL.Entity;

namespace CoreDAL
{
    public class BoardDa : _BaseDA
    {
        private DbContext _context;

        public DbContext Context
        {
            get
            {
                if (_context == null) _context = new DbContext();
                return _context;
            }
        }

        public List<SettingDto> GetData(IEnumerable<string> ids)
        {
            var settings = _context
                           .Settings
                           .Where(r => ids.Contains(r.Id))
                           .ToList()
                           .Select(MappingSettingDto)
                           .ToList();

            return settings;
        }

        private static SettingDto MappingSettingDto(Setting r)
        {
            return new SettingDto
            {
                Id = r.Id,
                Name = r.Name
            };
        }
    }
}