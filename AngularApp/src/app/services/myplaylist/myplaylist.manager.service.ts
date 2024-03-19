import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Playlist } from 'src/app/model';
import { PlaylistCacheService } from './myplaylist.cache.service';
import { MyPlaylistService } from './myplaylist.service';

@Injectable({
  providedIn: 'root'
})
export class PlaylistManagerService {
  private playlistsSubject = new Subject<Playlist[]>();
  playlists$ = this.playlistsSubject.asObservable();

  constructor(
    private myPlaylistService: MyPlaylistService,
    private playlistCacheService: PlaylistCacheService
  ) {
    this.getListOfPlaylists().subscribe();
  }

  getListOfPlaylists(): Observable<Playlist[]> {
    const cachedPlaylists = this.playlistCacheService.getPlaylistCache();
    if (cachedPlaylists) {
      this.playlistsSubject.next(cachedPlaylists);
      return this.playlists$;
    } else {
      return this.myPlaylistService.getAllPlaylist().pipe(
        tap(playlists => {
          this.playlistCacheService.setPlaylistCache(playlists);
          this.playlistsSubject.next(playlists);
        }),
        catchError(error => {
          this.playlistsSubject.error(error);
          return [];
        })
      );
    }
  }
}
