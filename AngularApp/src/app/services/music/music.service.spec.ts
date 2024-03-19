import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Music } from '../../model';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomInterceptor } from 'src/app/interceptors/http.interceptor.service';
import { MusicService } from '..';

describe('MusicService', () => {
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MusicService,
        { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, }]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([MusicService], (service: MusicService) => {
    // Assert
    expect(service).toBeTruthy();
  }));

  it('should send a get request to the api/music endpoint', inject(
    [MusicService, HttpTestingController],
    (service: MusicService, httpMock: HttpTestingController) => {
        const mockResponse: Music[] = [
          { id: '1', name: 'Song 1', duration: 180 },
          { id: '2', name: 'Song 2', duration: 200 }
        ];

      service.getAllMusic().subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/music';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

});
