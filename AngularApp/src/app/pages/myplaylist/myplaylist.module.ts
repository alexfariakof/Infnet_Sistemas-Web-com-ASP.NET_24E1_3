import { NgModule } from "@angular/core";
import { MyplaylistComponent } from "./myplaylist.component";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { SharedModule } from "src/app/components/shared.module";
import { FlexLayoutModule } from "@angular/flex-layout";
@NgModule({
  declarations: [MyplaylistComponent],
  imports: [ FlexLayoutModule, MatCardModule, MatButtonModule, SharedModule],
  exports: [MyplaylistComponent]
})

export class MyplaylistModule { }
