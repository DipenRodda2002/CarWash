import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../services/admin/admin.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-bookings',
  imports: [ReactiveFormsModule,CommonModule, RouterModule],
  templateUrl: './bookings.component.html',
  styleUrl: './bookings.component.css'
})
export class BookingsComponent implements OnInit {

  bookings:any[]=[];
  bookingForm!:FormGroup;


  constructor(private adminService:AdminService,private fb:FormBuilder){

  }
  ngOnInit(): void {
    this.fetchbookings();
    this.bookingForm  = this.fb.group({});
  }
  

  fetchbookings(){
    this.adminService.allBookings().subscribe({
      next:(res)=>{
        this.bookings=res;
        console.log(res);
      },
      error:(err)=>{
        console.error(err);
      }
    })
  }

  isSidebarCollapsed = true;

expandSidebar() {
  this.isSidebarCollapsed = false;
}

collapseSidebar() {
  this.isSidebarCollapsed = true;
}

}
