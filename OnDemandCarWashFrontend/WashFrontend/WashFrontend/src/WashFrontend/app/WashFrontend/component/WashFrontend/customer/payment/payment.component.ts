import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';

@Component({
  selector: 'app-payment',
  imports: [],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {
  bookingId: number = 0;
  amount: number = 0;
  paymentMethod: string = '';

  constructor(
    private route: ActivatedRoute,
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.bookingId = Number(params['bookingId']);  // ✅ Ensure number
      this.amount = Number(params['totalPrice']);    // ✅ Ensure number
      this.paymentMethod = params['paymentMethod'];
    });
  }

 
  proceedWithPayment() {
    this.customerService.proceedToPayment(
      this.bookingId,
      this.amount,
      this.paymentMethod
    ).subscribe({
      next: (response) => {
        alert(
          this.paymentMethod === 'Cash'
            ? 'Booking confirmed with Cash Payment.'
            : 'Payment Successful!'
        );
        this.router.navigate([`/customer/booking/${response.bookingId}`]);
      },
      error: (error) => {
        console.error('Payment failed:', error);
        alert('Payment failed. Please try again.');
      }
    });
  }
    
}