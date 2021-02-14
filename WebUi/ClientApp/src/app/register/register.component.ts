import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AccountService } from "../_services/account.service";
import { ToastrService } from 'ngx-toastr'
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from "@angular/forms";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {}
  registerForm: FormGroup ;
  validationErrors: string[] =[];
  @Output() cancelRegister= new EventEmitter();
  constructor(private accountService:AccountService,private toastr:ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }
  initializeForm() {
    this.registerForm = new FormGroup(
      {
        username: new FormControl('',Validators.required),
        password: new FormControl('',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]),
        confirmPassword: new FormControl('',[Validators.required,this.matchValues("password")]),

      });
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
      console.log(this.registerForm.get('password').errors);
      }
    );
  }

  register() {
    console.log(this.registerForm.value);
  //  this.accountService.register(this.model).subscribe(response => {
       
  //      this.cancel();
  //    },
  //    error => {
        
  //      this.validationErrors = error;
  //    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
    }
  }

}
