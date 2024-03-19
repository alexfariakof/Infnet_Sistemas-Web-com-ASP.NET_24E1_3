import { NgModule } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { AlbumComponent } from "./album.component";
import { SharedModule } from "src/app/components/shared.module";
import { FlexLayoutModule } from "@angular/flex-layout";

@NgModule({
  declarations: [AlbumComponent],
  imports: [MatCardModule, MatButtonModule, FlexLayoutModule, SharedModule],
  exports: [AlbumComponent]
})

export class AlbumModule { }
