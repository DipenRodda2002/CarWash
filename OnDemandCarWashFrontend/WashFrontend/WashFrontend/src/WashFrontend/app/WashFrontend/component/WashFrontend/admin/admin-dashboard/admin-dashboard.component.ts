import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { NgChartsModule } from 'ng2-charts'; 
import { AuthService } from '../../../services/auth.service';
import { CommonModule } from '@angular/common';
import { AdminSidebarComponent } from "../admin-sidebar/admin-sidebar.component";
import { AdminService } from '../../../services/admin/admin.service';
import { ChartConfiguration, ChartDataset, ChartOptions, ChartType } from 'chart.js';

@Component({
  selector: 'app-admin-dashboard',
  imports: [RouterModule, CommonModule,NgChartsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent {
  totalcustomerscount:number=0;
  totalwasherscount:number=0;
  totalbookingcount:number = 0;
  bookingTrendChartType: ChartType = 'bar';
  bookingStatusChartType: ChartType = 'pie';

  // Pie Chart
  // pieChartLabels = ['Completed', 'Pending', 'Cancelled'];
  // pieChartData: number[] = [];
  // pieChartType: ChartType = 'pie';

  // // Bar Chart
  // barChartLabels: string[] = [];
  // barChartData: ChartDataset[] = [
  //   { data: [], label: 'Bookings' }
  // ];
  // barChartOptions: ChartOptions = {
  //   responsive: true,
  //   plugins: {
  //     legend: { display: true },
  //     title: { display: true, text: 'Bookings Over Last 7 Days' }
  //   }
  // };
  bookingTrendChartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [
      { 
        data: [], 
        label: 'Bookings', 
        backgroundColor: '#42A5F5', 
        hoverBackgroundColor: '#1E88E5' 
      }
    ]
  };

  // bookingTrendChartOptions: ChartOptions<'bar'> = {
  //   responsive: true,
  //   plugins: {
  //     legend: {
  //       display: true
  //     }
  //   }
  // };
  bookingTrendChartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        display: true,
        position: 'top'
      },
      title: {
        display: true,
        text: 'Bookings in Last 7 Days'
      }
    },
    scales: {
      y: {
        beginAtZero: true,
        ticks: {
          stepSize: 1
        }
      }
    }
  };
  
  bookingStatusChartOptions: ChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top'
      },
      title: {
        display: true,
        text: 'Booking Status Overview'
      }
    }
  };

  bookingStatusChartData: ChartConfiguration<'pie'>['data'] = {
    labels: ['Completed', 'Pending', 'Cancelled'],
    datasets: [
      {
        data: [],
        backgroundColor: ['#4caf50', '#ffc107', '#f44336'],
        hoverOffset: 8
      }
    ]
  };

  constructor(private router:Router,private authService:AuthService,private adminService:AdminService){



  }
  ngOnInit(): void {
    this.totalCustomers();
    this.totalWashers();
    this.totalbookingscount();
    this.loadChartData();
  }

  
  logout() {
    this.authService.logout()
    this.router.navigate(['/login'])
  }

  totalCustomers(){
    this.adminService.totalCustomers().subscribe({
      next:(res)=>{
        this.totalcustomerscount=res.totalCustomers;
        console.log(res);
      },
      error: (err) => console.error('Error', err)
    })
  }
  totalWashers(){
    this.adminService.totalWashers().subscribe({
      next:(res)=>{
        this.totalwasherscount=res.totalWashers;
      },
      error:(err)=>{
        console.error(err);
      }
    })
  }

  totalbookingscount(){
    this.adminService.totalBookingsct().subscribe({
      next:(res)=>{
        this.totalbookingcount = res.totalBookings;
        console.log(res);
      },
      error:(err)=>{
        console.error(err);
      }
    })
  }

  // loadBookingStats() {
  //   this.adminService.getBookingStats().subscribe({
  //     next: res => {
  //       this.pieChartData = [
  //         res.completedBookings,
  //         res.pendingBookings,
  //         res.cancelledBookings
  //       ];

  //       this.barChartLabels = res.dailyTrends.map((d: any) => d.date);
  //       this.barChartData[0].data = res.dailyTrends.map((d: any) => d.count);
  //     },
  //     error: err => console.error('Error loading booking stats', err)
  //   });
  // }
  // loadChartData() {
  //   this.adminService.getBookingStats().subscribe({
  //     next: (res) => {
  //       this.bookingTrendChartData.labels = res.dailyTrends.map(d => d.date);
  //       this.bookingTrendChartData.datasets[0].data = res.dailyTrends.map(d => d.count);

  //       this.bookingStatusChartData.datasets[0].data = [
  //         res.completedBookings,
  //         res.pendingBookings,
  //         res.cancelledBookings
  //       ];
  //     },
  //     error: (err) => console.error(err)
  //   });
  // }
  loadChartData() {
    this.adminService.getBookingStats().subscribe({
      next: (res: BookingStatsDto) => {
        this.bookingTrendChartData.labels = res.dailyTrends.map((d: DailyBookingTrendDto) => d.date);
        this.bookingTrendChartData.datasets[0].data = res.dailyTrends.map((d: DailyBookingTrendDto) => d.count);
  
        this.bookingStatusChartData.datasets[0].data = [
          res.completedBookings,
          res.pendingBookings,
          res.cancelledBookings
        ];
      },
      error: (err) => console.error(err)
    });
  }
  

  

}

interface DailyBookingTrendDto {
  date: string;
  count: number;
}

export interface BookingStatsDto {
  totalBookings: number;
  completedBookings: number;
  pendingBookings: number;
  cancelledBookings: number;
  dailyTrends: DailyBookingTrendDto[];
}
