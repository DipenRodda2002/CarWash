import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../services/admin/admin.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-user-management',
  imports: [ReactiveFormsModule,CommonModule,FormsModule, RouterModule],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit{

  users:User[]=[];
  userForm !: FormGroup
  filteredUsers: any[] = [];
  selectedRole: string = 'All';
  constructor(private adminService:AdminService,private fb:FormBuilder){}

  ngOnInit(): void {
    this.loadUsers();
    this.userForm  = this.fb.group({});
  }

  loadUsers(): void {
    debugger
    this.adminService.getAllUsers().subscribe({
      
      next:(data)=>{
        
        this.users = data as any[];
        this.filterUsers();
        console.log("Data ",data);
      },
      error:(error)=>{
        console.error(error);
      }
    })
  }

filterUsers(): void {
    if (this.selectedRole === 'All') {
      this.filteredUsers = this.users;
    } else {
      this.filteredUsers = this.users.filter(user => user.roles === this.selectedRole);
    }
  }

  // editUser(user: any): void {
  //   // You can navigate to an Edit User page or open a modal for editing.
  //   console.log("Edit User:", user);
  // }

  deleteUser(userId: string): void {
    if (confirm("Are you sure you want to delete this user?")) {
      this.adminService.deleteuser(userId).subscribe({
        next: () => {
          alert("User deleted successfully.");
          this.loadUsers();
        },
        error: (err) => {
          console.error(err);
          alert("Failed to delete user.");
        }
      });
    }
  }
  toggleStatus(user: any): void {
    const updatedStatus = !user.isActive;
  
    this.adminService.toggleUserStatus(user.userId, updatedStatus).subscribe({
      next: (res: any) => {
        alert(`User is now ${updatedStatus ? 'Active' : 'Blocked'}`);
        this.loadUsers(); // Refresh the list
      },
      error: (err) => {
        console.error(err);
        alert('Failed to toggle user status');
      }
    });
  }

  // toggleBlock(user: any): void {
  //   const newStatus = !user.isActive;
  //   this.adminService.updateUserStatus(user.userId, newStatus).subscribe({
  //     next: () => {
  //       user.isActive = newStatus;
  //       alert(`User has been ${newStatus ? 'unblocked' : 'blocked'}.`);
  //     },
  //     error: (err) => {
  //       console.error(err);
  //       alert("Failed to update user status.");
  //     }
  //   });


}
export interface User {
  userId: string;
  name: string;
  email: string;
  phone: string;
  profileImage: string;
  createdAt: Date;
  isActive: boolean;
  roles: string;
}

