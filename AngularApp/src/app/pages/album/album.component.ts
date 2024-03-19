import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Album, Music} from 'src/app/model';
import { AlbumService } from 'src/app/services';

@Component({
  selector: 'app-album',
  templateUrl: './album.component.html',
  styleUrls: ['./album.component.css']
})
export class AlbumComponent implements OnInit {
  album: Album = {
    id: '',
    name: '',
    bandId: ''
  };
  musics: Music[] = [];
  hasStyle: string = '';

  constructor(
    private route: ActivatedRoute,
    public albumService: AlbumService
    ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const albumId = params['albumId'];
      this.getAlbum(albumId);
    });
  }

  getAlbum = (albumId: string): void => {
    this.albumService.getAlbumById(albumId).subscribe({
      next: (response: Album) => {
        if (response != null) {
          this.album = response;
          this.musics = response.musics ?? [];
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
