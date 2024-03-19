import { Music } from "./music";

export interface Album {
  id: string;
  name: string;
  flatId?: string;
  bandId: string;
  musics?: Music[];
}
