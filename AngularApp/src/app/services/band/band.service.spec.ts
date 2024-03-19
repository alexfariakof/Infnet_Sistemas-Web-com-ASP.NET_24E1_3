import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Band } from '../../model';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomInterceptor } from 'src/app/interceptors/http.interceptor.service';
import { BandService } from './band.service';

describe('BandService', () => {
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [BandService,
        { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, }]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([BandService], (service: BandService) => {
    // Assert
    expect(service).toBeTruthy();
  }));

  it('getAllBand should send a get request to the api/band endpoint', inject(
    [BandService, HttpTestingController],
    (service: BandService, httpMock: HttpTestingController) => {
        const mockResponse: Band[] = [
        {
          id: '1', name: 'Band 1',
          album: { id: '1', name: 'Album 1', bandId: '1' },
          description: 'Band Description 1',
          backdrop: ''
        },
        {
          id: '2', name: 'Band 2',
          album: { id: '2', name: 'Album 2', bandId: '2' },
          description: 'Band Description 2',
          backdrop: ''
        }
        ];

      service.getAllBand().subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/band';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

  it('getBandById should send a get request to the api/band endpoint', inject(
    [BandService, HttpTestingController],
    (service: BandService, httpMock: HttpTestingController) => {
        const mockResponse: Band =
        {
          id: '1', name: 'Band 1',
          album: { id: '1', name: 'Album 1', bandId: '1' },
          description: 'Band Description 1',
          backdrop: ''
        };

      service.getBandById('1').subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/band/1';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));
});
