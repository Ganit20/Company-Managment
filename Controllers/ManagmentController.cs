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
    public class ManagmentController : Controller
    {
        public IConfiguration configuration;

        public ManagmentController(IConfiguration conf)
        {
            configuration = conf;

        }

        public async System.Threading.Tasks.Task<IActionResult> MCompanyAsync(int id)
        {
            
            Models.Companies s=null;
            using (var con = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand CompaniesQuery = new SqlCommand("SELECT companies.id,worker,former From Companies INNER JOIN Worker ON worker.CompanyID=Companies.id WHERE companies.id = " + id + "AND Worker ='"+User.Identity.Name+"';", con);
                var Read = CompaniesQuery.ExecuteReader();
                List<string> employ = new List<string>();
                while (Read.Read())
                {
                    if (Read.GetInt32(0) == null  )
                    {
                        return View("You do not have permission");
                    } else if(Read.GetInt32(2) == 0)
                    {
                        return RedirectToAction("Info", "InfoCompany",new { idi = id }, null);
                    }
                    
                }
                Read.Close();
                CompaniesQuery.Dispose();
                con.Close();
            }
            try
            {

                using (var con = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand CompaniesQuery = new SqlCommand("SELECT  Name,CreateDate,CreatorName,companies.id,worker,former From Companies INNER JOIN Worker ON worker.CompanyID=Companies.id WHERE companies.id = " + id + ";", con);
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
        [HttpPost]
        public string addemployee()
        {
            return "ok";
        }


           
    }
    }
