import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Interface } from 'readline';
import { jwtDecode } from 'jwt-decode';


@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['admin@gmail.com', [Validators.required, Validators.email]],
      password: ['Admin@123', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      console.log('Form is invalid');
      return;
    }

    const loginPayload: ILogin = this.loginForm.value;

    this.authService.login(loginPayload).subscribe({
      next: (res) => {
        console.log('Login Successful', res);
        const token = res.jwtToken;
        localStorage.setItem('jwtToken', res.jwtToken);
        const decodedToken : any= jwtDecode(token);
        console.log(decodedToken);
        const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

        if (userRole === 'Admin') {
          this.router.navigate(['/admin']);
        } else if (userRole === 'Customer') {
          this.router.navigate(['/customer']);
        } else if (userRole === 'Washer') {
          this.router.navigate(['/washer']);
        } else {
          console.error('Unknown role');
        }
      },
      error: (err) => {
        console.error('Login Failed', err);
      }
    });
  }
}
export interface ILogin {
  username: string;
  password: string;
}
