import { Component, OnInit } from '@angular/core';
import { WashersService } from '../../../services/Washer/washers.service';
import { jwtDecode } from 'jwt-decode';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { WasherSidebarComponent } from '../washer-sidebar/washer-sidebar.component';
import { WasherFooterComponent } from '../washer-footer/washer-footer.component';

@Component({
  selector: 'app-viewbookings',
  imports: [ReactiveFormsModule,CommonModule,WasherSidebarComponent,FormsModule,WasherFooterComponent],
  templateUrl: './viewbookings.component.html',
  styleUrl: './viewbookings.component.css'
})
export class ViewbookingsComponent implements OnInit {
  viewbooking:BookingsWasher[] = [];
  userId:string='';
  bookingForm: FormGroup;
  message: string = '';

  constructor(private washerService:WashersService, private fb: FormBuilder){
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
    this.washerService.getBookingsByWasherId(this.userId).subscribe({
      next:(data:BookingsWasher[])=>{
        this.viewbooking=data.map(b=>({...b,tempStatus:b.orderStatus}));
      },
      error:(err)=>{
        console.error("Error is:",err);
      }
    });
  }

  get filteredBookings(): BookingsWasher[] {
    const searchText = this.bookingForm.value.search.toLowerCase();
    return this.viewbooking.filter(booking =>
      booking.name.toLowerCase().includes(searchText) ||
      booking.paymentMethod.toLowerCase().includes(searchText)
    );
  }

  updateStatus(bookingId: number, newStatus: string|undefined) {
    if (!newStatus) {
      this.message = 'Please select a valid status.';
      return;
    }
    const updatedBooking: Partial<BookingsWasher> = { orderStatus: newStatus };

    this.washerService.updateBookingStatus(bookingId, updatedBooking).subscribe({
      next: () => {
        // Update the local array with the new status
        this.viewbooking = this.viewbooking.map(booking =>
          booking.bookingId === bookingId ? { ...booking, orderStatus: newStatus, tempStatus: newStatus } : booking
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

export interface BookingsWasher{
  bookingId:number;
  name:string;
  orderDate:Date;
  serviceDate:Date;
  totalPrice:number;
  paymentMethod:string;
  orderStatus:string;
  tempStatus?: string;
  
}
