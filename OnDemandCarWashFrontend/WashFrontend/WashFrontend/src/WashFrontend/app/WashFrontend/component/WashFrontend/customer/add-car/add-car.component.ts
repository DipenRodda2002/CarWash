import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomerService } from '../../../services/customer.service';
import { CustomerNavbarComponent } from '../customer-navbar/customer-navbar.component';
import { Route } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-car',
  imports: [ReactiveFormsModule,CustomerNavbarComponent],
  templateUrl: './add-car.component.html',
  styleUrl: './add-car.component.css'
})
export class AddCarComponent implements OnInit {
  carForm !: FormGroup
  selectedFile!: File;
  constructor(
    private fb:FormBuilder,
    private customerService: CustomerService,
    private router:Router

  ){}

  ngOnInit(): void {
    this.carForm = this.fb.group({
      brand: ['', Validators.required],
      model: ['', Validators.required],
      year: ['', Validators.required],
      licensePlate: ['', Validators.required],
      // carImage: '' 
    });
  }
  onFileChange(event: any): void {
    if (event.target.files && event.target.files.length > 0) {
      this.selectedFile = event.target.files[0];
    }
  }
  // onSubmit(): void {
  //   if (this.carForm.valid) {
  //     const car: Car = this.carForm.value;

  //     this.customerService.addCar(car).subscribe({
  //       next: res => {
  //         alert('Car added successfully!');
  //         this.carForm.reset();
  //         this.router.navigate(['customer/mycars'])
  //       },
  //       error: err => {
  //         alert('Failed to add car.');
  //         console.error(err);
  //       }
  //     });
  //   } else {
  //     alert('Please fill out all fields');
  //   }
  // }
  onSubmit(): void {
    if (this.carForm.valid && this.selectedFile) {
      const formData = new FormData();
      formData.append('brand', this.carForm.get('brand')?.value);
      formData.append('model', this.carForm.get('model')?.value);
      formData.append('year', this.carForm.get('year')?.value);
      formData.append('licensePlate', this.carForm.get('licensePlate')?.value);
      formData.append('carImage', this.selectedFile); // this is the file

      this.customerService.addCar(formData).subscribe({
        next: res => {
          alert('Car added successfully!');
          this.carForm.reset();
          this.router.navigate(['customer/mycars']);
        },
        error: err => {
          alert('Failed to add car.');
          console.error(err);
        }
      });
    } else {
      alert('Please fill all fields and upload an image');
    }
  }

}


export interface Car {
  brand:string;
  model: string;
  year: number;
  licensePlate: string; 
  carImage:string;
  
}


