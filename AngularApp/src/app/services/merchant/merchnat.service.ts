import { Merchant } from 'src/app/model';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class MerchantService {
  private routeUrl:string = 'api/merchant';

  constructor(public httpClient: HttpClient) { }

  public create(merchant: Merchant): Observable<any> {
    return this.httpClient.post<Merchant>(`${ this.routeUrl }`, merchant);
  }

}
