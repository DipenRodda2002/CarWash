// import { Component } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../../../services/customer.service';
import { Car } from '../add-car/add-car.component';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { CustomerNavbarComponent } from '../customer-navbar/customer-navbar.component';

@Component({
  selector: 'app-mycars',
  imports: [CommonModule,CustomerNavbarComponent],
  templateUrl: './mycars.component.html',
  styleUrl: './mycars.component.css'
})
export class MycarsComponent {
  cars: MyCar[] = [];
  userId: string = '';



  constructor(private customerService: CustomerService) {}

  ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decoded: any = jwtDecode(token);
      this.userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      console.log(this.userId);
      this.fetchCars();
    }
  }

fetchCars(): void {
  this.customerService.getUserCars(this.userId).subscribe({
    next: (data: MyCar[]) => { // Explicitly typing data as MyCar[]
      this.cars = data;
      console.log(data);
    },
    error: (err) => {
      console.error('Error fetching cars', err);
    }
  });
}
  deletecar(carId:number):void{
    if (confirm('Are you sure you want to delete this car?')) {
      this.customerService.deletebyCarId(carId).subscribe({
        next: () => {
          alert('Car deleted successfully!');
          this.cars = this.cars.filter(car => car.carId !== carId); // Remove from UI
        },
        error: (err) => {
          alert('Failed to delete car.');
          console.error(err);
        }
      });
    }
  }

  

}



export interface MyCar{
  carId: number; // Required for delete
  brand: string;
  model: string;
  year: number;
  licensePlate: string;
  carImage: string;
}
