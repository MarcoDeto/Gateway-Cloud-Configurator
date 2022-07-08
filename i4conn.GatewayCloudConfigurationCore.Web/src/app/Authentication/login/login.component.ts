import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserService } from 'src/app/Core/Services/user.service';
import { LoginModel } from '../Models/login.model';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm = this.formBuilder.group({
    username: ["TestUser", [Validators.required]],
    password: ["SecretPwd", [Validators.required]]
  });
  title = 'Login';
  subscribers: Array<Subscription>;
  loginData: LoginModel = { username: '', password: '' };
  hide = true;
  caricamento = false;
  returnUrl: any;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private snackBar: MatSnackBar
  ) {
    this.subscribers = [];
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.caricamento = true;
    this.subscribers.push(this.userService.login(this.loginForm.value).subscribe(
      (res: any) => {
        this.userService.setToken(res.token);
        this.userService.yes();
        this.caricamento = false;
        this.snackBar.open('Bentornato!', "", {
          duration: 2000,
          panelClass: "success"
        });
        this.router.navigate(['home']);
      },
      error => {
        if (error.status == 400) {
          this.snackBar.open('Autenticazione fallita!', "Errore", {
            duration: 2000,
            panelClass: "error"
          });
        }
        if (error.status == 500) {
          this.router.navigate(['/errore']);
        }
        this.caricamento = false;
      }
    ));
  }

}
