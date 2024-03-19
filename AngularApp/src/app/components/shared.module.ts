import { NgModule } from "@angular/core";
import { AddFavoritesComponent } from "./add-favorites/add-favorites.component";
import { FavoritesBarComponent } from "./favorites-bar/favorites-bar.component";
import { CommonModule } from "@angular/common";
import { AddFavoritesModule } from "./add-favorites/add-favorites.module";
import { FavoritesBarModule } from "./favorites-bar/favorites-bar.module";

@NgModule({
  imports: [CommonModule, AddFavoritesModule, FavoritesBarModule ],
  exports: [CommonModule, AddFavoritesComponent, FavoritesBarComponent],
})

export class SharedModule { }
