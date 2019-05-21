using System.Collections.Generic;
using System.Linq;

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
            var settings = Context
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
                Name = r.Name,
            };
        }
    }

    public class _BaseDA
    {
    }

    public class SettingDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class DbContext
    {
        public List<Setting> Settings { get; set; }
    }

    public class Setting
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}