import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';
import { CommonModule } from '@angular/common';
// import { error } from 'console';

@Component({
  selector: 'app-give-review',
  imports: [FormsModule,ReactiveFormsModule,CommonModule],
  templateUrl: './give-review.component.html',
  styleUrl: './give-review.component.css'
})
export class GiveReviewComponent {

  
  reviewForm!: FormGroup;
  bookingId!: number;
  message = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.reviewForm = this.fb.group({
      rating: ['', [Validators.required, Validators.min(1), Validators.max(5)]],
      comment: ['', [Validators.required, Validators.maxLength(300)]]
    });

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.bookingId = Number(id);
        console.log("Booking ID loaded from route:", this.bookingId);
      } else {
        console.error("Booking ID not found in route!");
      }
    });
  }

  submitReview(): void {
    if (this.reviewForm.valid && this.bookingId) {
      const reviewData = {
        bookingId: this.bookingId,
        ...this.reviewForm.value
      };

      console.log("Review Data being sent:", reviewData);

      this.customerService.submitReview(reviewData).subscribe({
        next: () => {
          this.message = 'Review submitted successfully!';
          setTimeout(() => this.router.navigate(['/customer']), 2000);
        },
        error: (error) => {
          console.error('Submit review failed:', error);
          this.message = 'Failed to submit review.';
        }
      });
    } else {
      console.warn("Form invalid or bookingId missing");
    }
  }

}
