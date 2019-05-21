using System.Collections.Generic;
using CoreDAL.Entity;

namespace CoreDAL
{
    public class DbContext
    {
        public List<Board> Boards { get; set; }
    }
}