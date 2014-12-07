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
using LFC.DAL;

namespace LFC.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult FlightLogs()
        {
            var lfc = new LFCContext();
            using (var db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Steven\\Downloads\\LFC.mdb"))
            {
                var command = new OleDbCommand("SELECT m_id, flight_date, acid, start, stop FROM FlightLog");
                command.Connection = db;

                db.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var log = new FlightLog();
                    var shortname = (String)reader[0];
                    log.Date = (DateTime)reader[1];
                    var acid = (String)reader[2];
                    var plane = lfc.Airplanes.Where(x => x.AirplaneID == acid);
                    if (plane.Count() == 0)
                    {
                        continue;
                    }
                    log.Airplane = plane.First();
                    if (acid.IndexOf(shortname) >= 0)
                    {
                        continue;
                    }
                    var user = lfc.Users.Where(x => x.ShortName == shortname);
                    if (user.Count() == 0)
                    {
                        continue;
                    }
                    log.Pilot = user.First();
                    log.StartTach = (double)reader[3];
                    log.EndTach = (double)reader[4];

                    lfc.FlightLogs.Add(log);
                }
                reader.Close();
                db.Close();
                lfc.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles="Admin")]
        public ActionResult Airplanes()
        {
            var lfc = new LFCContext();
            using (var db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Steven\\Downloads\\LFC.mdb"))
            {
                var command = new OleDbCommand("SELECT acid, type, description, rate FROM Aircraft");
                command.Connection = db;

                db.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var airplane = new Airplane();
                    airplane.AirplaneID = (String)reader[0];
                    airplane.Type = (String)reader[1];
                    airplane.Description = (String)reader[2];
                    var rate = (Decimal)(reader[3] ?? 0.0f);
                    airplane.Rate = (float)Decimal.ToDouble(rate);

                    lfc.Airplanes.Add(airplane);
                }
                reader.Close();

                command = new OleDbCommand("SELECT acid, year, serial, engine_type, engine_mfg, engine_hp, cruise_speed, cruise_alt, range, range_alt, empty_wt, gross_wt, total_fuel, usable_fuel, moment, arm, voltage, oil_sump FROM equipment");
                command.Connection = db;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var acid = (String)reader[0];
                    var airplane = lfc.Airplanes.Find(acid);
                    airplane.ModelYear = (int)reader[1];
                    airplane.Serial = (String)reader[2];
                    airplane.EngineModel = (String)reader[3];
                    airplane.EngineMake = (string)reader[4];
                    airplane.HP = (int)reader[5];
                    airplane.CruiseSpeed = (int)reader[6];
                    airplane.CruiseAlt = (int)reader[7];
                    airplane.Range = (int)reader[8];
                    airplane.RangeAlt = (int)reader[9];
                    airplane.EmptyWt = (int)reader[10];
                    airplane.GrossWt = (int)reader[11];
                    var total_fuel = (double)reader[12];
                    var usable_fuel = (double)reader[13];
                    airplane.TotalFuel = (float)total_fuel;
                    airplane.UsableFuel = (float)usable_fuel;
                    airplane.Moment = (double)reader[14];
                    airplane.Arm = (float)reader[15];
                    airplane.Voltage = (int)reader[16];
                    airplane.OilSump = (int)reader[17];
                }
                reader.Close();

                command = new OleDbCommand("SELECT acid, tach_add, engine_overhaul, cur_tach, hun_hour, oil_change, annual_mon, annual_year, elt_mon, elt_year, e_bat_mon, e_bat_year, xpndr_mon, xpndr_year, static_mon, static_year, engine_serial, gps_exp FROM acstats3");
                command.Connection = db;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var acid = (String)reader[0];
                    var airplane = lfc.Airplanes.Find(acid);
                    airplane.TachAdd = (double)reader[1];
                    airplane.EngineOverhaul = (double)reader[2];
                    airplane.CurrentTach = (double)reader[3];
                    airplane.HundredHour = (double)reader[4];
                    airplane.OilChange = (int)reader[5];
                    var mon = int.Parse((String)reader[6]);
                    var year = (int)reader[7];
                    airplane.AnnualDue = new DateTime(year, mon, 28);
                    mon = int.Parse((String)reader[8]);
                    year = (int)reader[9];
                    airplane.EltDue = new DateTime(year, mon, 28);
                    mon = int.Parse((String)reader[10]);
                    year = int.Parse((String)reader[11]);
                    airplane.EltBatteryDue = new DateTime(year, mon, 28);
                    mon = int.Parse((String)reader[12]);
                    year = (int)reader[13];
                    airplane.TransponderDue = new DateTime(year, mon, 28);
                    mon = int.Parse((String)reader[14]);
                    year = (int)reader[15];
                    airplane.StaticDue = new DateTime(year, mon, 28);
                    var user = lfc.Users.First(u => u.UserName == User.Identity.Name);
                    airplane.UpdatedNow(user);

                    airplane.EngineSerial = (String)reader[16];
                    var gps_exp = (String)reader[17];
                    airplane.GPSExpires = DateTime.Parse(gps_exp);
                }
                reader.Close();

                command = new OleDbCommand("SELECT officer.plane, members.user_id FROM officer INNER JOIN members ON officer.officer_id = members.officer_id");
                command.Connection = db;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader[0] == DBNull.Value)
                    {
                        continue;
                    }
                    var acid = (String)reader[0];
                    var airplane = lfc.Airplanes.Find(acid); ;
                    var user_id = (int)reader[1];
                    var officer = lfc.Users.First(u => u.UserName == user_id.ToString());
                    airplane.MaintenanceOfficer = officer;
                }
                reader.Close();

                command = new OleDbCommand("SELECT acid, comm_a, comm_b, gps, transponder, autopilot, ic FROM equipment");
                command.Connection = db;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var equip = new Equipment();
                    equip.AirplaneID = (String)reader[0];
                    equip.Type = Equipment.EquipmentType.NAVComm;
                    equip.Description = (String)reader[1];
                    lfc.Equipments.Add(equip);

                    equip = new Equipment();
                    equip.AirplaneID = (String)reader[0];
                    equip.Type = Equipment.EquipmentType.NAVComm;
                    equip.Description = (String)reader[2];
                    lfc.Equipments.Add(equip);

                    equip = new Equipment();
                    equip.AirplaneID = (String)reader[0];
                    equip.Type = Equipment.EquipmentType.GPS;
                    equip.Description = (String)reader[3];
                    lfc.Equipments.Add(equip);

                    equip = new Equipment();
                    equip.AirplaneID = (String)reader[0];
                    equip.Type = Equipment.EquipmentType.Transponder;
                    equip.Description = (String)reader[4];
                    lfc.Equipments.Add(equip);

                    equip = new Equipment();
                    equip.AirplaneID = (String)reader[0];
                    equip.Type = Equipment.EquipmentType.Autopilot;
                    equip.Description = (String)reader[5];
                    lfc.Equipments.Add(equip);

                    equip = new Equipment();
                    equip.AirplaneID = (String)reader[0];
                    equip.Type = Equipment.EquipmentType.Intercom;
                    equip.Description = (String)reader[6];
                    lfc.Equipments.Add(equip);
                }
                reader.Close();

                db.Close();
                lfc.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Import
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Users()
        {
            var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            using (var db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\Users\\Steven\\Downloads\\LFC.mdb"))
            {
                var command = new OleDbCommand("SELECT user_id, password, m_id, last, first, middle, home_tel, office_tel, e_mail, address, city, state, zip, license, instrument, mbr_type, safety, officer_id FROM members;");
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
                    var officer = (int)((double)reader[17]);
                    switch (officer)
                    {
                        case 99:
                            break;
                        case 1:
                            user.Officer = ApplicationUser.OfficerTitle.President;
                            break;
                        case 2:
                            user.Officer = ApplicationUser.OfficerTitle.Secretary;
                            break;
                        case 3:
                            user.Officer = ApplicationUser.OfficerTitle.Treasurer;
                            break;
                        case 4:
                            user.Officer = ApplicationUser.OfficerTitle.AsstTreasurer;
                            break;
                        case 5:
                            user.Officer = ApplicationUser.OfficerTitle.SafetyOfficer;
                            break;
                        case 12:
                            user.Officer = ApplicationUser.OfficerTitle.GPSProgrammer;
                            break;
                    }
                    var passwd = (int)reader[1];
                    var result = await UserManager.CreateAsync(user, passwd.ToString());
                }
                reader.Close();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}