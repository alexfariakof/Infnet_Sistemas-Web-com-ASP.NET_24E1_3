import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomInterceptor } from './http.interceptor.service';

describe('CustomInterceptor', () => {
  let interceptor: CustomInterceptor;
  let modalService: NgbModal;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CustomInterceptor, NgbModal],
    });

    interceptor = TestBed.inject(CustomInterceptor);
    modalService = TestBed.inject(NgbModal);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', inject([CustomInterceptor], (service: CustomInterceptor) => {
    expect(service).toBeTruthy();
  }));
});
