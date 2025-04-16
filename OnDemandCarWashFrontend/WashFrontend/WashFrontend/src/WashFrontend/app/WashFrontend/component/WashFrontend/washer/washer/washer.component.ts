import { Component, OnInit } from '@angular/core';
import { WasherSidebarComponent } from '../washer-sidebar/washer-sidebar.component';
import { Router } from 'express';
import { WasherFooterComponent } from '../washer-footer/washer-footer.component';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { WashersService } from '../../../services/Washer/washers.service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-washer',
  imports: [WasherSidebarComponent,WasherFooterComponent,CommonModule],
  templateUrl: './washer.component.html',
  styleUrl: './washer.component.css'
})
export class WasherComponent implements OnInit{
  notification: string | null = '';
  recentBookings:any
  washerId:string='';
  constructor(private washerService:WashersService){
    
  }

  ngOnInit(): void {
    this.notification = localStorage.getItem('washerNotification');
    console.log(localStorage.getItem('washerNotification'));
    const token = localStorage.getItem('jwtToken');
          if (token) {
            const decoded: any = jwtDecode(token);
            this.washerId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
            console.log(this.washerId);
            this.washerrecentbookings(this.washerId);
          }
    // const washerId = localStorage.getItem('washerId');
    // console.log(washerId);
    // if (washerId) {
    //   this.washerrecentbookings(washerId);
    // }
    
  }

  clearNotification() {
    localStorage.removeItem('washerNotification');
    this.notification = null;
  }
  todayBookings = 3;
upcomingBookings = 5;
totalReviews = 12;
totalEarnings = 8500;
  washerrecentbookings(washerId:string){
    this.washerService.getrecentbookings(washerId).subscribe({
      next:(res)=>{
        this.recentBookings = res;
        console.log(res);
      },
      error:(err)=>{
        console.error(err);
      }
    })
  }

// notification = "You have 2 bookings scheduled today.";

// recentBookings = [
//   {
//     customerName: 'Rahul Sharma',
//     date: new Date(),
//     time: '10:00 AM',
//     packageName: 'Premium Wash',
//     status: 'Confirmed'
//   },
//   {
//     customerName: 'Anjali Mehra',
//     date: new Date(),
//     time: '2:00 PM',
//     packageName: 'Standard Wash',
//     status: 'Confirmed'
//   }
// ];

stats = [
  {
    title: 'Today’s Bookings',
    value: this.todayBookings,
    textClass: 'text-primary',
    borderClass: 'border-primary'
  },
  {
    title: 'Upcoming Washes',
    value: this.upcomingBookings,
    textClass: 'text-success',
    borderClass: 'border-success'
  },
  {
    title: 'My Reviews',
    value: this.totalReviews,
    textClass: 'text-warning',
    borderClass: 'border-warning'
  },
  {
    title: 'Total Earnings',
    value: '₹' + this.totalEarnings,
    textClass: 'text-info',
    borderClass: 'border-info'
  }
];



}
