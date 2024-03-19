import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FlexLayoutModule } from "@angular/flex-layout";
import { ToolBarComponent } from "./tool-bar.component";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";

@NgModule({
  declarations: [ToolBarComponent],
  imports: [CommonModule, FlexLayoutModule, MatToolbarModule, MatButtonModule],
  exports: [ToolBarComponent]
})

export class ToolBarModule { }