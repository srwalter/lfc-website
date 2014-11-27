using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using LFC.Models;

namespace LFC.Controllers
{
    public class ImportController : Controller
    {
        // GET: Import
        public async Task<ActionResult> Users()
        {
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            using (var db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Steven\\Downloads\\LFC.mdb"))
            {
                var command = new OleDbCommand("SELECT user_id, password, m_id, last, first, middle, home_tel, office_tel, e_mail, address, city, state, zip, license, instrument, mbr_type, safety FROM members;");
                command.Connection = db;

                db.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0] == DBNull.Value)
                    {
                        continue;
                    }
                    var user = new ApplicationUser();
                    var user_id = (int)reader[0];
                    user.UserName = user_id.ToString();
                    user.ShortName = (String)reader[2];
                    user.LastName = (String)reader[3];
                    user.FirstName = (String)reader[4];
                    var mbr_type = (String)reader[15];
                    if (mbr_type == "PLANE")
                    {
                        continue;
                    }
                    user.MiddleInitial = (String)(reader[5] == DBNull.Value ? "" : reader[5]);
                    user.HomeTel = (String)(reader[6] == DBNull.Value ? "" : reader[6]);
                    user.OfficeTel = (String)(reader[7] == DBNull.Value ? "" : reader[7]);
                    user.Email = (String)reader[8];
                    user.Address = (String)reader[9];
                    user.City = (String)reader[10];
                    user.State = (String)reader[11];
                    user.ZipCode = (String)reader[12];
                    var license = (String)reader[13];
                    switch (license)
                    {
                        case "S":
                            user.Certificate = ApplicationUser.CertificateType.Student;
                            break;
                        case "P":
                            user.Certificate = ApplicationUser.CertificateType.Private;
                            break;
                        case "C":
                            user.Certificate = ApplicationUser.CertificateType.Commercial;
                            break;
                        case "ATP":
                            user.Certificate = ApplicationUser.CertificateType.ATP;
                            break;
                    }
                    var instrument = (String)reader[14];
                    user.Instrument = (instrument == "yes");
                    switch (mbr_type)
                    {
                        case "RSTD":
                            user.MemberType = ApplicationUser.MembershipType.Restricted;
                            break;
                        case "FULL":
                            user.MemberType = ApplicationUser.MembershipType.Full;
                            break;
                        case "X":
                            user.MemberType = ApplicationUser.MembershipType.Inactive;
                            break;
                    }
                    var safety = (String)reader[16];
                    user.Safety = (safety == "yes");
                    var passwd = (int)reader[1];
                    var result = await UserManager.CreateAsync(user, passwd.ToString());
                }
                reader.Close();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}