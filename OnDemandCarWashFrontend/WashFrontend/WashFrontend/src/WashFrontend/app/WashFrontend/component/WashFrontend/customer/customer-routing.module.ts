import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerComponent } from './customer.component';
import { AddCarComponent } from './add-car/add-car.component';
import { MycarsComponent } from './mycars/mycars.component';

// const routes: Routes = [{
//   path: '', 
//     component: CustomerComponent, 
//     children: [
//       // { path: '', component: CustomerComponent },
//       { path: 'add-car', component: AddCarComponent },
//       {path:'my-cars',component:MycarsComponent},
//       { path: '', redirectTo: 'my-cars', pathMatch: 'full' }
//     ]
//   }
// ];


// @NgModule({
//   imports: [RouterModule.forChild(routes)],
//   exports: [RouterModule]
// })
// export class CustomerRoutingModule { }
