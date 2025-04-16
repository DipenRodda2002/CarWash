import { Component, OnInit } from '@angular/core';
import { WashersService } from '../../../services/Washer/washers.service';
import { jwtDecode } from 'jwt-decode';
import { FormBuilder, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { WasherSidebarComponent } from "../washer-sidebar/washer-sidebar.component";
import { WasherFooterComponent } from '../washer-footer/washer-footer.component';

@Component({
  selector: 'app-view-reviews',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, WasherSidebarComponent,WasherFooterComponent],
  templateUrl: './view-reviews.component.html',
  styleUrl: './view-reviews.component.css'
})
export class ViewReviewsComponent implements OnInit {
  viewreviews: ReviewsWasher[] = [];
  filteredReviews: ReviewsWasher[] = [];
  userId: string = '';
  reviewForm: FormGroup;

  constructor(private washerService: WashersService, private fb: FormBuilder) {
    this.reviewForm = this.fb.group({
      search: [''], // Search input
      sort: ['']    // Sorting dropdown
    });
  }

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decoded: any = jwtDecode(token);
      this.userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      console.log(this.userId);
      this.washerReviews();
    }

    // Update filtered reviews on form change
    this.reviewForm.valueChanges.subscribe(() => this.filterReviews());
  }

  washerReviews() {
    this.washerService.getReviewsByWasherId(this.userId).subscribe({
      next: (data: ReviewsWasher[]) => {
        console.log("Fetched Reviews:", data);
        this.viewreviews = data;
        this.filteredReviews = data;
      },
      error: (err) => {
        console.error("Error is:", err);
      }
    });
  }

  filterReviews() {
    const searchText = this.reviewForm.value.search.toLowerCase();
    const sortType = this.reviewForm.value.sort;

    this.filteredReviews = this.viewreviews.filter(review =>
      review.email.toLowerCase().includes(searchText) || review.comment.toLowerCase().includes(searchText)
    );

    if (sortType === 'high') {
      this.filteredReviews.sort((a, b) => b.rating - a.rating);
    } else if (sortType === 'low') {
      this.filteredReviews.sort((a, b) => a.rating - b.rating);
    }
  }
}

export interface ReviewsWasher {
  email: string;
  comment: string;
  rating: number;
}
