import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../../../services/customer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-booking-details',
  imports: [CommonModule],
  templateUrl: './booking-details.component.html',
  styleUrl: './booking-details.component.css'
})
export class BookingDetailsComponent implements OnInit {
  bookingDetails!: BookingDetails;
  isLoading = true;
  errorMessage = '';

  constructor(
    private bookingService: CustomerService,
    private route: ActivatedRoute,
    private router:Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    //const id = this.route.snapshot.paramMap.get('id');
    console.log('Route Param ID:', id);
    if (id) {
      this.bookingService.getBookingById(id).subscribe({
        next: (data) => {
          this.bookingDetails = data;
          this.isLoading = false;
        },
        error: (err) => {
          this.errorMessage = 'Booking not found or an error occurred.';
          this.isLoading = false;
        }
      });
    }
  }
  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending':
        return 'badge-pending';
      case 'confirmed':
        return 'badge-confirmed';
      case 'completed':
        return 'badge-completed';
      case 'cancelled':
        return 'badge-cancelled';
      default:
        return 'badge-default';
    }
    
  }
  downloadReceipt() {
    window.print();
  }
  ondashboard(){
    this.router.navigate(['/customer'])
  }
  
}

export interface BookingDetails {
  bookingId: number;
  customerName: string;
  washerName: string;
  city: string;
  serviceDate: string;  
  timeSlot: string;
  status: string;
  paymentMethod: string;
  paymentId: string;
  totalPrice: number;
}
