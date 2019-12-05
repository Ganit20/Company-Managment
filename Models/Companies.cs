using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Managment.Models
{
    public class Companies
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<string> workers { get; set; }
        public string CreateDate { get; set; }
        public string CreatorName { get; set; }
        public int former { get; set; }
        public Companies(int idl,string name, List<string> employees, string createdate,string creatorname,int form)
        {

            this.Name = name;
            this.workers = employees;
            this.CreateDate = createdate;
            this.CreatorName = creatorname;
            id = idl;
            this.former = form;

        }
    }
    public class CompaniesList
    {
        public List<Companies> CompanyList { get; set; }
        public CompaniesList()
        {
            CompanyList = new List<Companies>();
        }
    }
}
