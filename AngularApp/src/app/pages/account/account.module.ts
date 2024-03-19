import { NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { MatNativeDateModule } from "@angular/material/core";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { AccountRoutingModule } from "./account.routing.module";
import { CustomerComponent } from './customer/customer.component'
import { MerchantComponent } from './merchant/merchant.component'

@NgModule({
  declarations: [CustomerComponent, MerchantComponent ],
  imports: [AccountRoutingModule, CommonModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, FlexLayoutModule],
  exports: [CustomerComponent, MerchantComponent]
})

export class AccountModule { }
