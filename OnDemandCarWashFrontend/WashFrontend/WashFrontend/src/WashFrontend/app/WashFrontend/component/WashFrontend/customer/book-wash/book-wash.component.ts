
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CustomerService } from '../../../services/customer.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { CustomerNavbarComponent } from '../customer-navbar/customer-navbar.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-book-wash',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, CommonModule, CustomerNavbarComponent],
  templateUrl: './book-wash.component.html',
  styleUrls: ['./book-wash.component.css']
})
export class BookWashComponent implements OnInit {
  userId: string | null = null;
  washers: any[] = [];
  addresses: any[] = [];
  cars: any[] = [];
  selectedAddressId: number | null = null;
  selectedCarId: number | null = null;
  serviceDate: string = '';
  TimeSlot: string = '';
  packages: any[] = [];
  selectedWasher: any = null;
  bookingForm: FormGroup;

  constructor(private customerService: CustomerService, private fb: FormBuilder,private router:Router) {
    this.bookingForm = this.fb.group({
      washerId: ['', Validators.required],
      packageId: ['', Validators.required],
      carId: ['', Validators.required],
      notes: [''],
      paymentMethod: ['', Validators.required],
      addressId: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.decodeToken();
    if (this.userId) {
      this.loadUserAddresses();
      this.loadUserCars();
      this.loadWashPackages();
    } else {
      alert('User not logged in!');
    }
  }

  // Decode JWT token to get userId
  decodeToken() {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decoded: any = jwtDecode(token);
      this.userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      console.log('User ID:', this.userId);
    }
  }

  loadWashPackages() {
    this.customerService.getAllPackages().subscribe({
      next: (data) => {
        this.packages = data;
      },
      error: (error) => {
        console.error('Error fetching wash packages:', error);
        alert('Failed to fetch wash packages.');
      }
    });
  }

  // Load user's saved addresses
  loadUserAddresses() {
    if (!this.userId) return;

    this.customerService.getUserAddress(this.userId).subscribe({
      next: (data) => {
        this.addresses = data;
      },
      error: (error) => {
        console.error('Error fetching addresses:', error);
        alert('Failed to fetch addresses.');
      }
    });
  }

  loadUserCars() {
    if (!this.userId) return;

    this.customerService.getUserCars(this.userId).subscribe({
      next: (data) => {
        console.log('User cars', data);
        this.cars = data;
      },
      error: (error) => {
        console.error('Error fetching cars:', error);
        alert('Failed to fetch cars.');
      }
    });
  }

  // Fetch available washers
  findWashers() {
    if (!this.serviceDate || !this.selectedAddressId) {
      alert('Please select an address and service date.');
      return;
    }

    this.customerService.getAvailableWashers(this.selectedAddressId, this.serviceDate, this.TimeSlot).subscribe({
      next: (data) => {
        this.washers = data;
      },
      error: (error) => {
        console.error('Error fetching washers:', error);
        alert('No washers available for the selected date.');
      }
    });
  }

  // Select a washer
  selectWasher(washer: any) {
    this.selectedWasher = washer;
    this.bookingForm.patchValue({
      washerId: washer.washerId,
      addressId: this.selectedAddressId
    });
  }

  // Confirm booking and handle payment
  confirmBooking() {
    console.log('Washer:', this.selectedWasher)
    console.log('Package ID:', this.bookingForm.value.packageId);
    console.log('Car ID:',this.bookingForm.value.carId);
    console.log('AddressID:',this.bookingForm.value.addressId);
    console.log('Service Date:', this.serviceDate);

    if (
      !this.selectedWasher ||
      !this.bookingForm.value.packageId ||
      !this.serviceDate
    ) {
      alert('Please select a washer, package, and service date.');
      return;
    }
    if (this.bookingForm.invalid || !this.serviceDate) {
      alert('Please select a washer, package, and service date.');
      return;
    }
    const selectedPackage = this.packages.find(pkg => pkg.packageId === this.bookingForm.value.packageId);
    //const totalPrice = selectedPackage ? selectedPackage.price : 0;

    const bookingRequest = {
      customerId: this.userId,
      washerId: this.bookingForm.value.washerId,
      packageId: this.bookingForm.value.packageId,
      carId: this.bookingForm.value.carId,
      notes: this.bookingForm.value.notes,
      paymentMethod: this.bookingForm.value.paymentMethod,
      addressId: this.selectedAddressId,
      serviceDate: this.serviceDate,
      TimeSlot: this.TimeSlot,
      //totalPrice: totalPrice 
    };

    this.customerService.confirmBooking(bookingRequest).subscribe({
      next: (response) => {
        console.log('Booking Response:', response);
  
        if (response && typeof response.bookingId === 'number') {
          this.router.navigate(['/customer/payment'], {
            queryParams: { 
              bookingId: response.bookingId,  
              totalPrice: response.totalPrice,  
              
              paymentMethod: bookingRequest.paymentMethod 
            }
          });
          
        } else {
          console.error('Invalid Booking ID:', response);
        }
      },
      error: (error) => {
        console.error('Error confirming booking:', error);
        alert('Booking Failed. Please try again.');
      }
    });
  }
}
