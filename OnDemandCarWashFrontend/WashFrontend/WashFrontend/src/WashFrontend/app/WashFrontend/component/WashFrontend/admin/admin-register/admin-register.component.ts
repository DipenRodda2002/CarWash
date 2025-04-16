import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AdminService } from '../../../services/admin/admin.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-register',
  imports: [ReactiveFormsModule,CommonModule,RouterModule],
  templateUrl: './admin-register.component.html',
  styleUrl: './admin-register.component.css'
})
export class AdminRegisterComponent {

  registerForm: FormGroup;
  submitted = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private adminService: AdminService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      name: ['', Validators.required],
      phone: ['', Validators.required],
      password: ['', Validators.required],
      profileImage: [''] ,// optional,
      roles: ['Admin', [Validators.required]]
    });
  }

  onSubmit() {
    
    this.submitted = true;
    if (this.registerForm.invalid) return;

    this.adminService.registerAdmin(this.registerForm.value).subscribe({
      next: () => {
        alert('Admin registered successfully!');
        this.router.navigate(['/admin']); // or wherever you want
      },
      error: err => {
        this.errorMessage = 'Failed to register admin.';
        console.error(err);
      }
    });
  }

}
