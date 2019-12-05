using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Company_Managment.Controllers
{
    [Authorize]
    public class InfoCompanyController : Controller
    {
        public IConfiguration configuration;

        public InfoCompanyController(IConfiguration conf)
        {
            configuration = conf;

        }

        public async System.Threading.Tasks.Task<IActionResult> Info(int idi)
        {
            Models.Companies s = null;
            try
            {

                using (var con = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand CompaniesQuery = new SqlCommand("SELECT  Name,CreateDate,CreatorName,companies.id,worker,former From Companies INNER JOIN Worker ON worker.CompanyID=Companies.id WHERE companies.id = " + idi + ";", con);
                    var Read = CompaniesQuery.ExecuteReader();
                    List<string> employ = new List<string>();
                    while (Read.Read())
                    {
                        s = new Models.Companies(Read.GetInt32(3), Read[0].ToString(), employ, Read[1].ToString(), Read[2].ToString(), Read.GetInt32(5));
                        if (Read[4] != null)
                            s.workers.Add(Read[4].ToString());
                    }
                    Read.Close();
                    CompaniesQuery.Dispose();
                    con.Close();
                }
               
            }
            catch (IOException) { }
            return View(s);
        }
    }
}
