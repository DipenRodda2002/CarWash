import { Component } from '@angular/core';
import { CustomerService } from '../../services/customer.service';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WasherSidebarComponent } from "../washer/washer-sidebar/washer-sidebar.component";
import { WasherFooterComponent } from '../washer/washer-footer/washer-footer.component';

@Component({
  selector: 'app-packages',
  imports: [CommonModule, FormsModule, ReactiveFormsModule, WasherSidebarComponent,WasherFooterComponent],
  templateUrl: './packages.component.html',
  styleUrl: './packages.component.css'
})
export class PackagesComponent {

  packages: any[] = []; 
  userId:string='';

  constructor(private customerService: CustomerService){

  }

  ngOnInit(): void {
      const token = localStorage.getItem('jwtToken');
      if (token) {
        const decoded: any = jwtDecode(token);
        this.userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        console.log(this.userId);
        this.loadWashPackages();
      }
    }

    loadWashPackages(){
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

}
