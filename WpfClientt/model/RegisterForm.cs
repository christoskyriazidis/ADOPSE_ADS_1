using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    /// <summary>
    /// A model class for registration form.A decorator that calls a callback each time a 
    /// setter is called.
    /// </summary>
    public class RegisterForm {
        private string username = string.Empty;
        private string password = string.Empty;
        private string confirmatPassword = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string phone = string.Empty;
        private string email = string.Empty;
        private string address = string.Empty;

        public Action afterSetCallback;

        [Required(ErrorMessage = "The username is not specified.")]
        [StringLength(40, MinimumLength = 3,ErrorMessage = "Username's length must be between [3-40].")]
        public string Username {
            get => username; 
            set {
                username = value;
                afterSetCallback.Invoke();
            }
        }

        [Required(ErrorMessage = "Password is not specified.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Password's length must be between [6,40]")]
        public string Password {
            get => new string('*', password.Length);
            set {
                password = value;
                afterSetCallback();
            }
        }

        [Required(ErrorMessage = "Configramtion password is not specified.")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "The two passwords do not match.")]
        public string ConfirmPassword {
            get => new string('*',confirmatPassword.Length);
            set {
                confirmatPassword = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The phone number is not specifided.")]
        [RegularExpression("^69[0-9]{8}",ErrorMessage = "The phone number is not a valid greek number.")]
        public string MobilePhone {
            get => phone;
            set {
                phone = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The email address is not specified.")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",ErrorMessage = "The email field does not contain a valid email value.")]
        public string Email {
            get => email;
            set {
                email = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The first name is not specified.")]
        public string FirstName {
            get => firstName;
            set {
                firstName = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The last name is not specified.")]
        public string LastName {
            get => lastName;
            set {
                lastName = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The street address is not specified.")]
        public string Address {
            get => address;
            set {
                address = value;
                afterSetCallback.Invoke();
            } 
        }

        public string ReturnUrl { get; private set; } = "";


        public RegisterForm(Action afterSetCallback) {
            this.afterSetCallback = afterSetCallback;
        }
    }
}
