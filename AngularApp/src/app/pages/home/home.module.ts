import { NgModule } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { HomeComponent } from "./home.component";
import { SharedModule } from "src/app/components/shared.module";
import { HomeRoutingModule } from "./home.routing.module";
import { FlexLayoutModule } from "@angular/flex-layout";

@NgModule({
  declarations: [HomeComponent],
  imports: [HomeRoutingModule, MatCardModule, FlexLayoutModule, MatButtonModule, SharedModule ],
  exports: [HomeComponent]
})

export class HomeModule { }
