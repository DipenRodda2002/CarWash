import { Component } from '@angular/core';
import { CustomerService } from '../../services/customer.service';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';
import { CustomerNavbarComponent } from '../customer/customer-navbar/customer-navbar.component';

@Component({
  selector: 'app-myaddress',
  imports: [CommonModule,FormsModule,CustomerNavbarComponent],
  templateUrl: './myaddress.component.html',
  styleUrl: './myaddress.component.css'
})
export class MyaddressComponent {

  address:Address[]= [];
  userId:string="";
  editingAddress: Address | null = null;
  tempAddress: Address = { addressId: 0, street: '', city: '', state: '', pincode: '', addressType: '' };


  constructor(private customerService:CustomerService){}

  ngOnInit(): void {
      const token = localStorage.getItem('jwtToken');
      if (token) {
        const decoded: any = jwtDecode(token);
        this.userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        console.log(this.userId);
        this.fetchAddress();
      }
    }
  
  fetchAddress(): void {
    this.customerService.getUserAddress(this.userId).subscribe({
      next: (data: Address[]) => { 
        this.address = data;
        console.log(data);
      },
      error: (err) => {
        console.error('Error fetching Address', err);
      }
    });
  }
    deletecar(addressId:number):void{
    if (confirm('Are you sure you want to delete this car?')) {
      this.customerService.deletebyaddressId(addressId).subscribe({
        next: () => {
          alert('Address deleted successfully!');
          this.address = this.address.filter(address => address.addressId !== addressId); // Remove from UI
        },
        error: (err) => {
          alert('Failed to delete address.');
          console.error(err);
        }
      });
    }
  }

  editAddress(address: Address): void {
    this.editingAddress = { ...address }; // Create a copy to avoid modifying directly
    this.tempAddress = { ...address }; 
  }

  // Save Edited Address
  saveEdit(): void {
    if (this.editingAddress) {
      this.customerService.updateUserAddress(this.tempAddress).subscribe({
        next: () => {
          this.fetchAddress(); // Refresh the list
          this.editingAddress = null; // Hide the form after update
        },
        error: (err) => {
          console.error("Error updating address:", err);
        }
      });
    }
  }
  

  cancelEdit(): void {
    this.editingAddress = null;
  }

  

}
   


export interface Address{
    addressId: number,
    street: string,
    city: string,
    state: string,
    pincode: string,
    addressType:string,
  
}
