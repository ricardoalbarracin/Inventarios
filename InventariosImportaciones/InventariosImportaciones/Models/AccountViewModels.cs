using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inventarioImportaciones.Models
{

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {

        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailRequerido")]
        [Display(Name = "Email", ResourceType = typeof(Resourses.Global))]  
        [EmailAddress(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailInvalido")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "passwordRequerido")]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resourses.Global))]
        public string Password { get; set; }

        [Display(Name = "Recordarme", ResourceType = typeof(Resourses.Global))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {

        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "validacionnombres")]
        [Display(Name = "Nombres", ResourceType = typeof(Resourses.Global))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailRequerido")]
        [Display(Name = "Email", ResourceType = typeof(Resourses.Global))]
        [EmailAddress(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailInvalido")]
        public string Email { get; set; }

        //[StringLength(100, ErrorMessage = "La {0} debe tener por lo menos {2} caracteres de longitud.", MinimumLength = 6)]
        [StringLength(100,ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "validacionlongitudcontraseña", MinimumLength = 6)]
        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "passwordRequerido")]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resourses.Global))]
        public string Password { get; set; }

        
        
        [Compare("Password", ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "validacionconfirmarpassword")]
        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "validacionconfirmarpassword")]
        [DataType(DataType.Password)]
        [Display(Name = "confirmarpassword", ResourceType = typeof(Resourses.Global))]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailRequerido")]
        [Display(Name = "Email", ResourceType = typeof(Resourses.Global))]
        [EmailAddress(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailInvalido")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "validacionlongitudcontraseña", MinimumLength = 6)]
        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "passwordRequerido")]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resourses.Global))]
        public string Password { get; set; }

        [Compare("Password", ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "validacionconfirmarpassword")]
        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "validacionconfirmarpassword")]
        [DataType(DataType.Password)]
        [Display(Name = "confirmarpassword", ResourceType = typeof(Resourses.Global))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailRequerido")]
        [Display(Name = "Email", ResourceType = typeof(Resourses.Global))]
        [EmailAddress(ErrorMessageResourceType = (typeof(Resourses.Global)), ErrorMessageResourceName = "EmailInvalido")]
        public string Email { get; set; }
    }


}