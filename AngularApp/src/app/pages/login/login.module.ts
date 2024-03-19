import { NgModule } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { LoginRoutingModule } from "./login.routing.module";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { LoginComponent } from "./login.component";

@NgModule({
  declarations: [LoginComponent],
  imports: [LoginRoutingModule, CommonModule, MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule ],
  exports: [LoginComponent]
})

export class LoginModule { }
