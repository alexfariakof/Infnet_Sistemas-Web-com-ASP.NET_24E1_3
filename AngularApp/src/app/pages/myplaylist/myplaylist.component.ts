import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Music, Playlist } from 'src/app/model';
import { MyPlaylistService } from 'src/app/services';
@Component({
  selector: 'app-myplaylist',
  templateUrl: './myplaylist.component.html',
  styleUrls: ['./myplaylist.component.css']
})
export class MyplaylistComponent implements OnInit {
  hasStyle: string = '';
  myPlaylist: Playlist[] = [];
  musics: Music[] = [];

  constructor(
    private route: ActivatedRoute,
    public myPlaylistService: MyPlaylistService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const playlistId = params['playlistId'];
        this.getMyplaylist(playlistId);
    });
  }

  getMyplaylist = (playlistId: string ): void => {
    this.myPlaylistService.getPlaylist(playlistId).subscribe({
      next: (response: Playlist) => {
        if (response != null) {
          this.musics = response.musics;
        }
        else {
          throw (response);
        }
      },
      error: (response: any) => {
        console.log(response.error);
      },
      complete() { }
    });
  }
}
