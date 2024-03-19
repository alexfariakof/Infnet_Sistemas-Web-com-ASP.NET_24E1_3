import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { Auth, Login } from 'src/app/model';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomInterceptor } from 'src/app/interceptors/http.interceptor.service';

describe('Unit Test AuthService', () => {
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers:[AuthService,
        { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, }]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([AuthService], (service: AuthService) => {
    // Assert
    expect(service).toBeTruthy();
  }));

  it('should send a POST request to the api/auth endpoint', inject(
    [AuthService, HttpTestingController],
    (service: AuthService, httpMock: HttpTestingController) => {
      const loginData: Login = {
        email: 'teste@teste.com',
        password: 'teste',
      };

      const mockResponse: Auth = {
        accessToken: 'fakeToken',
        expiration: '2023-01-01T00:00:00Z',
        authenticated: true,
        created: '2023-01-01T00:00:00Z',
        refreshToken: 'fakeToken',
        usertype: 'customer'
      };

      service.signIn(loginData).subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/auth';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('POST');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

});
