import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route,Router,RouterModule,RouterOutlet } from '@angular/router';
import { CustomerNavbarComponent } from "./customer-navbar/customer-navbar.component";
import { CustomerService } from '../../services/customer.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customer',
  imports: [RouterModule, CustomerNavbarComponent,CommonModule],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent implements OnInit{
  packages:any[] = [];
  notification: string | null = '';
  constructor(private router: Router, private route: ActivatedRoute,private customerService:CustomerService) {}

  ngOnInit(): void {
    this.loadpackages();
    this.notification = localStorage.getItem('customerBookingModification');
    console.log(localStorage.getItem('customerBookingModification'));
  }
  

  clearNotification() {
    localStorage.removeItem('customerBookingModification');
    this.notification = null;
  }
  loadpackages():void{
    this.customerService.getAllPackages().subscribe({
      next:(data)=>{
        this.packages=data;
      },
      error:(err)=>{
        console.error("Error",err);
      }
    });
  }
  onBookClick(){
    this.router.navigate(['customer/book-wash'])
  }
  // getPackageImage(packageName: string): string {
  //   const imageMap: { [key: string]: string } = {
  //     'Basic Wash': 'https://static.vecteezy.com/system/resources/previews/003/492/197/large_2x/car-washing-car-wash-at-the-special-place-alone-man-smiling-to-the-camera-while-washing-black-car-cleaning-car-using-high-pressure-water-concept-free-photo.JPG',
  //     'Premium Wash': 'https://img.freepik.com/premium-photo/photo-man-washing-cleaning-dirty-car-car-service-center_763111-190021.jpg',
  //     'Deluxe Wash': 'https://www.sudsdeluxe.com/wp-content/uploads/Suds-Deluxe-Georgetown-33-1200x800-1.jpg'
  //   };
  //   return imageMap[packageName] || 'https://static.vecteezy.com/system/resources/previews/003/492/197/large_2x/car-washing-car-wash-at-the-special-place-alone-man-smiling-to-the-camera-while-washing-black-car-cleaning-car-using-high-pressure-water-concept-free-photo.JPG'; // Default image
  // }

  

}
