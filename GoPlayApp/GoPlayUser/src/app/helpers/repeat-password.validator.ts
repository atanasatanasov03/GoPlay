import { FormGroup } from "@angular/forms";

export function ConfirmPasswordValidator(pass: string, repeat: string) {
    return (formGroup: FormGroup) => {
      let password = formGroup.controls[pass];
      let repeatPassword = formGroup.controls[repeat]

      if ( repeatPassword.errors && !repeatPassword.errors.confirmPasswordValidator ) return;
      
      if (password.value !== repeatPassword.value) 
        repeatPassword.setErrors({ confirmPasswordValidator: true });
      else 
        repeatPassword.setErrors(null);
    };
  }