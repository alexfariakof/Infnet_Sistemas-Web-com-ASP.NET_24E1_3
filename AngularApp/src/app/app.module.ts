import { ToolBarModule } from './components/tool-bar/tool-bar.module';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app.routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { MatToolbarModule} from '@angular/material/toolbar';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CustomInterceptor } from './interceptors/http.interceptor.service';
import { AlbumModule } from './pages/album/album.module';
import { BandModule } from './pages/band/band.module';
import { MusicModule } from './pages/musics/musics.module';
import { AddFavoritesModule } from './components/add-favorites/add-favorites.module';
import { FavoritesBarModule } from './components/favorites-bar/favorites-bar.module';
import { MyplaylistModule } from './pages/myplaylist/myplaylist.module';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [AppComponent ],
  imports: [ BrowserModule, BrowserAnimationsModule, AppRoutingModule, HttpClientModule, CommonModule, ReactiveFormsModule, FlexLayoutModule,
    MatToolbarModule, MatFormFieldModule, MatInputModule, MatDatepickerModule,
    MyplaylistModule, AlbumModule, BandModule, MusicModule,
    ToolBarModule, AddFavoritesModule, FavoritesBarModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, },
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
