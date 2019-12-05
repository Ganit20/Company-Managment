using Company_Managment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Company_Managment.Controllers
{
    [ViewComponent(Name = "Companies")]
    [Authorize]
    public class Companies : ViewComponent
    {
        public IConfiguration configuration;
        public Companies(IConfiguration conf)
        {
            configuration = conf;
        }
        public async Task<IViewComponentResult> InvokeAsync(ClaimsPrincipal User)
        {

            using (var con = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {

                var UserId = User.Identity.Name;

                con.Open();
                SqlCommand CompaniesQuery = new SqlCommand("SELECT Name,CreateDate,CreatorName,Companies.id,former From Companies INNER JOIN Worker ON Companies.id=Worker.CompanyID WHERE CreatorName = '"+ UserId +"';", con);
                var Read = CompaniesQuery.ExecuteReader();
                CompaniesList CompanyL = new CompaniesList();
                List<string> employees = new List<string>();
                while (Read.Read())
                {
                    CompanyL.CompanyList.Add(new Models.Companies((int)Read.GetInt32(Read.GetOrdinal("Id")), Read[0].ToString(), employees, Read[1].ToString(), Read[2].ToString(), Read.GetInt32(4)));

                }


                return View(CompanyL);
            }
        }

    }
}



