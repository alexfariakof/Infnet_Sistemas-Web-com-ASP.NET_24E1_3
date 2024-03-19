import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AlbumComponent } from './album.component';
import { ActivatedRoute } from '@angular/router';
import { AlbumService } from 'src/app/services';
import { of, throwError } from 'rxjs';
import { Album } from 'src/app/model';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('AlbumComponent', () => {
  let component: AlbumComponent;
  let fixture: ComponentFixture<AlbumComponent>;
  let albumService: AlbumService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AlbumComponent],
      imports: [RouterTestingModule, HttpClientTestingModule],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ albumId: '1' })
          }
        },
        AlbumService
      ]
    });
    fixture = TestBed.createComponent(AlbumComponent);
    component = fixture.componentInstance;
    albumService = TestBed.inject(AlbumService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve album on initialization', fakeAsync(() => {
    // Arrange
    const mockAlbum: Album = {
      id: '1',
      name: 'Album 1',
      bandId: '1',
      musics: [
        { id: '1', name: 'Music 1', duration: 20 },
        { id: '2', name: 'Music 2', duration: 30 }
      ]
    };
    spyOn(albumService, 'getAlbumById').and.returnValue(of(mockAlbum));

    // Act
    fixture.detectChanges();
    tick();

    // Assert
    expect(albumService.getAlbumById).toHaveBeenCalledWith('1');
    expect(component.album).toEqual(mockAlbum);
    expect(component.musics).toEqual(mockAlbum.musics ?? []);
  }));

  it('should handle error when retrieving album', fakeAsync(() => {
    // Arrange
    const errorMessage = 'Error retrieving album';
    spyOn(albumService, 'getAlbumById').and.returnValue(throwError(() => errorMessage));

    // Act
    fixture.detectChanges();
    tick();

    // Assert
    expect(albumService.getAlbumById).toHaveBeenCalledWith('1');
    expect(component.album).toEqual({ id: '', name: '', bandId: '' });
    expect(component.musics).toEqual([]);
  }));

  it('should handle null response when retrieving album', fakeAsync(() => {
    // Arrange
    const mockAlbum: Album | any = null;
    spyOn(albumService, 'getAlbumById').and.returnValue(of(mockAlbum));

    // Act
    fixture.detectChanges();
    tick();

    // Assert
    expect(albumService.getAlbumById).toHaveBeenCalledWith('1');
    expect(component.album).toEqual({ id: '', name: '', bandId: '' });
    expect(component.musics).toEqual([]);
  }));
});
