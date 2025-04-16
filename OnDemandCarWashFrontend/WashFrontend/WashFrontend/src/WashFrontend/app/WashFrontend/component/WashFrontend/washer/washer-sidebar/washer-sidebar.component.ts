import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-washer-sidebar',
  imports: [CommonModule],
  templateUrl: './washer-sidebar.component.html',
  styleUrl: './washer-sidebar.component.css'
})
export class WasherSidebarComponent {
   userRole:string='';

  constructor(private router:Router,private authService:AuthService){

  }


ngOnInit(): void {
    const token = localStorage.getItem('jwtToken');
    if (token) {
      const decodedToken: any = jwtDecode(token);
      this.userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    }
  }
  OnAddress(){
    this.router.navigate(['/address']);
  }
  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  OnViewBookings(){
    this.router.navigate(['washer/viewbookings'])
  }
  OnViewReviews(){
    this.router.navigate(['washer/viewreviews'])
  }
  OnPackages(){
    this.router.navigate(['washer/packages'])
  }



}
