import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Playlist } from 'src/app/model';

@Injectable({
  providedIn: 'root'
})
export class PlaylistCacheService {
  private playlistCacheSubject = new BehaviorSubject<Playlist[] | null>(null);
  playlistCache$: Observable<Playlist[] | null> = this.playlistCacheSubject.asObservable();

  constructor() {}

  setPlaylistCache(playlists: Playlist[]): void {
    this.playlistCacheSubject.next(playlists);
  }

  getPlaylistCache(): Playlist[] | null {
    return this.playlistCacheSubject.getValue();
  }
}
