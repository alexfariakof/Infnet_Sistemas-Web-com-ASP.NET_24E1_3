import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Login } from 'src/app/model';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private routeUrl:string = 'api/auth';

  constructor(private httpClient: HttpClient) { }

  signIn(login: Login): Observable<any> {
    return this.httpClient.post<Login>(`${ this.routeUrl }`, login);
  }
}
