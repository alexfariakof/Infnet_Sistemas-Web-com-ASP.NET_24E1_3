import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Playlist } from '../../model';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { CustomInterceptor } from 'src/app/interceptors/http.interceptor.service';
import { MyPlaylistService } from '..';
import { PlaylistCacheService } from './myplaylist.cache.service';

describe('MyPlaylistService', () => {
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [MyPlaylistService, PlaylistCacheService,
        { provide: HTTP_INTERCEPTORS, useClass: CustomInterceptor, multi: true, }]
    });
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([MyPlaylistService], (service: MyPlaylistService) => {
    // Assert
    expect(service).toBeTruthy();
  }));

  it('getAllPlaylist should send a get request to the api/customer/myplaylist endpoint', inject(
    [MyPlaylistService, HttpTestingController],
    (service: MyPlaylistService, httpMock: HttpTestingController) => {
        const mockResponse: Playlist[] = [
          { id: '1', name: 'Playlist 1', musics: [] },
          { id: '2', name: 'Playlist 2', musics: [] }
        ];
      service.getAllPlaylist().subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/customer/myplaylist';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

  it('getPlaylist should send a get request to the api/customer/myplaylist endpoint', inject(
    [MyPlaylistService, HttpTestingController],
    (service: MyPlaylistService, httpMock: HttpTestingController) => {
        const mockResponse: Playlist = { id: '1', name: 'Playlist 1', musics: [] };
;
      service.getPlaylist('1').subscribe((response: any) => {
        expect(response).toBeTruthy();
      });
      const expectedUrl = 'api/customer/myplaylist/1';
      const req = httpMock.expectOne(expectedUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
      httpMock.verify();
    }
  ));

});
