import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BandComponent } from './pages/band/band.component';
import { AlbumComponent } from './pages/album/album.component';
import { MusicComponent } from './pages/musics/musics.component';
import { MyplaylistComponent } from './pages/myplaylist/myplaylist.component';

export const routes: Routes = [
    { path: '',  pathMatch: 'full', redirectTo: '' },
    { path: '', loadChildren: () => import('./pages/home/home.module').then(m => m.HomeModule) },
    { path: "login", loadChildren: () => import('./pages/login/login.module').then(m => m.LoginModule) },
    { path: "account/customer", loadChildren: () => import('./pages/account/account.module').then(m => m.AccountModule)},
    { path: "account/merchant", loadChildren: () => import('./pages/account/account.module').then(m => m.AccountModule)},
    { path: "favorites/:playlistId", component: MyplaylistComponent},
    { path: 'band', component: BandComponent},
    { path: 'band/:bandId', component: BandComponent},
    { path: 'album', component: AlbumComponent},
    { path: 'album/:albumId', component: AlbumComponent},
    { path: 'music', component: MusicComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
