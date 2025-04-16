import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// import { CustomerRoutingModule } from './customer-routing.module';
import { RouterModule } from '@angular/router';
import { CustomerComponent } from './customer.component';
import { AddCarComponent } from './add-car/add-car.component';
import { MycarsComponent } from './mycars/mycars.component';
import { AddressComponent } from '../address/address.component';
import { MyaddressComponent } from '../myaddress/myaddress.component';


@NgModule({
  declarations: [
    
  ],
  imports: [
    CommonModule,
    RouterModule,
    CustomerComponent,
    AddCarComponent,
    MycarsComponent,
    AddressComponent,
    MyaddressComponent,
  ]
})
export class CustomerModule { }
