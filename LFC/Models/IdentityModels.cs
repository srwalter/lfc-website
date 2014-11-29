using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace LFC.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public enum CertificateType
        {
            Student,
            Private,
            Commercial,
            ATP,
            CFI,
            CFII
        };
        public enum MembershipType
        {
            Full,
            Restricted,
            Inactive
        };
        public enum OfficerTitle
        {
            President,
            Secretary,
            Treasurer,
            AsstTreasurer,
            SafetyOfficer,
            GPSProgrammer,
        };

        public String ShortName { get; set; }
        public String LastName { get; set; }
        public String MiddleInitial { get; set; }
        public String FirstName { get; set; }
        public String HomeTel { get; set; }
        public String OfficeTel { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String ZipCode { get; set; }
        public CertificateType? Certificate { get; set; }
        public bool Instrument { get; set; }
        public MembershipType? MemberType { get; set; }
        public bool Safety { get; set; }
        public OfficerTitle? Officer { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}