import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { HttpTestingController, HttpClientTestingModule } from "@angular/common/http/testing";
import { TestBed, inject } from "@angular/core/testing";
import { CustomInterceptor } from "src/app/interceptors/http.interceptor.service";
import { Customer } from "src/app/model";
import { CustomerService } from "./customer.service";

describe('CustomerService', () => {
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CustomerService,
        { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, }]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([CustomerService], (service: CustomerService) => {
    // Assert
    expect(service).toBeTruthy();
  }));

  it('should send a post request to the api/album endpoint', inject(
    [CustomerService, HttpTestingController],
    (service: CustomerService, httpMock: HttpTestingController) => {
        const mockResponse: Customer = {
          name: 'John Doe',
          email: 'john@example.com',
          password: 'password123',
          cpf: '12345678901',
          birth: '1990-01-01',
          phone: '123456789',
          address: {
            zipcode: '12345-678',
            street: 'Main St',
            number: '123',
            neighborhood: 'Downtown',
            city: 'City',
            state: 'State'
          },
          flatId: '',
          card: {}
        };

      service.create(mockResponse).subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/customer';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('POST');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

});
