import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {  CustomerComponent } from './customer/customer.component'
import {  MerchantComponent } from './merchant/merchant.component'

const routes: Routes = [
  { path: '', component: CustomerComponent },
  { path: '', component: MerchantComponent },
];
  @NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })

export class AccountRoutingModule{}


