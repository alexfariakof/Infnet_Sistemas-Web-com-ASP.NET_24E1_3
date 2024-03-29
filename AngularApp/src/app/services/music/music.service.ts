import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Music } from '../../model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class MusicService {
  public routeUrl:string = 'api/music';

  constructor(public httpClient: HttpClient) { }

  public getAllMusic(): Observable<Music[]> {
    return this.httpClient.get<Music[]>(`${ this.routeUrl }`);
  }

  public getMusicById(musicId: string): Observable<Music> {
    return this.httpClient.get<Music>(`${ this.routeUrl }/${ musicId }`);
  }

  public searchMusic(searchParam: string): Observable<Music[]> {
    return this.httpClient.get<Music[]>(`${ this.routeUrl }/search/${ searchParam }`);
  }

}
