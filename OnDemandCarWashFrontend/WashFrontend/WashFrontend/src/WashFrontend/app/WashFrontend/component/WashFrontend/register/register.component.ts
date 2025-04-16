// import { Component } from '@angular/core';
import { EmailValidator, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { Component } from '@angular/core';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})

export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      name: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      profileImage: [''],
      roles: ['Customer', [Validators.required]] // Default role string
    });
  }

  register() {
    if (this.registerForm.valid) {
      const formValue = this.registerForm.value;
      const payload: IUserRegister = {
        ...formValue,
        roles: [formValue.roles]
      };

      this.authService.register(payload).subscribe({
        next: () => {
          alert('Registration Successful!');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          alert('Registration Failed!');
          console.error(err);
        }
      });
    } else {
      alert('Please fill all required fields!');
    }
  }
}
export interface IUserRegister {
  username: string;
  password: string;
  name: string;
  phone: string;
  profileImage: string;
  roles: string; // single role, not array
}
