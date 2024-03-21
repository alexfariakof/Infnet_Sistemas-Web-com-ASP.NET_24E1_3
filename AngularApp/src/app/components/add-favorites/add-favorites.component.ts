import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Playlist } from 'src/app/model';
import { PlaylistManagerService } from '../../services';
@Component({
  selector: 'app-add-favorites',
  templateUrl: './add-favorites.component.html',
  styleUrls: ['./add-favorites.component.css']
})
export class AddFavoritesComponent implements OnInit {
  @Output() addToFavoritesEvent: EventEmitter<string> = new EventEmitter<string>();

  myPlaylist: Playlist[] = [];
  constructor(private playlistManagerService: PlaylistManagerService) { }

  ngOnInit(): void {
    this.myPlaylist = this.playlistManagerService.getCachedPlaylist();
    if (this.myPlaylist.length === 0)
        this.playlistManagerService.playlists$.subscribe(playlists => {
            this.myPlaylist = playlists;
    });

  }

  onAddToFavoritesClick(playlistId?: string): void {
    this.addToFavoritesEvent.emit(playlistId);
  }
}
