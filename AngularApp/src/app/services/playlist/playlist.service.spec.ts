import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Playlist } from '../../model';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomInterceptor } from 'src/app/interceptors/http.interceptor.service';
import { PlaylistService } from '..';

describe('PlaylistService', () => {
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [PlaylistService,
        { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, }]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([PlaylistService], (service: PlaylistService) => {
    // Assert
    expect(service).toBeTruthy();
  }));

  it('should send a get request to the api/playlist endpoint', inject(
    [PlaylistService, HttpTestingController],
    (service: PlaylistService, httpMock: HttpTestingController) => {
        const mockResponse: Playlist[] = [
          { id: '1', name: 'Playlist 1', musics: [] },
          { id: '2', name: 'Playlist 2', musics: [] }
        ];
      service.getAllPlaylist().subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/playlist';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

});
