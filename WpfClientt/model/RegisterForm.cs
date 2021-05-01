using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClientt.model {
    public class RegisterForm : Customer {

        private string confirmatPassword = string.Empty;
        public Action afterSetCallback;

        [Required(ErrorMessage = "The username is not specified.")]
        [StringLength(40, MinimumLength = 3,ErrorMessage = "Username's length must be between [3-40].")]
        public override string Username {
            get => base.Username; 
            set {
                base.Username = value;
                afterSetCallback.Invoke();
            }
        }

        [Required(ErrorMessage = "Password is not specified.")]
        [DataType(DataType.Password)]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Password's length must be between [6,40]")]
        public override string Password {
            get => new string('*', base.Password.Length);
            set {
                base.Password = value;
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
        //Regex not working - error/bug
        //[RegularExpression("/^69[0-9]{8}+$",ErrorMessage = "The phone number is not a valid greek number.")]
        public override string MobilePhone {
            get => base.MobilePhone;
            set {
                base.MobilePhone = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The email address is not specified.")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",ErrorMessage = "The email field does not contain a valid email value.")]
        public override string Email {
            get => base.Email;
            set {
                base.Email = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The first name is not specified.")]
        public override string FirstName {
            get => base.FirstName;
            set {
                base.FirstName = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The last name is not specified.")]
        public override string LastName {
            get => base.LastName;
            set {
                base.LastName = value;
                afterSetCallback.Invoke();
            } 
        }

        [Required(ErrorMessage = "The street address is not specified.")]
        public override string Address {
            get => base.Address;
            set {
                base.Address = value;
                afterSetCallback.Invoke();
            } 
        }

        public string ReturnUrl { get; private set; } = "";


        public RegisterForm(Action afterSetCallback) {
            this.afterSetCallback = afterSetCallback;
            base.Password = "";
        }
    }
}
