using System.Collections.Generic;
using System.Linq;
using CoreDAL.Dto;
using CoreDAL.Entity;

namespace CoreDAL
{
    public class BoardDa : _BaseDA
    {
        public List<BoardDto> GetBoardData(IEnumerable<string> ids)
        {
            var settings = DBContext
                           .Boards
                           .Where(r => ids.Contains(r.Id))
                           .ToList()
                           .Select(MappingBoardDto)
                           .ToList();

            return settings;
        }

        private static BoardDto MappingBoardDto(Board r)
        {
            return new BoardDto
            {
                Id = r.Id,
                Name = r.Name
            };
        }
    }
}