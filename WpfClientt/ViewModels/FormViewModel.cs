using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfClientt.viewModels {
    public abstract class FormViewModel<T> : BaseViewModel, IViewModel {
        public abstract T Form { get; protected set; }
        public ObservableCollection<ValidationResult> Errors { get; } = new ObservableCollection<ValidationResult>();
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public ICommand SubmitCommand { get; private set; }

        public FormViewModel() {
            SubmitCommand = new AsyncCommand(SubmitForm);
        }

        protected async Task SubmitForm() {
            Validate();
            if (Errors.Count == 0) {
                Messages.Add("Trying to submite the form.");
                await SubmitAction().Invoke(Form);
                Messages.Clear();
                Messages.Add("The form has been submitted successfully.");
                await ClearForm();
            } else {
                Messages.Clear();
                Messages.Add("The form can't be submitted because of the errors.");
            }
        }

        protected async Task ClearForm() {
            ClearFormStrep();
            OnPropertyChanged("Form");
            await Task.Delay(2000);
            Messages.Clear();
        }


        protected void Validate() {
            Errors.Clear();
            ValidationContext validationContext = new ValidationContext(Form);
            Validator.TryValidateObject(Form ,validationContext, Errors, true);
        }

        protected abstract Func<T,Task> SubmitAction();

        protected abstract void ClearFormStrep();

    }
}
