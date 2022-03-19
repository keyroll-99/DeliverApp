using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Db;

public class Company : BaseModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string ContactNumber { get; set; }

    public virtual ICollection<User> Users { get; set; }
}
