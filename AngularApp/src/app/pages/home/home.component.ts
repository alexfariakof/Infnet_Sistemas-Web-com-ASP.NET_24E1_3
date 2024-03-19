import { Component, OnInit } from '@angular/core';
import { Playlist } from 'src/app/model';
import { PlaylistService } from 'src/app/services';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  bands: [] = [];
  hasStyle: string = '';
  playlist: Playlist[] = [];

  constructor(public playlistService: PlaylistService) { }

  ngOnInit() : void {
    this.getListOfPlaylist();
  }

  getListOfPlaylist = (): void => {
    this.playlistService.getAllPlaylist()
    .subscribe({
      next: (response: Playlist[]) => {
        if (response != null)
          this.playlist = response;
      },
      error: (response: any) => {
        console.log(response.error);
      },
      complete() { }
    });
  }
}
