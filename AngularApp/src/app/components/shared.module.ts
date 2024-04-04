import { NgModule } from "@angular/core";
import { AddFavoritesComponent } from "./add-favorites/add-favorites.component";
import { FavoritesBarComponent } from "./favorites-bar/favorites-bar.component";
import { CommonModule } from "@angular/common";
import { AddFavoritesModule } from "./add-favorites/add-favorites.module";
import { FavoritesBarModule } from "./favorites-bar/favorites-bar.module";
import { AudioPlayerModule } from "./audio-player/audio-player.module";
import { AudioPlayerComponent } from "./audio-player/audio-player.component";
import { SearchModule } from "./search/search.module";
import { SearchComponent } from "./search/search.component";
import { MasterPageComponent } from './master.page/master.page.component';
import { MasterPageModule } from "./master.page/master.page.module";

@NgModule({
  imports: [CommonModule, MasterPageModule,  AddFavoritesModule, FavoritesBarModule, AudioPlayerModule, SearchModule ],
  exports: [CommonModule, MasterPageComponent, AddFavoritesComponent, FavoritesBarComponent, AudioPlayerComponent, SearchComponent]
})

export class SharedModule { }
