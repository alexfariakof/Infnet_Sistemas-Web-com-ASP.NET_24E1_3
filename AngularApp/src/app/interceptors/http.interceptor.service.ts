import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router, NavigationExtras } from '@angular/router';
import { LoadingComponent } from '../components';

@Injectable()
export class CustomInterceptor implements HttpInterceptor {
  private activeRequests: number = 0;
  private isModalOpen$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private modalService: NgbModal, private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.showLoader();
    let modifiedRequest: any;
    if (request.url === 'http://localhost:5055/connect/token') {
      modifiedRequest = request.clone({ url: `${request.url}` });
    } else {
      modifiedRequest = request.clone({
        url: `${request.url}`,
        setHeaders: {
          Authorization: `Bearer ${sessionStorage.getItem('@token')}`
        }
      });
    }
    return next.handle(modifiedRequest).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 403) {
          this.router.navigate(['access-denied']);
        }
        return throwError(() => error);
      }),
      finalize(() => this.hideLoader())
    );
  }

  private showLoader(): void {
    if (this.activeRequests === 0) {
      this.isModalOpen$.next(true);
      const modalRef = this.modalService.open(LoadingComponent, {
        centered: true,
        fullscreen: true,
        windowClass: 'loading-modal',
      });
      modalRef.result.then(
        () => {},
        () => {
          if (!this.isModalOpen$.value) {
            this.activeRequests = 0;
          }
        }
      );
    }
    this.activeRequests++;
  }

  private hideLoader(): void {
    this.activeRequests--;
    if (this.activeRequests === 0) {
      this.isModalOpen$.next(false);
      this.modalService.dismissAll();
    }
  }
}
