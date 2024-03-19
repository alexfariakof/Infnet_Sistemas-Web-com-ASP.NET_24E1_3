import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AlbumService } from './album.service';
import { Album } from '../../model';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomInterceptor } from 'src/app/interceptors/http.interceptor.service';

describe('AlbumService', () => {
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AlbumService,
        { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, }]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([AlbumService], (service: AlbumService) => {
    // Assert
    expect(service).toBeTruthy();
  }));

  it('should send a get request to the api/album endpoint', inject(
    [AlbumService, HttpTestingController],
    (service: AlbumService, httpMock: HttpTestingController) => {
        const mockResponse: Album[] = [
        {
          id: '1', name: 'Album 1',
          bandId: ''
        },
        {
          id: '2', name: 'Album 2',
          bandId: ''
        }
      ];

      service.getAllAlbum().subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/album';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

});
