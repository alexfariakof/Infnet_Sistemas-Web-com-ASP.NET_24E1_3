import { Music } from "./music";

export interface Playlist {
  id: string;
  name: string;
  musics: Music[];
}
