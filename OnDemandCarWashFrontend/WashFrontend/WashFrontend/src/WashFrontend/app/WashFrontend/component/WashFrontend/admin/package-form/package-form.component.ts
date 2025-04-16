import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AdminService } from '../../../services/admin/admin.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-package-form',
  imports: [ReactiveFormsModule],
  templateUrl: './package-form.component.html',
  styleUrl: './package-form.component.css'
})
export class PackageFormComponent implements OnInit {
  @Input() packageData: any = null;
  @Output() formSubmitted = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  packageForm!: FormGroup;
  isEditMode = false;
  selectedPackageId: string = '';

  constructor(private fb: FormBuilder, private adminService: AdminService) {}

  ngOnInit(): void {
    this.packageForm = this.fb.group({
      packageName: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(1)]],
      duration: ['', Validators.required]
    });

    if (this.packageData) {
      this.isEditMode = true;
      this.selectedPackageId = this.packageData.packageId;
      this.packageForm.patchValue(this.packageData);
    }
  }

  onSubmit(): void {
    if (this.packageForm.invalid) return;

    const packageInfo = this.packageForm.value;

    if (this.isEditMode) {
      this.adminService.updatePackage(this.selectedPackageId, packageInfo).subscribe({
        next: () => {
          alert('Package updated successfully!');
          this.formSubmitted.emit();
        },
        error: (err) => console.error('Update failed', err)
      });
    } else {
      this.adminService.addPackage(packageInfo).subscribe({
        next: () => {
          alert('Package added successfully!');
          this.formSubmitted.emit();
        },
        error: (err) => console.error('Add failed', err)
      });
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }
}