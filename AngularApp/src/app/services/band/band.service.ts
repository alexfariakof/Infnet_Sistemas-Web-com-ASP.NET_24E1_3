import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Band } from '../../model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class BandService {
  public routeUrl:string = 'api/band';

  constructor(public httpClient: HttpClient) { }

  public getAllBand(): Observable<Band[]> {
    return this.httpClient.get<Band[]>(`${ this.routeUrl }`);
  }

  public getBandById(bandId: string): Observable<Band> {
    return this.httpClient.get<Band>(`${ this.routeUrl }/${bandId}`);
  }
}
