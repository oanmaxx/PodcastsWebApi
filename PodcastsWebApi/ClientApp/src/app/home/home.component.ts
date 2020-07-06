import { Component, Inject } from '@angular/core';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { React, JSX } from 'react-jsx';
import { GoogleLoginButton } from 'ts-react-google-login-component';

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

  private SetLoggedInUser(user: User) {
    HomeComponent.loggedInUser = user;
    localStorage.setItem('loggeduser', JSON.stringify(user));
    this.isLoggedIn = true;
  }

  public LoggedUserName() {
    return HomeComponent.loggedInUser.firstName + ' ' + HomeComponent.loggedInUser.lastName;
  }

  public LoggedUserEmail() {
    return HomeComponent.loggedInUser.emailAddress;
  }

  // NORMAL LOGIN
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
            this.SetLoggedInUser(result);            
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
    if (HomeComponent.loggedInUser.googleId != null && this.authInstance != null) {      
      this.authInstance.disconnect();
      this.authInstance.signOut();
    }

    HomeComponent.loggedInUser = null;
    localStorage.removeItem('loggeduser');
    this.isLoggedIn = false;
  }

  ////////////////////////////////////////////////////////////
  //                    GOOGLE LOGIN                        //
  ///////////////////////////////////////////////////////////
  private gapiSetup;
  private authInstance;
  private googleError = {
    error: ""
  }

  async initGoogleAuth(): Promise<void> {
    //  Create a new Promise where the resolve 
    // function is the callback passed to gapi.load
    const pload = new Promise((resolve) => {
      gapi.load('auth2', resolve);
    });

    // When the first promise resolves, it means we have gapi
    // loaded and that we can call gapi.init
    return pload.then(async () => {
      await gapi.auth2
        .init({ client_id: '434963150741-lacnq641pb16n3gvl8iukecmgis01aeh.apps.googleusercontent.com' })
        .then(
            auth => {
            this.gapiSetup = true;
            this.authInstance = auth;
          },
          error => {
            console.log('We cannot initialize google login.');
            this.googleError = {
              error : "In order to enable google login, 3rd party storage access need to be allowed on the browser."
            };
        });
    });
  }

  async authenticate(): Promise<gapi.auth2.GoogleUser> {
    // Initialize gapi if not done yet
    if (!this.gapiSetup) {
      await this.initGoogleAuth();
    }

    // Resolve or reject signin Promise
    return new Promise(async () => {
      await this.authInstance.signIn().then(
        user => {
          console.log(user);
          var internalUserData = {
            id: 0,
            googleId: user['Qt']['JU'],
            firstName: user['Qt']['nW'],
            lastName: user['Qt']['nU'],
            emailAddress: user['Qt']['Au'],
          }
          this.SetLoggedInUser(internalUserData);
        },
        error => {
          this.googleError = error;
          console.log(this.googleError);
        });
    });
  }

  async checkIfUserAuthenticated(): Promise<boolean> {
    // Initialize gapi if not done yet
    if (!this.gapiSetup) {
      await this.initGoogleAuth();
    }

    return this.authInstance.isSignedIn.get();
  }

  async ngOnInit() {
    if (await this.checkIfUserAuthenticated()) {
      let check = this.authInstance.currentUser.get();
      if (check['Ea'] !== undefined) {
        console.log(check);

        if (HomeComponent.loggedInUser == null) {
          var internalUserData = {
            id: 0,
            googleId: check['Qt']['JU'],
            firstName: check['Qt']['nW'],
            lastName: check['Qt']['nU'],
            emailAddress: check['Qt']['Au'],
          }
          this.SetLoggedInUser(internalUserData);
        }
      } else {
        this.googleError = check;
        console.log(this.googleError);
      }      
    }
  }

}

export interface User {
  id: number;
  googleId: string;
  firstName: string;
  lastName: string;
  emailAddress: number;
}
