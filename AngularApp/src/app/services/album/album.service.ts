import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Album } from '../../model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  public routeUrl:string = 'api/album';

  constructor(public httpClient: HttpClient) { }

  public getAllAlbum(): Observable<Album[]> {
    return this.httpClient.get<Album[]>(`${ this.routeUrl }`);
  }
  public getAlbumById(albumId: string): Observable<Album> {
    return this.httpClient.get<Album>(`${ this.routeUrl }/${albumId}`);
  }


}
