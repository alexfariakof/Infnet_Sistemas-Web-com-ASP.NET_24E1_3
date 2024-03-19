import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { HomeComponent } from './home.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';
import { Playlist } from 'src/app/model';
import { PlaylistService } from 'src/app/services';
import { RouterTestingModule } from '@angular/router/testing';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let playlistService: PlaylistService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    playlistService = TestBed.inject(PlaylistService);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve list of playlists on initialization', fakeAsync(() => {
    // Arrange
    const mockPlaylists: Playlist[] = [
      { id: '1', name: 'Playlist 1', musics: [] },
      { id: '2', name: 'Playlist 2', musics: [] }
    ];

    spyOn(playlistService, 'getAllPlaylist').and.returnValue(of(mockPlaylists));

    // Act
    component.getListOfPlaylist();
    fixture.detectChanges();
    tick();

    // Assert
    expect(playlistService.getAllPlaylist).toHaveBeenCalled();
    expect(component.playlist).toEqual(mockPlaylists);
  }));

  it('should handle error when retrieving list of playlists', fakeAsync(() => {
    // Arrange
    const errorMessage = 'Error retrieving playlists';
    spyOn(playlistService, 'getAllPlaylist').and.returnValue(throwError(() => errorMessage));

    // Act
    component.getListOfPlaylist();
    fixture.detectChanges();
    tick();

    // Assert
    expect(playlistService.getAllPlaylist).toHaveBeenCalled();
    expect(component.playlist).toEqual([]);
  }));

  it('should handle empty response when retrieving list of playlists', fakeAsync(() => {
    // Arrange
    const mockPlaylists: Playlist[] = [];
    spyOn(playlistService, 'getAllPlaylist').and.returnValue(of(mockPlaylists));

    // Act
    component.getListOfPlaylist();
    fixture.detectChanges();
    tick();

    // Assert
    expect(playlistService.getAllPlaylist).toHaveBeenCalled();
    expect(component.playlist).toEqual([]);
  }));
});
