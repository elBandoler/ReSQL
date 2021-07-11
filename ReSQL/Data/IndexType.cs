using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReSQL.Data
{
    public enum IndexType
    {
        NONE,
        PRIMARY,
        UNIQUE,
        INDEX,
        FULLTEXT,
        SPATIAL
    }
}
