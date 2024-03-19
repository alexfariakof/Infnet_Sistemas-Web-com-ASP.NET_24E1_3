import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Playlist } from '../../model';
import { Observable, of } from 'rxjs';
import { PlaylistCacheService } from './myplaylist.cache.service';

@Injectable({
  providedIn: 'root'
})

export class MyPlaylistService {
  private routeUrl = 'api/customer/myplaylist';

  constructor(public httpClient: HttpClient, private playlistCacheService: PlaylistCacheService) { }

  public getAllPlaylist(): Observable<Playlist[]> {
    const cachedPlaylists = this.playlistCacheService.getPlaylistCache();
    if (cachedPlaylists) {
      return of(cachedPlaylists);
    } else {
      return this.httpClient.get<Playlist[]>(this.routeUrl);
    }
  }

  public getPlaylist(playlistId: string): Observable<Playlist> {
    return this.httpClient.get<Playlist>(`${this.routeUrl}/${playlistId}`);
  }
}
