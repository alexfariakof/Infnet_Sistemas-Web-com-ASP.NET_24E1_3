import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { MyplaylistComponent } from './myplaylist.component';
import { ActivatedRoute } from '@angular/router';
import { MyPlaylistService } from 'src/app/services';
import { of, throwError } from 'rxjs';
import { Playlist, Music } from 'src/app/model';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('MyplaylistComponent', () => {
  let component: MyplaylistComponent;
  let fixture: ComponentFixture<MyplaylistComponent>;
  let myPlaylistService: MyPlaylistService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MyplaylistComponent],
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({ playlistId: '1' })
          }
        },
        MyPlaylistService
      ]
    });
    fixture = TestBed.createComponent(MyplaylistComponent);
    component = fixture.componentInstance;
    myPlaylistService = TestBed.inject(MyPlaylistService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve playlist on initialization', fakeAsync(() => {
    // Arrange
    const mockPlaylist: Playlist = {
      id: '1',
      name: 'Playlist 1',
      musics: [
        { id: '1', name: 'Music 1', duration: 20 },
        { id: '2', name: 'Music 2', duration: 30 }
      ]
    };
    spyOn(myPlaylistService, 'getPlaylist').and.returnValue(of(mockPlaylist));

    // Act
    fixture.detectChanges();
    tick();

    // Assert
    expect(myPlaylistService.getPlaylist).toHaveBeenCalledWith('1');
    expect(component.musics).toEqual(mockPlaylist.musics);
  }));

  it('should handle error when retrieving playlist', fakeAsync(() => {
    // Arrange
    const errorMessage = 'Error retrieving playlist';
    spyOn(myPlaylistService, 'getPlaylist').and.returnValue(throwError(errorMessage));

    // Act
    fixture.detectChanges();
    tick();

    // Assert
    expect(myPlaylistService.getPlaylist).toHaveBeenCalledWith('1');
    expect(component.musics).toEqual([]);
  }));

  it('should handle null response when retrieving playlist', fakeAsync(() => {
    // Arrange
    const mockPlaylist: Playlist| any = null;
    spyOn(myPlaylistService, 'getPlaylist').and.returnValue(of(mockPlaylist));

    // Act
    fixture.detectChanges();
    tick();

    // Assert
    expect(myPlaylistService.getPlaylist).toHaveBeenCalledWith('1');
    expect(component.musics).toEqual([]);
  }));

});
