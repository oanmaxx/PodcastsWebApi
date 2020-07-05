import { Component, Inject } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

/** @title Input with a custom ErrorStateMatcher */
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  private static loggedInUser: User;
  public isLoggedIn: boolean;
  public lastLoginError: string;

  constructor(private httpContext: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    if (HomeComponent.GetLoggedInUser() != null) {
      this.isLoggedIn = true;
    }
  }

  public static GetLoggedInUser() {
    if (HomeComponent.loggedInUser == null) {
      let user = localStorage.getItem('loggeduser');
      if (user) {
        HomeComponent.loggedInUser = JSON.parse(user);        
      }
    }

    return HomeComponent.loggedInUser;
  }

  public LoggedUserName() {
    return HomeComponent.loggedInUser.firstName + ' ' + HomeComponent.loggedInUser.lastName;
  }

  public emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  public passwordFormControl = new FormControl('', [
    Validators.required
  ]);

  matcher = new MyErrorStateMatcher();

  public login() {
    this.lastLoginError = "";

    let emailCtrl = this.emailFormControl as FormControl;
    let passCtrl = this.passwordFormControl as FormControl;
    if (emailCtrl.valid && passCtrl.valid) {
      console.log("Logging in to:" + this.baseUrl + 'api/users/loginuser/');
      this.httpContext.get<User>(this.baseUrl + 'api/users/loginuser/' + emailCtrl.value + '/' + passCtrl.value).subscribe(
        result => {
          if (result == null) {
            this.lastLoginError = "Invalid email or password.";
          } else {
            HomeComponent.loggedInUser = result;
            localStorage.setItem('loggeduser', JSON.stringify(result));
            this.isLoggedIn = true;
          }
        },
        error => {
          console.error(error)
          this.lastLoginError = "Invalid credentials.";
        });
    } else {
      console.error("Invalid credentials.");
      this.lastLoginError = "Invalid credentials.";
    }
  }

  public logout() {
    HomeComponent.loggedInUser = null;
    localStorage.removeItem('loggeduser');
    this.isLoggedIn = false;
  }
}

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  emailAddress: number;
}
