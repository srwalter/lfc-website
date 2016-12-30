using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using LFC.DAL;

namespace LFC.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User ID")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Member ID")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(1, ErrorMessage = "The {0} must be at most 1 character log")]
        [Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression("\\d\\d\\d-\\d\\d\\d-\\d\\d\\d\\d", ErrorMessage = "Use the format XXX-XXX-XXXX")]
        [Display(Name = "Home Telephone")]
        public string HomeTel { get; set; }

        [RegularExpression("\\d\\d\\d-\\d\\d\\d-\\d\\d\\d\\d", ErrorMessage = "Use the format XXX-XXX-XXXX")]
        [Display(Name = "Office Telephone")]
        public string OfficeTel { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Address {get; set; }
        public string City {get; set; }
        public string State {get; set; }

        [Display(Name="Zip Code")]
        [RegularExpression("\\d\\d\\d\\d\\d")]
        public string ZipCode {get; set; }

        [Display(Name = "Certificate Type")]
        public ApplicationUser.CertificateType CertificateType { get; set; }
        [Display(Name="Membership Type")]
        public ApplicationUser.MembershipType MembershipType { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel ()
        {
        }

        public EditUserViewModel (ApplicationUser user)
        {
            this.Email = user.Email;
            this.UserName = user.UserName;
            this.ShortName = user.ShortName;
            this.LastName = user.LastName;
            this.MiddleInitial = user.MiddleInitial;
            this.FirstName = user.FirstName;
            this.Officer = user.Officer;
            this.HomeTel = user.HomeTel;
            this.OfficeTel = user.OfficeTel;
            this.Address = user.Address;
            this.City = user.City;
            this.State = user.State;
            this.ZipCode = user.ZipCode;
            this.Certificate = user.Certificate;
            this.MemberType = user.MemberType;
            this.Instrument = user.Instrument;
            this.SafetyPilot = user.Safety;
            this.BadgeExpires = user.BadgeExpires.GetValueOrDefault();
            this.BadgeID = user.BadgeID;
        }

        [Required]
        [Display(Name = "Member ID")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(1, ErrorMessage = "The {0} must be at most 1 character log")]
        [Display(Name = "Middle Initial")]
        public string MiddleInitial { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public ApplicationUser.OfficerTitle? Officer { get; set; }

        [RegularExpression("\\d\\d\\d-\\d\\d\\d-\\d\\d\\d\\d", ErrorMessage = "Use the format XXX-XXX-XXXX")]
        [Display(Name = "Home Telephone")]
        public string HomeTel { get; set; }

        [RegularExpression("\\d\\d\\d-\\d\\d\\d-\\d\\d\\d\\d", ErrorMessage = "Use the format XXX-XXX-XXXX")]
        [Display(Name = "Office Telephone")]
        public string OfficeTel { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        [Display(Name = "Zip Code")]
        [RegularExpression("\\d\\d\\d\\d\\d")]
        public string ZipCode { get; set; }

        public LFC.Models.ApplicationUser.CertificateType? Certificate { get; set; }
        [Display(Name="Instrument Rated?")]
        public bool Instrument { get; set; }

        [Display(Name="Safety Pilot?")]
        public bool SafetyPilot { get; set; }

        [Display(Name = "Membership Type")]
        public LFC.Models.ApplicationUser.MembershipType? MemberType { get; set; }

        [Display(Name = "Badge ID")]
        public String BadgeID { get; set; }

        [Display(Name = "Badge Expires")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BadgeExpires { get; set; }
    }

    public class SelectUserRolesViewModel
    {
        public SelectUserRolesViewModel ()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }

        public SelectUserRolesViewModel (ApplicationUser user) : this ()
        {
            this.UserName = user.UserName;
            this.ShortName = user.ShortName;

            var db = new LFCContext();
            var allRoles = db.Roles;
            foreach (var role in allRoles)
            {
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            foreach (var userRole in user.Roles)
            {
                var checkUserRole = this.Roles.Find(r => r.RoleId == userRole.RoleId);
                checkUserRole.Selected = true;
            }
        }

        public string UserName {get; set;}
        public string ShortName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel()
        {
        }

        public SelectRoleEditorViewModel(IdentityRole role)
        {
            this.RoleName = role.Name;
            this.RoleId = role.Id;
        }

        public bool Selected {get; set;}

        [Required]
        public string RoleName {get; set;}
        public string RoleId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
