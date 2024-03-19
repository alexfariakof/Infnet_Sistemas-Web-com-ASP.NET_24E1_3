import { Component, OnInit } from '@angular/core';
import { Playlist } from 'src/app/model';
import { PlaylistManagerService } from '../../services';
@Component({
  selector: 'app-add-favorites',
  templateUrl: './add-favorites.component.html',
  styleUrls: ['./add-favorites.component.css']
})
export class AddFavoritesComponent implements OnInit {
  myPlaylist: Playlist[] = [];

  constructor(private playlistManagerService: PlaylistManagerService) { }

  ngOnInit(): void {
    this.playlistManagerService.playlists$.subscribe(playlists => {
      this.myPlaylist = playlists;
    });
  }

}
