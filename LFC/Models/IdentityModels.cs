using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
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
            Inactive,
            Special,
            Retired,
            Associate,
            Waitlist
        };
        public enum OfficerTitle
        {
            President,
            Secretary,
            Treasurer,
            [Display(Name="Assistant Treasurer")]
            AsstTreasurer,
            [Display(Name="Safety Officer")]
            SafetyOfficer,
            [Display(Name="GPS Programmer")]
            GPSProgrammer,
        };

        [Display(Name="Billing Name")]
        public String ShortName { get; set; }
        [Display(Name="Last Name")]
        public String LastName { get; set; }
        [Display(Name="Middle Initial")]
        public String MiddleInitial { get; set; }
        [Display(Name="First Name")]
        public String FirstName { get; set; }
        [Display(Name="Home Phone")]
        public String HomeTel { get; set; }
        [Display(Name="Office Phone")]
        public String OfficeTel { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        [Display(Name="Zip Code")]
        public String ZipCode { get; set; }
        public CertificateType? Certificate { get; set; }
        public bool Instrument { get; set; }
        [Display(Name="Membership Type")]
        public MembershipType? MemberType { get; set; }
        public bool Safety { get; set; }
        public OfficerTitle? Officer { get; set; }
        public DateTime? BadgeExpires { get; set; }
        public String BadgeID { get; set; }

        public String FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}