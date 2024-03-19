import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Auth } from '../model';
@Injectable({
  providedIn: 'root',
})

export class AuthProvider implements CanActivate {
  private accessTokenSubject = new BehaviorSubject<string | undefined>(undefined);

  accessToken$ = this.accessTokenSubject.asObservable();

  constructor() {
    try {
      const accessToken = localStorage.getItem('@token');
      if (accessToken) {
        this.setAccessToken(accessToken);
      } else {
        this.clearLocalStorage();
      }
    } catch {
      this.clearLocalStorage();
    }
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot ): boolean {
    return this.isAuthenticated();
  }

  public clearLocalStorage() {
    this.setAccessToken(undefined);
    localStorage.clear();
  }

  private setAccessToken(token: string | undefined) {
    this.accessTokenSubject.next(token);
  }

  isAuthenticated(): boolean {
    const accessToken = this.accessTokenSubject.getValue() ?? localStorage.getItem('@token') ;
    if (accessToken === null || accessToken === undefined) {
      this.clearLocalStorage();
      return false;
    }
    return true;
  }

  createAccessToken(auth: Auth): void {
    localStorage.setItem('@token', auth.accessToken ?? '');
    this.setAccessToken(auth.accessToken);
  }
}
