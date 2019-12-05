using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Company_Managment.Controllers
{
    [Authorize]
    public class CheckInviteController : Controller
    {
        public IConfiguration configuration;
        public CheckInviteController(IConfiguration conf)
        {
            configuration = conf;
        }
        [HttpPost]
        public string Check(int Cid,string email,List<string> work)
        {
            List<string> Workers = work;
            bool check = false;
           

            string id="0";
            string Email =email;
            int CompanyID = Cid;
            
                using (var con2 = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    con2.Open();
                    SqlCommand CompaniesQuery = new SqlCommand("SELECT id,Email FROM AspNetUsers Where Email='" + Email + "';", con2);
                    var Read = CompaniesQuery.ExecuteReader();
                    while (Read.Read())
                    {
                        if (Read[0].ToString() == null)
                        {
                            return "wrong Email";
                        }
                        else
                        {
                            id = Read[0].ToString();
                            Email = Read[1].ToString();
                        }

                    }
                    CompaniesQuery.Dispose();
                    Read.Close();
                    con2.Close();
                }
                using (var con = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand CompaniesQuery = new SqlCommand("SELECT  AccountID from worker Where CompanyID='" + CompanyID + "';", con);
                    var Read = CompaniesQuery.ExecuteReader();
                    List<string> employ = new List<string>();
                    while (Read.Read())
                    {

                        if (Read[0] != null)
                            Workers.Add(Read[0].ToString());
                    }
                    foreach(var item in Workers)
                {
                    if (item == id) { check = true; }
                }
                    Read.Close();
                    CompaniesQuery.Dispose();
                    con.Close();
                }
            if (check == false)
            {
                using (var con2 = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    con2.Open();
                    SqlCommand CompaniesQuery = new SqlCommand("Insert into worker(worker,companyID,accountID) values ('"+  Email+ "'," +CompanyID+",'" + id +"');", con2);
                    var Write = CompaniesQuery.ExecuteNonQuery();
                    CompaniesQuery.Dispose();
                    con2.Close();
                    return "Invite send to " + Email;
                }
            } else
            {
                return "Can't add him second time";
            }
        }
        
        }
    }
