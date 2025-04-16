import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CustomerService } from '../../services/customer.service';
import { CustomerNavbarComponent } from '../customer/customer-navbar/customer-navbar.component';
import { Router } from '@angular/router';
import { WasherSidebarComponent } from '../washer/washer-sidebar/washer-sidebar.component';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { WasherFooterComponent } from "../washer/washer-footer/washer-footer.component";
@Component({
  selector: 'app-address',
  imports: [ReactiveFormsModule, CustomerNavbarComponent, WasherSidebarComponent, CommonModule, WasherFooterComponent],
  templateUrl: './address.component.html',
  styleUrl: './address.component.css'
})
export class AddressComponent  implements OnInit {
  addressForm !: FormGroup
  userRole: string = '';
  constructor(
    private fb:FormBuilder,
    private customerService: CustomerService,
    private router:Router

  ){}

  ngOnInit(): void {
    this.addressForm = this.fb.group({
      street: ['', Validators.required],
      city: ['', Validators.required],
      pincode: ['', Validators.required],
      state: ['', Validators.required],
      addressType:['Home',Validators.required]
    });
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decoded: any = jwtDecode(token);
      this.userRole = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]; // Assuming role is stored in JWT
    }
  }
  onSubmit(): void {
    if (this.addressForm.valid) {
      const address: MyAddress = this.addressForm.value;

      this.customerService.AddAddress(address).subscribe({
        next: res => {
          alert('Address added successfully!');
          this.router.navigate(['myaddress'])
          this.addressForm.reset();
        },
        error: err => {
          alert('Failed to add Address.');
          console.error(err);
        }
      });
    } else {
      alert('Please fill out all fields');
    }
  }


}

export interface MyAddress{
  street: string,
  city: string,
  pincode: string,
  state: string,
  addressType: string
}
