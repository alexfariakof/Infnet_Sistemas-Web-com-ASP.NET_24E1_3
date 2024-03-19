import { Component, OnInit } from '@angular/core';
import { Playlist } from 'src/app/model';
import { PlaylistManagerService } from '../../services';

@Component({
  selector: 'app-favorites-bar',
  templateUrl: './favorites-bar.component.html',
  styleUrls: ['./favorites-bar.component.css']
})
export class FavoritesBarComponent implements OnInit {
  myPlaylist: Playlist[] = [];

  constructor(private playlistManagerService: PlaylistManagerService) { }

  ngOnInit(): void {
    this.playlistManagerService.playlists$.subscribe(playlists => {
      this.myPlaylist = playlists;
    });
  }
}
