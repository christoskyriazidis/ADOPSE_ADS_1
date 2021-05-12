using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClientt.services;

namespace WpfClientt.viewModels {
    public abstract class FormViewModel<T> : BaseViewModel, IViewModel {
        public abstract T Form { get; protected set; }
        public ObservableCollection<ValidationResult> Errors { get; } = new ObservableCollection<ValidationResult>();
        public ICommand SubmitCommand { get; private set; }

        private ICustomerNotifier notifier;
        private string succesMessage;

        public FormViewModel(ICustomerNotifier notifier, string succesMessage) {
            SubmitCommand = new AsyncCommand(SubmitForm);
            this.notifier = notifier;
            this.succesMessage = succesMessage;
        }

        protected async Task SubmitForm() {
            Validate();
            if (Errors.Count == 0) {
                notifier.Information("Trying to submite the form.");
                await SubmitAction().Invoke(Form);
                notifier.Success(succesMessage);
                await ClearForm();
            } else {
                notifier.Error("The form can't be submitted because of the errors.");
            }
        }

        protected async Task ClearForm() {
            ClearFormStrep();
            OnPropertyChanged(nameof(Form));
            await Task.Delay(2000);
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
