import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AdminService } from '../../../services/admin/admin.service';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';
import { PackageFormComponent } from '../package-form/package-form.component';

@Component({
  selector: 'app-package-management',
  imports: [ReactiveFormsModule,CommonModule,PackageFormComponent, RouterModule],
  templateUrl: './package-management.component.html',
  styleUrl: './package-management.component.css'
})
export class PackageManagementComponent  {
  // packageForm!: FormGroup;
  packages: any[] = [];
  // isEditMode: boolean = false;
  // selectedPackageId: string = '';
  showForm: boolean = false;
  packageData: any = null;

  constructor(private fb: FormBuilder, private adminService: AdminService,private customerService:CustomerService) {}

  ngOnInit(): void {

    this.loadPackages();
  }
  showAddForm() {
    if (this.showForm && !this.packageData) {
      this.showForm = false;
      return;
    }
    this.showForm = true;
    this.packageData=null;
  }

  loadPackages() {
    this.customerService.getAllPackages().subscribe({
      next: (res: any) => {
        this.packages = res;
      },
      error: err => console.error(err)
    });
  }


  editPackage(pkg: any): void {
    if (this.showForm && this.packageData?.packageId === pkg.packageId) {
      this.showForm = false;
      this.packageData = null;
      return;
    }

    this.packageData = { ...pkg };
    this.showForm = true;
  }

  deletePackage(packageId: string): void {
    if (confirm('Are you sure you want to delete this package?')) {
      this.adminService.deletePackage(packageId).subscribe({
        next: () => {
          alert('Package deleted!');
          this.loadPackages();
        },
        error: err => console.error(err)
      });
    }
  }

  onCancelForm(): void {
    this.showForm = false;
    this.packageData = null;
  }
  onFormSubmit(): void {
    this.showForm = false;
    this.packageData = null;
    this.loadPackages();
  }


}
