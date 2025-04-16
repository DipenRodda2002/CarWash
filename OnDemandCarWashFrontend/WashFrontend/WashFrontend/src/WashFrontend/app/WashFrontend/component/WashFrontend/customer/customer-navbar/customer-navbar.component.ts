import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { CustomerService } from '../../../services/customer.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-customer-navbar',
  imports: [RouterModule],
  templateUrl: './customer-navbar.component.html',
  styleUrl: './customer-navbar.component.css'
})
export class CustomerNavbarComponent {
  constructor(private authService: AuthService, private customerService: CustomerService, private router: Router) {}

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  onclick(){
    this.router.navigate(['customer/addcar']);
  }

  onmyCarclick(){
    this.router.navigate(['customer/mycars'])
  }
  onAddressclick(){
    this.router.navigate(['address']);
  }
  onmyAddressclick(){
    this.router.navigate(['myaddress']);
  }
  onmyNewBookingclick(){
    this.router.navigate(['customer/book-wash'])
  }
  onMyBookingclick(){
    this.router.navigate(['customer/view-bookings'])
  }

}
