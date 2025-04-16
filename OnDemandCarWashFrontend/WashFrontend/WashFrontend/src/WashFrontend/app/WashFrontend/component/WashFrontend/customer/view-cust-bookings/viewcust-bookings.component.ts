import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WashersService } from '../../../services/Washer/washers.service';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { CustomerNavbarComponent } from "../customer-navbar/customer-navbar.component";
import { CustomerService } from '../../../services/customer.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-view-bookings',
  imports: [ReactiveFormsModule, CommonModule, CustomerNavbarComponent,FormsModule,RouterModule],
  templateUrl: './view-bookings.component.html',
  styleUrl: './view-bookings.component.css'
})
export class ViewCustbookingsComponent implements OnInit {
  viewbooking:BookingsCustomer[] = [];
  userId:string='';
  bookingForm: FormGroup;
  message: string = '';

  constructor(private customerService:CustomerService, private fb: FormBuilder){
    this.bookingForm = this.fb.group({
      search: [''] // Form control for searching/filtering
    });

  }

  ngOnInit(): void {
      const token = localStorage.getItem('jwtToken');
      if (token) {
        const decoded: any = jwtDecode(token);
        this.userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        console.log(this.userId);
        this.WasherBookings();
      }
    }

  WasherBookings(){
    this.customerService.getBookingsByCustId(this.userId).subscribe({
      next:(data:BookingsCustomer[])=>{
        this.viewbooking=data;
      },
      error:(err)=>{
        console.error("Error is:",err);
      }
    });
  }

  get filteredBookings(): BookingsCustomer[] {
    const searchText = this.bookingForm.value.search.toLowerCase();
    return this.viewbooking.filter(booking =>
      booking.name.toLowerCase().includes(searchText) ||
      booking.paymentMethod.toLowerCase().includes(searchText)
    );
  }

  updateStatus(bookingId: number, newStatus: string) {
    if (confirm("Are you sure you want to cancel this booking?")) {
    const updatedBooking: Partial<BookingsCustomer> = { orderStatus: newStatus };

    this.customerService.updateBookingStatus(bookingId, updatedBooking).subscribe({
      next: () => {
        // Update the local array with the new status
        this.viewbooking = this.viewbooking.map(booking =>
          booking.bookingId === bookingId ? { ...booking, orderStatus: newStatus } : booking
        );
        this.message = 'Order status updated successfully! ✅'; // ✅ Set success message
        setTimeout(() => this.message = '', 3000); // Hide message after 3 seconds
      },
      error: (err) => {
        console.error('Error updating order status:', err);
        this.message = 'Failed to update order status. ❌';
      }
    });
  }
  }

  

}

export interface BookingsCustomer{
  bookingId:number;
  name:string;
  orderDate:Date;
  serviceDate:Date;
  totalPrice:number;
  paymentMethod:string;
  orderStatus:string;
  
}