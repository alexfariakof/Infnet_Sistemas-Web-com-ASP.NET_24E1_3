import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Playlist } from '../../model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class PlaylistService {
  public routeUrl:string = 'api/playlist';

  constructor(public httpClient: HttpClient) { }

  public getAllPlaylist(): Observable<Playlist[]> {
    return this.httpClient.get<Playlist[]>(`${ this.routeUrl }`);
  }
}
